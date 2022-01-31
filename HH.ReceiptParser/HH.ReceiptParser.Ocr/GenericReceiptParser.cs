using MathNet.Spatial.Euclidean;
using MathNet.Spatial.Units;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HH.ReceiptParser.Ocr
{
  public class GenericReceiptParser : IReceiptParser
  {
    public ParseResult ParseReceipt(IEnumerable<RecognitionResult> ocrText)
    {
      ParseResult result = new ParseResult();
      List<LineItem> lineItems = new List<LineItem>();

      IEnumerable<string> lines = this.FlattenRecognitionResults(ocrText);

      foreach(string line in lines)
      {
        lineItems.Add(this.ParseLine(line));
      }

      result.LineItems = this.ConsolidateLineItems(lineItems);

      return result;
    }

    private Polygon2D GetBoundingBoxPolygon(BoundingBox boundingBox)
    {
      Polygon2D polygon = new Polygon2D(
        new Point2D(boundingBox.TopLeft.X, boundingBox.TopLeft.Y),
        new Point2D(boundingBox.TopRight.X, boundingBox.TopRight.Y),
        new Point2D(boundingBox.BottomRight.X, boundingBox.BottomRight.Y),
        new Point2D(boundingBox.BottomLeft.X, boundingBox.BottomLeft.Y));

      return polygon;
    }

    private IEnumerable<string> FlattenRecognitionResults(IEnumerable<RecognitionResult> recognitionResults)
    {
      List<RecognitionResult> ocrTextSet = new List<RecognitionResult>(recognitionResults);
      List<string> ocrLines = new List<string>();

      Angle tolerance = Angle.FromDegrees(10);

      while (ocrTextSet.Count > 0)
      {
        RecognitionResult ocrLine = ocrTextSet[0];
        ocrTextSet.Remove(ocrLine);

        StringBuilder line = new StringBuilder();
        line.Append(ocrLine.Text);

        Polygon2D bb1 = this.GetBoundingBoxPolygon(ocrLine.BoundingBox);

        List<RecognitionResult> toRemove = new List<RecognitionResult>();

        foreach (RecognitionResult ocrLine2 in ocrTextSet)
        {
          Polygon2D bb2 = this.GetBoundingBoxPolygon(ocrLine2.BoundingBox);

          bool topAligned = bb1.Edges.ElementAt(0).IsParallelTo(bb2.Edges.ElementAt(0), tolerance);
          bool bottomAligned = bb1.Edges.ElementAt(2).IsParallelTo(bb2.Edges.ElementAt(2), tolerance);

          bool horizontallySeparated = bb1.Edges.ElementAt(0).EndPoint.X < bb2.Edges.ElementAt(0).StartPoint.X;

          double overlap = this.PercentageVertialOverlap(bb1.Edges.ElementAt(1), bb2.Edges.ElementAt(1));

          if (topAligned && bottomAligned && horizontallySeparated && overlap > 0.55)
          {
            toRemove.Add(ocrLine2);
            line.Append(";");
            line.Append(ocrLine2.Text);
          }
        }

        foreach (RecognitionResult r in toRemove)
        {
          ocrTextSet.Remove(r);
        }

        ocrLines.Add(line.ToString());
      }

      return ocrLines;
    }

    private LineItem ParseLine(string line)
    {
      LineItem parseResult = new LineItem();
      parseResult.OcrValue = line;

      if (line.LastIndexOf(";") > 0)
      {
        if (line.Contains("@"))
        {
          string quantity = line.Substring(0, line.IndexOf("@")).Replace(";", "").ToUpper();
          
          foreach (string uom in UnitsOfMeasure.UOMs)
          {
            foreach (string uomSyn in UnitsOfMeasure.UOMSynonyms[uom])
            {
              if (quantity.Contains(uomSyn))
              {
                parseResult.UnitOfMeasure = uom;
                quantity = quantity.Replace(uomSyn, "").Trim();
                break;
              }
            }

            if (!string.IsNullOrEmpty(parseResult.UnitOfMeasure))
            {
              break;
            }
          }

          parseResult.Quantity = double.Parse(quantity);

          string unitPrice = line
            .Substring(line.IndexOf("@") + 1)
            .Replace(";", "")
            .Replace("$", "")
            .ToUpper()
            .Trim();

          if (unitPrice.Contains(" "))
          {
            unitPrice = unitPrice.Substring(0, unitPrice.IndexOf(" "));
          }

          unitPrice = unitPrice.Replace("/", "");

          if (!string.IsNullOrWhiteSpace(parseResult.UnitOfMeasure))
          {
            foreach (string uomSyn in UnitsOfMeasure.UOMSynonyms[parseResult.UnitOfMeasure])
            {
              if (unitPrice.Contains(uomSyn))
              {
                unitPrice = unitPrice.Replace(uomSyn, "").Trim();
                break;
              }
            }
          }
          else
          {
            string uomContent = line
            .Substring(line.IndexOf("@") + 1)
            .Replace(";", "")
            .Replace("/", "")
            .Replace(unitPrice, "")
            .ToUpper()
            .Trim();

            foreach (string uom in UnitsOfMeasure.UOMs)
            {
              foreach (string uomSyn in UnitsOfMeasure.UOMSynonyms[uom])
              {
                if (uomContent.StartsWith(uomSyn))
                {
                  parseResult.UnitOfMeasure = uom;
                  break;
                }
              }

              if (!string.IsNullOrEmpty(parseResult.UnitOfMeasure))
              {
                break;
              }
            }
          }

          parseResult.Cost = double.Parse(unitPrice);
        }
        else
        {
          parseResult.Name = line.Substring(0, line.LastIndexOf(";")).Replace(";", " ");

          double cost = 0;
          if (double.TryParse(line.Substring(line.LastIndexOf(";") + 1).Replace("$", ""), out cost))
          {
            parseResult.Cost = cost;
            parseResult.Quantity = 1;
          }
          else
          {
            parseResult.Name += " " + line.Substring(line.LastIndexOf(";") + 1);
          }
        }
      }
      else
      {
        parseResult.Name = line;
      }

      return parseResult;
    }

    private IEnumerable<LineItem> ConsolidateLineItems(IEnumerable<LineItem> lineItems)
    {
      List<LineItem> result = new List<LineItem>();

      for (int i = 0; i < lineItems.Count(); i++)
      {
        LineItem current = lineItems.ElementAt(i);

        if (current.Name == current.OcrValue && i + 1 < lineItems.Count())
        {
          LineItem next = lineItems.ElementAt(i + 1);

          if (!string.IsNullOrEmpty(next.UnitOfMeasure))
          {
            LineItem consolidated = new LineItem()
            {
              OcrValue = current.OcrValue + "\r\n" + next.OcrValue,
              Name = current.Name,
              Cost = next.Cost,
              UnitOfMeasure = next.UnitOfMeasure,
              Quantity = next.Quantity
            };

            result.Add(consolidated);
            i++;
            continue;
          }
          else
          {
            // TODO: discard but track that we discarded it
          }
        }
        else
        {
          if (current.Name != null && !SpecialLineNames.IsSpecialName(current))
          {
            result.Add(current);
          }
        }
      }

      return result;
    }

    private double PercentageVertialOverlap(LineSegment2D line1, LineSegment2D line2)
    {
      double overlap = 0;

      if (line1.StartPoint.Y <= line2.StartPoint.Y && line1.EndPoint.Y > line2.StartPoint.Y)
      {
        overlap = (line1.EndPoint.Y - line2.StartPoint.Y) / line1.Length;
      }
      else if (line1.StartPoint.Y >= line2.StartPoint.Y && line1.StartPoint.Y < line2.EndPoint.Y)
      {
        overlap = (line2.EndPoint.Y - line1.StartPoint.Y) / line1.Length;
      }
      else if (line2.StartPoint.Y > line1.StartPoint.Y && line2.EndPoint.Y < line1.EndPoint.Y)
      {
        overlap = 1;
      }
      else if (line1.StartPoint.Y > line2.StartPoint.Y && line1.EndPoint.Y < line2.EndPoint.Y)
      {
        overlap = 1;
      }

      return overlap;
    }
  }
}

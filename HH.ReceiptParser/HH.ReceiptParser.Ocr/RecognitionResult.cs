using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HH.ReceiptParser.Ocr
{
  public class RecognitionResult
  {
    private string text;
    private BoundingBox boundingBox;
    private double angle;

    public RecognitionResult(string text, float[] boundingBox, double angle)
    {
      this.text = text;
      this.boundingBox = new BoundingBox(boundingBox);
      this.angle = angle;
    }

    public string Text { 
      get 
      { 
        return this.text; 
      } 
    }

    public BoundingBox BoundingBox 
    { 
      get 
      { 
        return this.boundingBox; 
      }
    }

    public double Angle
    {
      get
      {
        return this.angle;
      }
    }
  }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HH.ReceiptParser.Ocr
{
  public class BoundingBox
  {
    private PointF topLeft;
    private PointF topRight;
    private PointF bottomRight;
    private PointF bottomLeft;

    public BoundingBox(float[] points)
    {
      if (points == null)
      {
        throw new ArgumentNullException(nameof(points));
      }
      else if (points.Length != 8)
      {
        throw new ArgumentException(nameof(points));
      }

      this.topLeft = new PointF(points[0], points[1]);
      this.topRight = new PointF(points[2], points[3]);
      this.bottomRight = new PointF(points[4], points[5]);
      this.bottomLeft = new PointF(points[6], points[7]);
    }

    public PointF TopLeft 
    {
      get
      {
        return this.topLeft;
      }

      set
      {
        this.topLeft = value;
      }
    }

    public PointF TopRight
    {
      get
      {
        return this.topRight;
      }

      set
      {
        this.topRight = value;
      }
    }

    public PointF BottomRight
    {
      get
      {
        return this.bottomRight;
      }

      set
      {
        this.bottomRight = value;
      }
    }

    public PointF BottomLeft
    {
      get
      {
        return this.bottomLeft;
      }

      set
      {
        this.bottomLeft = value;
      }
    }
  }
}

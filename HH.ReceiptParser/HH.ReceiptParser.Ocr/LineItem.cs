using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HH.ReceiptParser.Ocr
{
  public class LineItem
  {
    /// <summary>
    /// Gets or sets the entire recognized string that was parsed to produce
    /// this <see cref="LineItem"/>
    /// </summary>
    public string OcrValue { get; set; }

    /// <summary>
    /// Gets or sets the parsed name of this <see cref="LineItem"/>
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the known common name of this <see cref="LineItem"/>
    /// </summary>
    public string CommonName { get; set; }

    /// <summary>
    /// Gets or sets the parsed cost of this <see cref="LineItem"/>
    /// </summary>
    public double Cost { get; set; }

    /// <summary>
    /// Gets or sets the parsed quantity of this <see cref="LineItem"/>
    /// </summary>
    public double Quantity { get; set; }

    /// <summary>
    /// Gets or sets the unit of measure of this <see cref="LineItem"/>
    /// </summary>
    public string UnitOfMeasure { get; set; }
  }
}

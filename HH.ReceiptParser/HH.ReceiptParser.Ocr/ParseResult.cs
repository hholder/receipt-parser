using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HH.ReceiptParser.Ocr
{
  public class ParseResult
  {
    private List<LineItem> lineItems = new List<LineItem>();

    public IEnumerable<LineItem> LineItems 
    {
      get
      {
        return this.lineItems;
      }

      set
      {
        this.lineItems.Clear();
        this.lineItems.AddRange(value);
      }
    }
  }
}

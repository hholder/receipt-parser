using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HH.ReceiptParser.Ocr
{
  public static class SpecialLineNames
  {
    public static IEnumerable<string> Names = new List<string>()
    { 
      "SUB TOTAL", "BALANCE DUE", "CI CASH", "US CASH", "CHANGE", "TOTAL TAX",
      "TOTAL", "CARD NO", "POINTS THIS VISIT", "REWARD AVAILABLE", "ADVANTAGE CARD",
      "CASH CI", "CASH US", "TTL PROMOTIONS"
    };

    public static bool IsSpecialName(LineItem item)
    {
      return SpecialLineNames.Names.Contains(item.Name.ToUpper()) ||
        SpecialLineNames.Names.Any(name => item.OcrValue.ToUpper().StartsWith(name) && item.Cost == 0);
    }
  }
}

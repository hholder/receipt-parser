using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HH.ReceiptParser.Ocr
{
  public static class UnitsOfMeasure
  {
    public static IEnumerable<string> UOMs = new List<string>()
      { "EA", "LB" };

    public static IDictionary<string, IEnumerable<string>> UOMSynonyms =
      new Dictionary<string, IEnumerable<string>>()
      {
        { "EA", new List<string>() { "EA", "EACH" } },
        { "LB", new List<string>() { "LB", "1B", "POUNDS" } },
      };
  }
}

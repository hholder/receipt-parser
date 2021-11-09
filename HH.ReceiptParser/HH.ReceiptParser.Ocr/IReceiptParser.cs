using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HH.ReceiptParser.Ocr
{
  public interface IReceiptParser
  {
    ParseResult ParseReceipt(IEnumerable<RecognitionResult> ocrText);
  }
}

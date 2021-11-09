using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace HH.ReceiptParser.Ocr
{
  public interface IRecognizer
  {
    public Task<IEnumerable<RecognitionResult>> Recognize(byte[] image);
  }
}

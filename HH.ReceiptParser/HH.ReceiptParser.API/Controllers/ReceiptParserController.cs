using HH.ReceiptParser.Data;
using HH.ReceiptParser.Ocr;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace HH.ReceiptParser.API.Controllers
{
  public class ReceiptParserController : Controller
  {
    private const int READ_BUFFER_SIZE = 1024;

    private ICommonNameRepository commonNameRepo;
    private IReceiptParser receiptParser;
    private IRecognizer recognizer;

    public ReceiptParserController(
      ICommonNameRepository commonNameRepo,
      IReceiptParser receiptParser,
      IRecognizer recognizer)
    {
      this.commonNameRepo = commonNameRepo;
      this.receiptParser = receiptParser;
      this.recognizer = recognizer;
    }

    [HttpPost]
    public async Task<ParseResult> ParseReceipt(Stream imageStream)
    {
      MemoryStream imageBuffer = new MemoryStream();
      int bufferPosition = 0;

      while (imageStream.Position < imageStream.Length)
      {
        byte[] readBuffer = new byte[ReceiptParserController.READ_BUFFER_SIZE];
        int bytesRead = imageStream.Read(readBuffer, 0, ReceiptParserController.READ_BUFFER_SIZE);
        imageBuffer.Write(readBuffer, 0, bytesRead);
        bufferPosition += bytesRead;
      }

      IEnumerable<RecognitionResult> recogResult = await this.recognizer.Recognize(imageBuffer.ToArray());
      ParseResult parsedReceipt = this.receiptParser.ParseReceipt(recogResult);

      return parsedReceipt;
    }
  }
}

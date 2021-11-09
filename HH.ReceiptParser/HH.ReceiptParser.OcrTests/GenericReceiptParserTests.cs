using Microsoft.VisualStudio.TestTools.UnitTesting;
using HH.ReceiptParser.Ocr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HH.ReceiptParser.Ocr.Tests
{
  [TestClass()]
  public class GenericReceiptParserTests
  {
    [TestMethod]
    [DeploymentItem("data/PosAngle.jpg")]
    [DeploymentItem("data/NegAngle.jpg")]
    public void ParseReceiptTest()
    {
      AzureRecognizer azr = new AzureRecognizer(
        "f7d6bb41e1284d42b9935a92bdfff8de",
        "https://eastus.api.cognitive.microsoft.com/");

      byte[] image = File.ReadAllBytes("IMG_20211022_194235.jpg");
      IEnumerable<RecognitionResult> result = azr.Recognize(image).GetAwaiter().GetResult();

      //byte[] posangle = File.ReadAllBytes("PosAngle.jpg");
      //IEnumerable<RecognitionResult> pos = azr.Recognize(posangle).GetAwaiter().GetResult();

      //byte[] negangle = File.ReadAllBytes("NegAngle.jpg");
      //IEnumerable<RecognitionResult> neg = azr.Recognize(negangle).GetAwaiter().GetResult();

      GenericReceiptParser parser = new GenericReceiptParser();
      ParseResult parseResult = parser.ParseReceipt(result);
    }
  }
}
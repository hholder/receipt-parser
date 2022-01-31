using Microsoft.VisualStudio.TestTools.UnitTesting;
using HH.ReceiptParser.Ocr;
using System.IO;

namespace HH.ReceiptParser.API.Controllers.Tests
{
  [TestClass]
  public class ReceiptParserControllerTests
  {
    [TestMethod]
    [DeploymentItem("data/IMG_20211113_192456.jpg")]
    public void ParseReceiptTest()
    {
      ReceiptParserController controller = new ReceiptParserController(
        null,
        new GenericReceiptParser(),
        new AzureRecognizer(
          "f7d6bb41e1284d42b9935a92bdfff8de",
          "https://eastus.api.cognitive.microsoft.com/"));

      FileStream imageStream = File.OpenRead("IMG_20211113_192456.jpg");

      ParseResult result = controller.ParseReceipt(imageStream).GetAwaiter().GetResult();

      Assert.IsNotNull(result);
    }
  }
}
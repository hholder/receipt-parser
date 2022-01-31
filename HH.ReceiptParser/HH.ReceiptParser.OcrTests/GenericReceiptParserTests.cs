using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace HH.ReceiptParser.Ocr.Tests
{
  [TestClass]
  public class GenericReceiptParserTests
  {
    [TestMethod]
    [DeploymentItem("data/IMG_20211113_192456.jpg")]
    public void ParseReceipt_Hurleys_ItemsParsed()
    {
      AzureRecognizer azr = new AzureRecognizer(
        "f7d6bb41e1284d42b9935a92bdfff8de",
        "https://eastus.api.cognitive.microsoft.com/");

      byte[] image = File.ReadAllBytes("IMG_20211113_192456.jpg");
      IEnumerable<RecognitionResult> result = azr.Recognize(image).GetAwaiter().GetResult();

      GenericReceiptParser parser = new GenericReceiptParser();
      ParseResult parseResult = parser.ParseReceipt(result);

      Assert.AreEqual(11, parseResult.LineItems.Count());
      Assert.IsNotNull(parseResult.LineItems.FirstOrDefault(i => i.Name == "ESSEV CHUNK COLB" && i.Cost == 3.59));
      Assert.IsNotNull(parseResult.LineItems.FirstOrDefault(i => i.Name == "MORTO SEA SALT I" && i.Cost == 2.79));
    }

    [TestMethod]
    [DeploymentItem("data/IMG_20220128_224626.jpg")]
    public void ParseReceipt_Fosters_ItemsParsed()
    {
      AzureRecognizer azr = new AzureRecognizer(
        "f7d6bb41e1284d42b9935a92bdfff8de",
        "https://eastus.api.cognitive.microsoft.com/");

      byte[] image = File.ReadAllBytes("IMG_20220128_224626.jpg");
      IEnumerable<RecognitionResult> result = azr.Recognize(image).GetAwaiter().GetResult();

      GenericReceiptParser parser = new GenericReceiptParser();
      ParseResult parseResult = parser.ParseReceipt(result);

      Assert.AreEqual(16, parseResult.LineItems.Count());
      Assert.IsNotNull(parseResult.LineItems.FirstOrDefault(i => i.Name == "JOY SUGAR CONES" && i.Cost == 2.89));
      Assert.IsNotNull(parseResult.LineItems.FirstOrDefault(i => i.Name == "SHIRLEY ORIGINAL CKY" && i.Cost == 1.69));
    }
  }
}
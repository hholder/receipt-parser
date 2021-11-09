using Microsoft.VisualStudio.TestTools.UnitTesting;
using HH.ReceiptParser.Ocr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http.Headers;

namespace HH.ReceiptParser.Ocr.Tests
{
  [TestClass]
  public class AzureRecognizerTests
  {
    [TestMethod]
    [DeploymentItem("data/IMG_20211022_194235.jpg")]
    public void Recognize_ReceiptImage_ReceiptLinesReturned()
    {
      AzureRecognizer azr = new AzureRecognizer(
        "f7d6bb41e1284d42b9935a92bdfff8de",
        "https://eastus.api.cognitive.microsoft.com/");

      byte[] image = File.ReadAllBytes("IMG_20211022_194235.jpg");
      IEnumerable<RecognitionResult> result = azr.Recognize(image).GetAwaiter().GetResult();

      Assert.AreEqual(66, result.Count());
    }
  }
}
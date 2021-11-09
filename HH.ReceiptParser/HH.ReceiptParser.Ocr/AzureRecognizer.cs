using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace HH.ReceiptParser.Ocr
{
  public class AzureRecognizer : IRecognizer
  {
    private const int ID_LENGTH = 36;

    private string key;
    private string endpoint;

    public AzureRecognizer(string key, string endpoint)
    {
      this.key = key;
      this.endpoint = endpoint;
    }

    public async Task<IEnumerable<RecognitionResult>> Recognize(byte[] image)
    {
      List<RecognitionResult> result = new List<RecognitionResult>();

      try
      {
        HttpClient webClient = new HttpClient();
        NameValueCollection queryString = HttpUtility.ParseQueryString(string.Empty);

        // Request headers
        webClient.DefaultRequestHeaders.Add(
          "Ocp-Apim-Subscription-Key", "f7d6bb41e1284d42b9935a92bdfff8de");

        // Request parameters
        queryString["readingOrder"] = "natural";
        string uri = this.endpoint + "vision/v3.2/read/analyze?" + queryString;

        string resultUri = string.Empty;
        HttpResponseMessage response;

        // Request body
        using (var content = new ByteArrayContent(image))
        {
          content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
          response = await webClient.PostAsync(uri, content);
          resultUri = response.Headers.GetValues("Operation-Location").FirstOrDefault();
        }

        ReadOperationResult operationResult = null;

        do
        {
          response = await webClient.GetAsync(resultUri);
          string responseContent = await response.Content.ReadAsStringAsync();
          operationResult = JsonConvert.DeserializeObject<ReadOperationResult>(responseContent);
          Thread.Sleep(1000);
        } while (operationResult.Status == OperationStatusCodes.NotStarted ||
                 operationResult.Status == OperationStatusCodes.Running);

        if (operationResult.Status == OperationStatusCodes.Succeeded)
        {
          foreach (ReadResult readResult in operationResult.AnalyzeResult.ReadResults)
          {
            result.AddRange(readResult.Lines.Select(l =>
            {
              float[] boundingBox = new float[l.BoundingBox.Count];
              int index = 0;

              foreach (double d in l.BoundingBox)
              {
                boundingBox[index] = (float)d;
                index++;
              }

              return new RecognitionResult(l.Text, boundingBox, readResult.Angle);
            }));
          }
        }
      }
      catch (Exception x)
      {
        x.ToString();
      }

      return result;
    }
  }
}

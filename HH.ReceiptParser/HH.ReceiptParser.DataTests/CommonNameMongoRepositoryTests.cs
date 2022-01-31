using Microsoft.VisualStudio.TestTools.UnitTesting;
using HH.ReceiptParser.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HH.ReceiptParser.Data.Tests
{
  [TestClass]
  public class CommonNameMongoRepositoryTests
  {
    [TestMethod]
    public void RemoveCommonName_NameDoesNotExits_NothingRemoved()
    {
      ItemCommonName testCN = new ItemCommonName()
      {
        CommonName = "TestCN",
        Aliases = new List<string>() { "test-cn", "test cn" }
      };

      CommonNameMongoRepository repo = new CommonNameMongoRepository("mongodb://localhost");
      int result = repo.RemoveCommonName(testCN);

      Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void AddCommonName_NewCommonName_NameAdded()
    {
      ItemCommonName testCN = new ItemCommonName()
      {
        CommonName = "TestCN",
        Aliases = new List<string>() { "test-cn", "test cn" }
      };

      CommonNameMongoRepository repo = new CommonNameMongoRepository("mongodb://localhost");
      repo.AddCommonName(testCN);

      ItemCommonName result = repo.Find(testCN.CommonName).FirstOrDefault();

      Assert.AreEqual(testCN.CommonName, result.CommonName);
      CollectionAssert.AreEquivalent(testCN.Aliases.ToList(), result.Aliases.ToList());
    }

    [TestCleanup]
    public void TestCleanup()
    {
      ItemCommonName testCN = new ItemCommonName()
      {
        CommonName = "TestCN",
        Aliases = new List<string>() { "test-cn", "test cn" }
      };

      CommonNameMongoRepository repo = new CommonNameMongoRepository("mongodb://localhost");
      repo.RemoveCommonName(testCN);
    }
  }
}
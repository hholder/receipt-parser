using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HH.ReceiptParser.Data
{
  public class CommonNameMongoRepository : ICommonNameRepository
  {
    private IMongoCollection<ItemCommonName> commonNamesCollection;

    public CommonNameMongoRepository(string connectionString)
    {
      MongoClient mongoClient = new MongoClient(connectionString);
      IMongoDatabase db = mongoClient.GetDatabase("shopping-lists");
      this.commonNamesCollection = db.GetCollection<ItemCommonName>("item-names");
    }

    public void AddCommonName(ItemCommonName commonName)
    {
      this.commonNamesCollection.InsertOne(commonName);
    }

    public IEnumerable<ItemCommonName> Find(string commonName)
    {
      return this.commonNamesCollection.Find(item => item.CommonName == commonName).ToEnumerable();
    }

    public IEnumerable<ItemCommonName> FindByAlias(string alias)
    {
      return this.commonNamesCollection.Find(item => item.Aliases.Contains(alias)).ToEnumerable();
    }

    public int RemoveCommonName(ItemCommonName commonName)
    {
      return (int)this.commonNamesCollection.DeleteOne(item => item.CommonName == commonName.CommonName).DeletedCount;
    }

    public void UpdateCommonName(ItemCommonName commonName)
    {
      this.commonNamesCollection.UpdateOne(
        item => item.CommonName == commonName.CommonName,
        $"{{ set: {{ Aliases: {commonName.Aliases} }} }}");
    }
  }
}

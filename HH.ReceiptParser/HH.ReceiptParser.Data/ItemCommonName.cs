using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace HH.ReceiptParser.Data
{
  public class ItemCommonName
  {
    private List<string> aliases = new List<string>();

    [BsonElement("_id")]
    public ObjectId Id { get; set; }

    public string CommonName { get; set; }

    public IEnumerable<string> Aliases 
    {
      get
      {
        return this.aliases;
      }

      set
      {
        this.aliases.Clear();
        this.aliases.AddRange(value);
      }
    }

    public void AddAlias(string alias)
    {
      if (!string.IsNullOrWhiteSpace(alias))
      {
        this.aliases.Add(alias);
      }
    }

    public bool RemoveAlias(string alias)
    {
      return this.aliases.Remove(alias);
    }
  }
}

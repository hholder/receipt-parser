using System.Collections.Generic;

namespace HH.ReceiptParser.Data
{
  public interface ICommonNameRepository
  {
    void AddCommonName(ItemCommonName commonName);

    void UpdateCommonName(ItemCommonName commonName);

    int RemoveCommonName(ItemCommonName commonName);

    IEnumerable<ItemCommonName> Find(string commonName);

    IEnumerable<ItemCommonName> FindByAlias(string alias);
  }
}

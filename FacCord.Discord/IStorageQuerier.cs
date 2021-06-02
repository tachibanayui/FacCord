using IsekaiTechnologies.FacCord.Core.Models.Storages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IsekaiTechnologies.FacCord.Discord
{
    public interface IStorageQuerier
    {
        IEnumerable<Type> GetFilters();
        IEnumerable<StorageItem> GetItemPool();
        string Keyword { get; }


        void SetKeywords(string query);
        void AddItemFilter(Type type);
        void RemoveItemFilter(Type type);
        void RemoveAllFilters();
        void SetItemPool(IEnumerable<StorageItem> items);

        IAsyncEnumerable<StorageItem> Search();
    }
}

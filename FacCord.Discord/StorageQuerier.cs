using IsekaiTechnologies.FacCord.Core.Models.Storages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IsekaiTechnologies.FacCord.Discord
{
    public class StorageQuerier : IStorageQuerier
    {
        public List<Type> Filters { get; set; } = new List<Type>();
        public string Keyword { get; private set; }
        private IEnumerable<StorageItem> _Pool;

        public IAsyncEnumerable<StorageItem> Search()
        {
            var sc = StringComparison.InvariantCultureIgnoreCase;

            return GetItemPool()
                 .Where(x => Filters.Count == 0 ? true : Filters.Contains(x.Item.GetType())) // Type filter
                 .Where(x => string.IsNullOrEmpty(Keyword) ? true : string.Equals(x.Item.Name, Keyword, sc) || string.Equals(x.Item.IdName, Keyword, sc)) // Search keyword
                 .ToAsyncEnumerable();
        }

        public void AddItemFilter(Type type)
        {
            if ( typeof(Item).IsAssignableFrom(type))
            {
                Filters.Add(type);
            }
            else
            {
                throw new ArgumentException($"{type.Name} is not an Item");
            }
        }

        public IEnumerable<Type> GetFilters()
        {
            return Filters;
        }

        public void RemoveAllFilters()
        {
            Filters.Clear();
        }

        public void RemoveItemFilter(Type type)
        {
            Filters.Remove(type);
        }

        
        public void SetItemPool(IEnumerable<StorageItem> items)
        {
            _Pool = items;
        }

        public IEnumerable<StorageItem> GetItemPool()
        {
            return _Pool;
        }


        public void SetKeywords(string query)
        {
            Keyword = query;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IsekaiTechnologies.FacCord.Core.Models.Storages.ItemMetas
{
    public class ItemMeta
    {
        public long Id { get; set; }
        public string DisplayName { get; set; }

        public virtual Task<bool> IsMergeable(ItemMeta other)
        {
            return Task.FromResult(other == null || other.GetType() == typeof(ItemMeta) && other.DisplayName == DisplayName);
        }
    }
}

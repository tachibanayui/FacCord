using IsekaiTechnologies.FacCord.Core.Models.Storages.ItemMetas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;

namespace IsekaiTechnologies.FacCord.Core.Models.Storages
{
    public class StorageItem : ICloneable
    {
        public long Id { get; set; }
        // Item ID must be unique
        public Item Item { get; set; }
        public ItemMeta ItemMeta { get; set; }
        public long Count { get; set; }

        public StorageItem Clone()
        {
            return MemberwiseClone() as StorageItem;
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        public async Task<bool> Merge(StorageItem other)
        {
            if (await ItemMeta.IsMergeable(other.ItemMeta))
            {
                Count += other.Count;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

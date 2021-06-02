using IsekaiTechnologies.FacCord.Core.Models.Storages.ItemMetas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace IsekaiTechnologies.FacCord.Core.Models.Storages
{
    public class Item
    {
        public long Id { get; set; }
        /// <summary>
        /// Used for indentify item in code and discord admin command. Ex: faccord:stone
        /// </summary>
        [MaxLength(100)]
        public string IdName { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(1000)]
        public string Description { get; set; }

        public virtual Task<ItemMeta> CreateItemMeta()
        {
            return Task.FromResult(new ItemMeta() { DisplayName = Name });
        }
    }
}

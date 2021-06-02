using IsekaiTechnologies.FacCord.Core.Models.Equipments;
using IsekaiTechnologies.FacCord.Core.Models.Storages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IsekaiTechnologies.FacCord.Core.Models.Mines
{
    /// <summary>
    /// representing a block in a mine
    /// </summary>
    public class Block
    {
        public long Id { get; set; }

        public Item Item { get; set; }
        public int Hardness { get; set; }
        [NotMapped]
        public bool IsUnbreakable => Hardness == -1;
        public int Toughness { get; set; }
        public bool IsSolid { get; set; }
        // The block to be placed when this block have been broken
        public Block Replacement { get; set; }
        /// <summary>
        /// Dropped items when broken without silk touch
        /// </summary>
        public List<StorageItem> DropTable { get; set; }

        public List<StorageItem> Break(Drill breaker) 
            =>  DropTable ?? new List<StorageItem>() { new StorageItem() { Item = Item, Count = 1 } };
    }
}

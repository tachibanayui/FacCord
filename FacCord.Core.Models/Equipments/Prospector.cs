using IsekaiTechnologies.FacCord.Core.Models.Storages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IsekaiTechnologies.FacCord.Core.Models.Equipments
{
    public class Prospector : Item
    {
        public short RangeX { get; set; }
        public short RangeY { get; set; }
        /// <summary>
        /// Only prospector region level >= this property will be able to scan
        /// </summary>
        public short RegionLevel { get; set; }
        /// <summary>
        /// The attempts to spawn this ore vein
        /// </summary>
        public short MineRichness { get; set; }

        /// <summary>
        /// How large the cluster of ore
        /// </summary>
        public short OreVein { get; set; }

        public TimeSpan ScanCooldown { get; set; }
    }
}

using System.Collections.Generic;

namespace IsekaiTechnologies.FacCord.Core.Models.Mines
{
    public class Region
    {
        public long Id { get; set; }
        public string IdName { get; set; }
        /// <summary>
        /// Only prospector region level >= this property will be able to scan
        /// </summary>
        public short RegionLevel { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<BlockSpawnChance> MaterialDistribution { get; set; }

        public Dimension Dimension { get; set; }
    }
}
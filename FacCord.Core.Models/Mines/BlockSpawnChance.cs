using System;
using System.Collections.Generic;
using System.Text;

namespace IsekaiTechnologies.FacCord.Core.Models.Mines
{
    public class BlockSpawnChance
    {
        public long Id { get; set; }
        public Block Block { get; set; }
        public double Chance { get; set; }
    }
}

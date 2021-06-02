using IsekaiTechnologies.FacCord.Core.Models.Storages;
using System;
using System.Collections.Generic;
using System.Text;

namespace IsekaiTechnologies.FacCord.Core.Models.Equipments
{
    // This might stored as xml or json config
    public class Drill : Item
    {
        public long Durability { get; set; }
        public short RangeX { get; set; }
        public short RangeY { get; set; }

        // -1 might represent inf for these colunm
        public short Efficiency { get; set; }
        public short Hardness { get; set; }
    }
}

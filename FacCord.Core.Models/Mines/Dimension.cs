using IsekaiTechnologies.FacCord.Core.Models.Storages;
using System;
using System.Collections.Generic;
using System.Text;

namespace IsekaiTechnologies.FacCord.Core.Models.Mines
{
    public class Dimension
    {
        public long Id { get; set; }
        public string IdName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Region> Regions { get; set; }
        // We can loop through each region to get this but nah
        public List<Item> PossibleResources { get; set; }
    }
}

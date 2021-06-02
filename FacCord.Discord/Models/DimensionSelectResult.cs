using IsekaiTechnologies.FacCord.Core.Models;
using IsekaiTechnologies.FacCord.Core.Models.Mines;
using System;
using System.Collections.Generic;
using System.Text;

namespace IsekaiTechnologies.FacCord.Discord.Models
{
    public class DimensionSelectResult
    {
        public Profile Profile { get; set; }
        public Dimension Dimension { get; set; }
        public RegionSelectionRequest Request { get; set; }
    }
}

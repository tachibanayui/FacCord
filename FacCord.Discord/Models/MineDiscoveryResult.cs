using IsekaiTechnologies.FacCord.Core.Mine;
using IsekaiTechnologies.FacCord.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace IsekaiTechnologies.FacCord.Discord.Models
{
    public class MineDiscoveryResult
    {
        public bool IsSuccess { get; set; }
        public Profile Profile { get; set; }
        public AbstractMine Mine { get; set; }
    }
}

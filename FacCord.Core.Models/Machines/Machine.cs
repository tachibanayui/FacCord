using IsekaiTechnologies.FacCord.Core.Models.Storages;
using System;
using System.Collections.Generic;
using System.Text;

namespace IsekaiTechnologies.FacCord.Core.Models.Machines
{
    public class Machine : Item
    {
        public short Weight { get; set; }
        public short Width { get; set; }
        public short Height { get; set; }
    }
}

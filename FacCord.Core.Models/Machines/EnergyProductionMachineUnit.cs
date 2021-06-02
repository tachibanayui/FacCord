using IsekaiTechnologies.FacCord.Core.Models.Storages;
using System;
using System.Collections.Generic;
using System.Text;

namespace IsekaiTechnologies.FacCord.Core.Models.Machines
{
    public class EnergyProductionMachineUnit : ProcessingMachineUnit
    {
        // The energy produced by this machine measure in seconds
        public double ProductionRate { get; set; }
    }
}

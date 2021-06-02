using IsekaiTechnologies.FacCord.Core.Models.Machines;
using IsekaiTechnologies.FacCord.Core.Models.Storages;
using System;
using System.Collections.Generic;

namespace IsekaiTechnologies.FacCord.Core.Models
{
    public class Workshop
    {
        public long Id { get; set; }

        public int SizeX { get; set; }
        public int SizeY { get; set; }

        public short WeightCapacity { get; set; }
        public short SupportedWeight { get; set; }

        public long StorageCapacity { get; set; }
        public long CurrentStorageItemCount { get; set; }

        public double EnergyCapacity { get; set; }
        public double CurrentEnergy { get; set; }

        public List<MachineUnit> Units { get; set; }
        public List<StorageItem> Storage { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}

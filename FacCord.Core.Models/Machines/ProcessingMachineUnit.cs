using IsekaiTechnologies.FacCord.Core.Models.Storages;
using System;
using System.Collections.Generic;
using System.Text;

namespace IsekaiTechnologies.FacCord.Core.Models.Machines
{
    public class ProcessingMachineUnit : MachineUnit
    {
        /// <summary>
        /// The storage buffer for this machine for both input and output
        /// </summary>
        public long BufferSize { get; set; }

        public List<StorageItem> InputBuffer { get; set; }
        public List<StorageItem> OutputBuffer { get; set; }
        public Recipe Procedure { get; set; }
        public double Progress { get; set; }
    }
}

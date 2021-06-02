using System;
using System.Collections.Generic;
using System.Text;

namespace IsekaiTechnologies.FacCord.Core.Models.Machines
{
    public class MachineUnit
    {
        /// <summary>
        /// The MachineUnit ID this ID is a identifier for this instance of machine in the workshop
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Represent a MachineID. Note MachineID is a identifier for Machine Type not instances of Machine 
        /// </summary>
        public Machine Machine { get; set; }

        public short PosX { get; set; }
        public short PosY { get; set; }
    }
}

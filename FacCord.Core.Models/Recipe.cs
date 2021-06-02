using IsekaiTechnologies.FacCord.Core.Models.Machines;
using IsekaiTechnologies.FacCord.Core.Models.Storages;
using System.Collections.Generic;

namespace IsekaiTechnologies.FacCord.Core.Models
{
    public class Recipe
    {
        public long Id { get; set; }
        public string RecipeName { get; set; }
        public List<StorageItem> Inputs { get; set; }
        public List<StorageItem> Outputs { get; set; }

        /// <summary>
        /// The MachineId used to process this recipe
        /// </summary>
        public Machine ProcessedBy { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IsekaiTechnologies.FacCord.Core.Models.Equipments
{
    public class RuptureChemical
    {
        public long Id { get; set; }
        public short Count { get; set; }
        public TimeSpan NextRefresh { get; set; }
        public short Capacity { get; set; }
        public TimeSpan RefreshInterval { get; set; }
        public DateTime LastUpdated { get; set; }

        public static RuptureChemical CreateNew ()
        {
            return new RuptureChemical()
            {
                Count = 3,
                Capacity = 3,
                NextRefresh = TimeSpan.Zero,
                LastUpdated = DateTime.Now,
                RefreshInterval = TimeSpan.FromHours(1),
            };
        }
    }
}

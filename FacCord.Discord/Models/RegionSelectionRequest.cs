using Discord;
using IsekaiTechnologies.FacCord.Discord.Pages;
using System;
using System.Collections.Generic;
using System.Text;

namespace IsekaiTechnologies.FacCord.Discord.Models
{
    public class RegionSelectionRequest
    {
        // TODO: Implement Locked Region Picker, UnCancelable
        public bool RequireUnlocked { get; set; }
    }
}

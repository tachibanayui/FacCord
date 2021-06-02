using System;
using System.Collections.Generic;
using System.Text;

namespace IsekaiTechnologies.FacCord.Core.Models
{
    public class Player
    {
        public long Id { get; set; }
        public long DiscordId { get; set; }

        public List<Profile> Profiles { get; set; }
        public PlayerSettings Settings { get; set; }
    }
}

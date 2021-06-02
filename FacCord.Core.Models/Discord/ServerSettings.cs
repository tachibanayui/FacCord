using System;
using System.Collections.Generic;
using System.Text;

namespace IsekaiTechnologies.FacCord.Core.Models.Discord
{
    public class ServerSettings
    {
        public long Id { get; set; }
        public long DiscordServerId { get; set; }
        public string ServerPrefix { get; set; }
    }
}

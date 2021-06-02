using Discord;
using IsekaiTechnologies.FacCord.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace IsekaiTechnologies.FacCord.Discord
{
    public class DiscordPlayerContext
    {
        public PlayerContext PlayerContext { get; set; }
        public IUser User { get; set; }
        public IDiscordClient Client { get; set; }
        public IMessageChannel ActiveChannel { get; set; }

        public DiscordPlayerContext(PlayerContext context, IDiscordClient client, IUser user)
        {
            PlayerContext = context;
            Client = client;
            User = user;
        }
    }
}

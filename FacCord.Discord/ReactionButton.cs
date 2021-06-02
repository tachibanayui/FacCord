using Discord;
using Discord.WebSocket;
using IsekaiTechnologies.FacCord.Discord.Utils;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsekaiTechnologies.FacCord.Discord
{
    public class ReactionButton : IDisposable
    {
        public string ButtonEmote { get; set; }
        public DiscordSocketClient Client { get; set; }
        public IUserMessage Message { get; set; }

        public event Func<Cacheable<IUserMessage, ulong>, ISocketMessageChannel, SocketReaction, Task> Click;
        public ReactionButton(string emote, DiscordSocketClient client)
        {
            ButtonEmote = emote;
            Client = client;
        }

        public async Task AttachToMessage(IUserMessage msg)
        {
            Message = msg;
            await Message.AddReactionAsync(EmoteUtils.GetEmote(ButtonEmote));
            Client.ReactionAdded += Client_ReactionAdded;
        }

        private async Task Client_ReactionAdded(Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2, SocketReaction arg3)
        {
            if (arg1.Id == Message.Id && arg3.Emote.Name  == ButtonEmote && arg3.User.Value.Id != Client.CurrentUser.Id)
                await Click?.Invoke(arg1, arg2, arg3);
        }

        public void Dispose()
        {
            Client.ReactionAdded -= Client_ReactionAdded;
        }
    }
}

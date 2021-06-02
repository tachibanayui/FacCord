using Discord;
using Discord.Rest;
using Discord.WebSocket;
using IsekaiTechnologies.FacCord.Core;
using IsekaiTechnologies.FacCord.Core.DAL;
using IsekaiTechnologies.FacCord.Core.Models;
using IsekaiTechnologies.FacCord.Discord.Utils;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IsekaiTechnologies.FacCord.Discord.Pages
{
    public class FaccordPage : Page
    {
        protected IUnitOfWork DataAccess;
        protected IUIEmojiLookupTable UIEmoji;
        protected PlayerContext PlayerContext;
        protected List<ReactionButton> ReactionButtons = new List<ReactionButton>();
        protected bool IsActive;

        protected string FallbackEmoji;

        public override async Task OnNavigatedTo(Page sourcePage, PageManager manager, object args)
        {
            await base.OnNavigatedTo(sourcePage, manager, args);
            IsActive = true;

            DataAccess = Manager.Services.GetRequiredService<IUnitOfWork>();
            UIEmoji = Manager.Services.GetRequiredService<IUIEmojiLookupTable>();

            FallbackEmoji = await UIEmoji.GetUIEmoji("Sprites.Fallback", "❔");
            PlayerContext = await manager.GameManager.GetPlayerContext(await DataAccess.Players.GetPlayerByDiscordId((long)Manager.User.Id));
        }

        public override Task OnNavigatedFrom(Page destinationPage, PageManager manager, object args)
        {
            IsActive = false;
            foreach (var item in ReactionButtons)
            {
                item.Dispose();
            }

            return base.OnNavigatedFrom(destinationPage, manager, args);
        }


        protected async Task AddReactionButton(string emote, IUserMessage msg, Func<Cacheable<IUserMessage, ulong>, ISocketMessageChannel, SocketReaction, Task> onClick)
        {
            if (!IsActive) return;

            ReactionButton btn = new ReactionButton(emote, Manager.Client);
            btn.Click += async (a, b, c) =>
            {
                if (c.User.IsSpecified && c.User.Value.Id == Manager.User.Id)
                {
                    await onClick(a, b, c);
                }
            };
            await btn.AttachToMessage(msg);
            ReactionButtons.Add(btn);
        }
    }
}

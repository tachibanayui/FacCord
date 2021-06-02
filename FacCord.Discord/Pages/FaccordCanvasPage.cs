using IsekaiTechnologies.FacCord.Core;
using IsekaiTechnologies.FacCord.Core.DAL;
using IsekaiTechnologies.FacCord.Discord.Utils;
using IsekaiTechnologies.ImagiTextedDiscordGUI.DrawingContexts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IsekaiTechnologies.FacCord.Discord.Pages
{
    public class FaccordCanvasPage : CanvasPage
    {
        public FaccordCanvasPage(IDrawingContext ctx) : base(ctx)
        {
        }

        protected IUnitOfWork DataAccess;
        protected IUIEmojiLookupTable UIEmoji;
        protected PlayerContext PlayerContext;
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

        public override async Task OnNavigatedFrom(Page destinationPage, PageManager manager, object args)
        {
            await base.OnNavigatedFrom(destinationPage, manager, args);
            IsActive = false;
        }
    }
}

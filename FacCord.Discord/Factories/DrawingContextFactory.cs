using Discord;
using IsekaiTechnologies.FacCord.Core.Models;
using IsekaiTechnologies.FacCord.Discord.Utils;
using IsekaiTechnologies.ImagiTextedDiscordGUI.DrawingContexts;
using IsekaiTechnologies.ImagiTextedDiscordGUI.Renderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Region2DInt = IsekaiTechnologies.ImagiTextedDiscordGUI.Utils.Region2DInt;

namespace IsekaiTechnologies.FacCord.Discord.Factories
{
    public class DrawingContextFactory : IDrawingContextFactory
    {
        public IUIEmojiLookupTable UIEmoji { get; set; }

        public DrawingContextFactory(IUIEmojiLookupTable uiEmoji)
        {
            UIEmoji = uiEmoji;
        }

        public async Task<IDrawingContext> GetDrawingContext(ITextChannel channel, Region2DInt viewport)
        {
            var renderer = new Renderer(channel, viewport, await UIEmoji.GetEmptyEmojis().ToListAsync());
            return new DrawingContext(renderer);
        }
    }
}

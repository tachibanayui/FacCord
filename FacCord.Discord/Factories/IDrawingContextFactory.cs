using Discord;
using IsekaiTechnologies.ImagiTextedDiscordGUI.DrawingContexts;
using IsekaiTechnologies.ImagiTextedDiscordGUI.Utils;
using System.Threading.Tasks;

namespace IsekaiTechnologies.FacCord.Discord.Factories
{
    public interface IDrawingContextFactory
    {
        Task<IDrawingContext> GetDrawingContext(ITextChannel channel, Region2DInt viewport);
    }
}
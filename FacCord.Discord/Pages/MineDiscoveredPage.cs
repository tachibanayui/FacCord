using Discord;
using Discord.Rest;
using Discord.WebSocket;
using IsekaiTechnologies.FacCord.Core.Mine;
using IsekaiTechnologies.FacCord.Core.Models;
using IsekaiTechnologies.FacCord.Core.Models.Mines;
using IsekaiTechnologies.FacCord.Discord.Factories;
using IsekaiTechnologies.FacCord.Discord.Models;
using IsekaiTechnologies.ImagiTextedDiscordGUI.DrawingContexts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsekaiTechnologies.FacCord.Discord.Pages
{
    public class MineDiscoveredPage : FaccordPage
    {
        private MineDiscoveryResult Result;
        private string _EmojiConfirmTravel;
        private string _EmojiRescan;

        private IUserMessage _ViewMessage;

        public override async Task OnNavigatedTo(Page sourcePage, PageManager manager, object args)
        {
            if (args is MineDiscoveryResult mdr)
            {
                await base.OnNavigatedTo(sourcePage, manager, args);
                Result = mdr;

                _EmojiConfirmTravel = await UIEmoji.GetUIEmoji("MineDiscovered.Buttons.ConfirmTravel", FallbackEmoji);
                _EmojiRescan = await UIEmoji.GetUIEmoji("MineDiscovered.Buttons.Rescan", FallbackEmoji);
            }
            else
            {
                throw new InvalidOperationException("Invalid navigate argument!");
            }
        }

        public override async Task LoadView(IMessageChannel channel)
        {
            await base.LoadView(channel);

            StringBuilder bd = new StringBuilder();
            bd.AppendLine("**Do you want to traval to the mine?**");
            bd.AppendLine($"*React with {_EmojiConfirmTravel} to enter, {_EmojiRescan} to find a new mine*");
            bd.AppendLine($"To look for specific material send:");
            bd.AppendLine($"```<prefix> matstats <material name>```");
            bd.AppendLine("**Mine information:**");
            string regionEmoji = await DataAccess.EmojiLookups.GetRegionIcon(Result.Profile.SelectedRegion, FallbackEmoji);
            bd.AppendLine($"Region: **{regionEmoji} {Result.Profile.SelectedRegion.Name}**");
            bd.AppendLine($"Mine size: **{Result.Mine.Width}x{Result.Mine.Height}**");
            bd.AppendLine();
            bd.AppendLine("You can find these materials in this mine:");
            int matEntryCount = Result.Mine.MaterialCounts.Count;
            foreach (var item in Result.Mine.MaterialCounts.Take(25))
            {
                string blockEmoji = await DataAccess.EmojiLookups.GetBlockIcon(item.Key, FallbackEmoji);
                bd.AppendLine($"{blockEmoji} {item.Key.Item.Name}: {item.Value}");
            }
            if (matEntryCount > 25)
            {
                bd.AppendLine("...");
            }

            EmbedBuilder embedBuilder = new EmbedBuilder()
                .WithTitle("You just discovered a mine!")
                .WithDescription(bd.ToString());

            _ViewMessage = await channel.SendMessageAsync(embed: embedBuilder.Build());
            await AddReactionButton(_EmojiConfirmTravel, _ViewMessage, btnGo_OnClick);
            await AddReactionButton(_EmojiRescan, _ViewMessage, btnGo_OnClick);
        }

        private async Task btnGo_OnClick(Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2, SocketReaction arg3)
        {
            string waitMsg = "Please wait while we setting up your game. ";

            //if (Manager.DrawChannel == null)
            //{
            //    await Manager.CreateDrawChannel(((ITextChannel)arg2).Guild);
            //    waitMsg += $"A new channel has created for you to experience the game!";
            //}
            // Comment this for anti channel creation spam

            //Comment this line for production code
            Manager.DrawChannel = arg2 as SocketTextChannel;
            //

            await arg2.SendMessageAsync(waitMsg);

            var drawingContext = await Manager.Services.GetRequiredService<IDrawingContextFactory>().GetDrawingContext(Manager.DrawChannel, new ImagiTextedDiscordGUI.Utils.Region2DInt(998, 33));

            await PlayerContext.MineController.GoToMine(Result.Mine);
            await Manager.Navigate(new MinePage(drawingContext), arg2, null);
            await arg2.SendMessageAsync($"Thank for your patience, please head over to {Manager.DrawChannel.Mention} to continue!");
        }
    }
}

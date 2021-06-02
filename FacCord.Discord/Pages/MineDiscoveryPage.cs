using Discord;
using Discord.Rest;
using Discord.WebSocket;
using IsekaiTechnologies.FacCord.Core;
using IsekaiTechnologies.FacCord.Core.Controllers;
using IsekaiTechnologies.FacCord.Core.DAL;
using IsekaiTechnologies.FacCord.Core.EquipmentLogic;
using IsekaiTechnologies.FacCord.Core.MineGeneration;
using IsekaiTechnologies.FacCord.Core.Models;
using IsekaiTechnologies.FacCord.Core.Models.Equipments;
using IsekaiTechnologies.FacCord.Core.Models.Mines;
using IsekaiTechnologies.FacCord.Core.Models.Storages;
using IsekaiTechnologies.FacCord.Core.Providers;
using IsekaiTechnologies.FacCord.Discord.Factories;
using IsekaiTechnologies.FacCord.Discord.Models;
using IsekaiTechnologies.FacCord.Discord.Utils;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace IsekaiTechnologies.FacCord.Discord.Pages
{
    class MineDiscoveryPage : FaccordPage
    {
        private Profile _CurrentProfile;
        private IUserMessage _ViewMessage;

        private string _GenericProspectorSprite;
        private string _GenericRegionSprite;
        private string _ButtonFindMine;


        public override async Task OnNavigatedTo(Page sourcePage, PageManager manager, object args)
        {
            await base.OnNavigatedTo(sourcePage, manager, args);

            _GenericProspectorSprite = await UIEmoji.GetUIEmoji("Sprites.Generic.Prospector", FallbackEmoji);
            _GenericRegionSprite = await UIEmoji.GetUIEmoji("Sprites.Generic.Region", FallbackEmoji);
            _ButtonFindMine = await UIEmoji.GetUIEmoji("MineDiscovery.Buttons.FindMine", FallbackEmoji);


            if (args is PlayerContext ctx)
            {
                PlayerContext = ctx;
            }
            else
            {
                var player = await DataAccess.Players.GetPlayerByDiscordId((long)Manager.User.Id);
                PlayerContext = await Manager.GameManager.GetPlayerContext(player);
            }

            _CurrentProfile = await DataAccess.Profiles.GetActiveProfileForMineDiscoveryAsync(PlayerContext.CurrentPlayer);
        }

        public override async Task LoadView(IMessageChannel channel)
        {
            await base.LoadView(channel);

            var prospectorEmoji = await DataAccess.EmojiLookups.GetItemIcon(_CurrentProfile.CurrentProspector.Item, FallbackEmoji);
            var regionEmoji = await DataAccess.EmojiLookups.GetRegionIcon(_CurrentProfile.SelectedRegion, FallbackEmoji);

            EmbedBuilder embed = new EmbedBuilder()
                .WithTitle("Find a mine")
                .WithDescription($"React with {_GenericProspectorSprite} to change prospector and {_GenericRegionSprite} to change region\n\nThe prospector will scan base on these conditions: ")
                .AddField($"Prospector: ", $"{prospectorEmoji} {_CurrentProfile.CurrentProspector.Item.Name}", true)
                .AddField($"Region", $"{regionEmoji} {_CurrentProfile.SelectedRegion.Name}", true);

            _ViewMessage = await channel.SendMessageAsync(embed: embed.Build());
            await AddReactionButton(_ButtonFindMine, _ViewMessage, btnFindMine_Click);
            await AddReactionButton(_GenericProspectorSprite, _ViewMessage, btnChangeProspector_Click);
            await AddReactionButton(_GenericRegionSprite, _ViewMessage, btnChangeRegion_Click);
        }

        private async Task btnChangeRegion_Click(Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2, SocketReaction arg3)
        {
            RegionSelectionRequest r = new RegionSelectionRequest();
            var regionPicker = new DimensionPickerPage();
            regionPicker.RegionPickSuccess += async (s, e) =>
            {
                _CurrentProfile.SelectedRegion = e.Value;
                DataAccess.Complete();
                await Manager.Navigate(new MineDiscoveryPage(), arg2, null);

            };
            regionPicker.RegionPickFailed += async (s, e) => await Manager.Navigate(new MineDiscoveryPage(), arg2, null);
            await Manager.Navigate(regionPicker, arg2, r);
        }

        private async Task btnFindMine_Click(Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2, SocketReaction arg3)
        {
            if (DateTime.Now - _CurrentProfile.LastFindMine > (_CurrentProfile.CurrentProspector.Item as Prospector).ScanCooldown)
            {
                try
                {
                    var mineResult = await PlayerContext.MineController.FindMineAsync();
                    _CurrentProfile.LastFindMine = DateTime.Now;
                    DataAccess.Complete();

                    await Manager.Navigate(new MineDiscoveredPage(), arg2, new MineDiscoveryResult() { IsSuccess = true, Mine = mineResult, Profile = _CurrentProfile });
                }
                catch (Exception e)
                {
                    // Implement ErrorDiscoveryPage or maybe hook this up to MineDiscovered page
                }
            }
            else
            {
                // Implement Wait for cooldown page
            }
        }

        private async Task btnChangeProspector_Click(Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2, SocketReaction arg3)
        {
            ItemPickerRequest rq = ItemPickerRequest.CreateSingleItemRequest();
            rq.PickerTitle = "Choose a prospector";
            rq.TypeFilters = new List<Type>() { /*typeof(Prospector)*/ };
            rq.Cancelable = true;
            rq.FiltersChangeableByUser = false;
            rq.ItemPool = _CurrentProfile.Workshop.Storage;
            var picker = new ItemPickerPage();
            picker.Success += async (s, e) =>
            {
                using(var stomgmt = await Manager.Services.GetRequiredService<StorageManagementProvider>().GetStorageManagement(DataAccess, _CurrentProfile))
                {
                    await stomgmt.ModifyItemAsync(e.Value[0].Id, x => x.Count--);
                    _CurrentProfile.Workshop.CurrentStorageItemCount--;
                    _CurrentProfile.CurrentProspector = e.Value[0].Clone();
                    _CurrentProfile.CurrentProspector.Id = 0;
                }
            };
            await Manager.Navigate(picker, arg2, rq);
        }

     
    }
}

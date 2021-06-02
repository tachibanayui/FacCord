using Discord;
using Discord.Rest;
using Discord.WebSocket;
using IsekaiTechnologies.FacCord.Core;
using IsekaiTechnologies.FacCord.Core.DAL;
using IsekaiTechnologies.FacCord.Core.Models;
using IsekaiTechnologies.FacCord.Core.Utils;
using IsekaiTechnologies.FacCord.Discord.Utils;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace IsekaiTechnologies.FacCord.Discord.Pages
{
    public class TitlePage : FaccordPage
    {
        private Profile _CurrentProfile;

        private IUserMessage _ViewMessage;

        private string _MineEmoji;
        private string _InventoryEmoji;
        private string _WorkshopEmoji;
        private string _QuestEmoji;
        private string _ShopEmoji;

        private string _SpriteRuptureChemical;


        public override async Task OnNavigatedTo(Page sourcePage, PageManager manager, object args)
        {
            await base.OnNavigatedTo(sourcePage, manager, args);

            _CurrentProfile = await DataAccess.Profiles.GetActiveProfileForTitleAsync(PlayerContext.CurrentPlayer);

            _MineEmoji = await UIEmoji.GetUIEmoji("Title.Buttons.FindMine", FallbackEmoji);
            _InventoryEmoji = await UIEmoji.GetUIEmoji("Title.Buttons.Inventory", FallbackEmoji);
            _WorkshopEmoji = await UIEmoji.GetUIEmoji("Title.Buttons.Workshop", FallbackEmoji);
            _QuestEmoji = await UIEmoji.GetUIEmoji("Title.Buttons.Quest", FallbackEmoji);
            _ShopEmoji = await UIEmoji.GetUIEmoji("Title.Buttons.Shop", FallbackEmoji);
            _SpriteRuptureChemical = await UIEmoji.GetUIEmoji("Sprites.Generic.RuptureChemical", FallbackEmoji);
        }

        public override async Task LoadView(IMessageChannel channel)
        {
            var description = new StringBuilder();
            description.AppendLine($"Current balance: ${_CurrentProfile.Fund}");
            description.AppendLine($"Level: {_CurrentProfile.Level} ({_CurrentProfile.Exp}/{await Manager.GameManager.GetExpToLevelUp(_CurrentProfile.Level)}XP)");
            description.AppendLine($"");
            description.AppendLine($"Actions:");

            var fmDescription = new StringBuilder();
            string regionIcon = await DataAccess.EmojiLookups.GetRegionIcon(_CurrentProfile.SelectedRegion, FallbackEmoji);
            fmDescription.AppendLine($"React with {_MineEmoji} to find a mine");
            fmDescription.AppendLine($"Region: **{regionIcon} {_CurrentProfile.SelectedRegion.Name} ({_CurrentProfile.SelectedRegion.Dimension.Name})**");
            string prospectorEmoji = await DataAccess.EmojiLookups.GetItemIcon(_CurrentProfile.CurrentProspector.Item, FallbackEmoji);
            fmDescription.AppendLine($"Current prospector: {prospectorEmoji} **{_CurrentProfile.CurrentProspector.Item.Name}**");
            string drillEmoji = await DataAccess.EmojiLookups.GetItemIcon(_CurrentProfile.CurrentDrill.Item, FallbackEmoji);
            fmDescription.AppendLine($"Current drill: {drillEmoji} **{_CurrentProfile.CurrentDrill.Item.Name}**");
            TimeSpan tsRefreshDelta = _CurrentProfile.RuptureChemical.LastUpdated + _CurrentProfile.RuptureChemical.NextRefresh - DateTime.Now;
            TimeSpan tsRefresh = tsRefreshDelta > TimeSpan.Zero ? tsRefreshDelta : TimeSpan.Zero;
            fmDescription.AppendLine($"Rupture chemical status: {_SpriteRuptureChemical} **{_CurrentProfile.RuptureChemical.Count}/{_CurrentProfile.RuptureChemical.Capacity} ({tsRefresh.FormatHourAndMinute()})**");

            string profileDisplayName = string.IsNullOrEmpty(_CurrentProfile.ProfileName) ? "Unnamed Profile" : _CurrentProfile.ProfileName;

            var embedBuilder = new EmbedBuilder()
                .WithAuthor(Manager.User)
                .WithTitle($"Welcome {Manager.User.Username} on profile {profileDisplayName}!")
                .WithDescription(description.ToString())
                .WithColor(Color.Orange)
                .AddField($"{_MineEmoji} Find a mine", fmDescription.ToString())
                .AddField($"{_InventoryEmoji} Inventory", $"React with the {_InventoryEmoji} emote to change your equipment, organize resource and more\nStorage: {_CurrentProfile.Workshop.CurrentStorageItemCount}/{_CurrentProfile.Workshop.StorageCapacity}")
                .AddField($"{_WorkshopEmoji} Workshop", $"React with {_WorkshopEmoji} to go to your workshop")
                .AddField($"{_QuestEmoji} Quest", $"React with {_QuestEmoji} to browse quests")
                .AddField($"{_ShopEmoji} Shop", $"React with {_ShopEmoji} to buy items");
            
            _ViewMessage = await channel.SendMessageAsync(embed: embedBuilder.Build());
            await AddReactionButton(_MineEmoji, _ViewMessage, btnMine_Click);
            await AddReactionButton(_InventoryEmoji, _ViewMessage, null);
            await AddReactionButton(_WorkshopEmoji, _ViewMessage, null);
            await AddReactionButton(_QuestEmoji, _ViewMessage, null);
            await AddReactionButton(_ShopEmoji, _ViewMessage, null);
        }

        private async Task btnMine_Click(Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2, SocketReaction arg3)
        {
            await Manager.Navigate(new MineDiscoveryPage(), arg2, null);
        }

    }
}

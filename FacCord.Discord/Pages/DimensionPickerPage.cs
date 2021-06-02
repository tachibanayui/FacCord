using Discord;
using Discord.Rest;
using Discord.WebSocket;
using IsekaiTechnologies.FacCord.Core.Models;
using IsekaiTechnologies.FacCord.Core.Models.Mines;
using IsekaiTechnologies.FacCord.Discord.Models;
using IsekaiTechnologies.ImagiTextedDiscordGUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace IsekaiTechnologies.FacCord.Discord.Pages
{
    public class DimensionPickerPage : FaccordPage
    {
        private IUserMessage _InstructionMessage;
        private Dictionary<IUserMessage, Dimension> _ViewMessages = new Dictionary<IUserMessage, Dimension>();
        private List<Dimension> _UnlockedDimensions = new List<Dimension>();
        private Profile _CurrentProfile;
        public RegionSelectionRequest Request { get; set; }

        private string _BackEmoji;
        private string _SelectEmoji;
        private string _InfoEmoji;

        public override async Task OnNavigatedTo(Page sourcePage, PageManager manager, object args)
        {
            await base.OnNavigatedTo(sourcePage, manager, args);
            if (args is RegionSelectionRequest rsr)
            {
                Request = rsr;
                _CurrentProfile = await DataAccess.Profiles.GetActiveProfileForRegionSelectionAsync(PlayerContext.CurrentPlayer);
                _BackEmoji = await UIEmoji.GetUIEmoji("Sprites.Generic.Back", FallbackEmoji);
                _SelectEmoji = await UIEmoji.GetUIEmoji("Sprites.Generic.Tick", FallbackEmoji);
                _InfoEmoji = await UIEmoji.GetUIEmoji("Sprites.Generic.Info", FallbackEmoji);

                GetUnlockedDimensions();
            }
        }

        public override async Task LoadView(IMessageChannel channel)
        {
            await base.LoadView(channel);

            string lastRegionEmoji = await DataAccess.EmojiLookups.GetRegionIcon(_CurrentProfile.SelectedRegion, FallbackEmoji);

            StringBuilder bd = new StringBuilder();
            bd.AppendLine("**Choose a dimension:**");
            bd.AppendLine($"*Please react with the emote under a dimension entry to select or react with {_BackEmoji} to cancel region selection process*");
            bd.AppendLine($"Last selected region: {lastRegionEmoji} **{_CurrentProfile.SelectedRegion.Name}**");

            _InstructionMessage = await channel.SendMessageAsync(bd.ToString());
            await AddReactionButton(_BackEmoji, _InstructionMessage, btnBack_Click);

            foreach (var item in _UnlockedDimensions)
            {
                var msg = await GenerateMessage(channel, item);
                await AddReactionButton(_SelectEmoji, msg, btnSelect_Click);
                await AddReactionButton(_InfoEmoji, msg, btnInfo_Click);
            }
            await foreach (var item in GetLockedDimensions())
            {
                var msg = await GenerateMessage(channel, item);
                await AddReactionButton(_InfoEmoji, msg, btnInfo_Click);
            }
        }

        private Task btnBack_Click(Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2, SocketReaction arg3)
        {
            throw new NotImplementedException();
        }

        private Task btnInfo_Click(Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2, SocketReaction arg3)
        {
            throw new NotImplementedException();
        }

        private async Task btnSelect_Click(Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2, SocketReaction arg3)
        {
            if (ValidateDimensionSelection(arg1.Id, out var dimension))
            {
                var rpp = new RegionPickerPage();
                rpp.Success += (s, e) => RegionPickSuccess?.Invoke(rpp, e);
                rpp.Cancelled += (s, e) => RegionPickFailed?.Invoke(rpp, e);
                await Manager.Navigate(rpp, arg2, new DimensionSelectResult() { Profile = _CurrentProfile, Dimension = dimension, Request = Request });
            }
        }

        private bool ValidateDimensionSelection(ulong messageId, out Dimension dimension)
        {
            var queryResult = _ViewMessages.FirstOrDefault(x => x.Key.Id == messageId);
            if(queryResult.Key != null && _UnlockedDimensions.Contains(queryResult.Value))
            {
                dimension = queryResult.Value;
                return true;
            }
            else
            {
                dimension = null;
                return false;
            }
        }

        private async Task<IUserMessage> GenerateMessage(IMessageChannel channel, Dimension item)
        {
            string dimensionEmoji = await DataAccess.EmojiLookups.GetDimensionIcon(item, FallbackEmoji);
            StringBuilder bd = new StringBuilder();
            bd.AppendLine($"{dimensionEmoji} __{item.Name}__");
            bd.AppendLine(item.Description);
            bd.Append($"Possible resources:");
            foreach (var res in item.PossibleResources.Take(5))
            {
                string resourceEmoji = await DataAccess.EmojiLookups.GetItemIcon(res, FallbackEmoji);
                bd.Append($" {resourceEmoji},");
            }
            bd.Append("...");

            var msg = await channel.SendMessageAsync(bd.ToString());
            _ViewMessages.Add(msg, item);
            return msg;
        }

        private void GetUnlockedDimensions()
        {
            foreach (var item in _CurrentProfile.UnlockedRegions)
            {
                if (!_UnlockedDimensions.Contains(item.Dimension))
                {
                    _UnlockedDimensions.Add(item.Dimension);
                }
            }
        }

        private async IAsyncEnumerable<Dimension> GetLockedDimensions()
        {
            foreach (var item in await DataAccess.Dimensions.GetAllWidthPossibleResourcesAsync().Where(x => !_UnlockedDimensions.Any(p => p.Id == x.Id)).ToListAsync())
            {
                yield return item;
            }
        }

        public event EventHandler<ValueEventArgs<Region>> RegionPickSuccess;
        public event EventHandler<EventArgs> RegionPickFailed;
    }
}

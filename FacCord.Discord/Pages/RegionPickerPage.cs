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
using System.Text;
using System.Threading.Tasks;

namespace IsekaiTechnologies.FacCord.Discord.Pages
{
    public class RegionPickerPage : FaccordPage
    {
        private Profile _CurrentProfile;
        private Dimension _SelectedDimension;

        private IUserMessage _InstructionMessage;
        private Dictionary<IUserMessage, Region> _ViewMessages = new Dictionary<IUserMessage, Region>();
        private List<Region> _UnlockedRegions = new List<Region>();

        public RegionSelectionRequest Request { get; set; }

        private string _BackEmoji;
        private string _DimensionEmoji;
        private string _SelectEmoji;
        private string _InfoEmoji;

        public override async Task OnNavigatedTo(Page sourcePage, PageManager manager, object args)
        {
            await base.OnNavigatedTo(sourcePage, manager, args);
            if (args is DimensionSelectResult dsr)
            {
                Request = dsr.Request;
                _SelectedDimension = dsr.Dimension;
                _CurrentProfile = dsr.Profile;


                _BackEmoji = await UIEmoji.GetUIEmoji("Sprites.Generic.Back", FallbackEmoji);
                _DimensionEmoji = await UIEmoji.GetUIEmoji("Sprites.Generic.Dimension", FallbackEmoji);
                _SelectEmoji = await UIEmoji.GetUIEmoji("Sprites.Generic.Tick", FallbackEmoji);
                _InfoEmoji = await UIEmoji.GetUIEmoji("Sprites.Generic.Info", FallbackEmoji);

                GetUnlockedRegion();
            }
            else
            {
                throw new InvalidOperationException("Invalid argument");
            }
        }

        public override async Task LoadView(IMessageChannel channel)
        {
            await base.LoadView(channel);

            string selectedDimensionEmoji = await DataAccess.EmojiLookups.GetDimensionIcon(_SelectedDimension, FallbackEmoji);
            string lastRegionEmoji = await DataAccess.EmojiLookups.GetRegionIcon(_CurrentProfile.SelectedRegion, FallbackEmoji);

            StringBuilder bd = new StringBuilder();
            bd.AppendLine("**Pick a region**");
            bd.AppendLine($"*Please react with the emote under a region entry to select or react with {_BackEmoji} to cancel region selection process. You can also react with {_DimensionEmoji} to repick dimension*");
            bd.AppendLine($"Selected dimension: {selectedDimensionEmoji} **{_SelectedDimension.Name}**");
            bd.AppendLine($"Last selected region: {lastRegionEmoji} **{_CurrentProfile.SelectedRegion.Name}**");

            _InstructionMessage = await channel.SendMessageAsync(bd.ToString());
            await AddReactionButton(_BackEmoji, _InstructionMessage, btnBack_Click);
            await AddReactionButton(_DimensionEmoji, _InstructionMessage, btnRepick_Click);

            foreach (var item in _UnlockedRegions)
            {
                var msg = await GenerateMessage(channel, item);
                await AddReactionButton(_SelectEmoji, msg, btnSelect_Click);
                await AddReactionButton(_InfoEmoji, msg, btnInfo_Click);
            }

            foreach (var item in _SelectedDimension.Regions.Where(x => !_UnlockedRegions.Any(p => p.Id == x.Id)))
            {
                var msg = await GenerateMessage(channel, item);
                await AddReactionButton(_InfoEmoji, msg, btnInfo_Click);
            }
        }

        private Task btnInfo_Click(Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2, SocketReaction arg3)
        {
            throw new NotImplementedException();
        }

        private Task btnSelect_Click(Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2, SocketReaction arg3)
        {
            if (ValidateRegion(arg1.Id, out var region))
            {
                OnSuccess(region);
            }

            return Task.CompletedTask;
        }

        private async Task btnRepick_Click(Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2, SocketReaction arg3)
        {
            await Manager.Navigate(new DimensionPickerPage(), arg2, Request);
        }

        private Task btnBack_Click(Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2, SocketReaction arg3)
        {
            OnCancel();
            return Task.CompletedTask;
        }

        private bool ValidateRegion(ulong msgId, out Region region)
        {
            var queryResult = _ViewMessages.FirstOrDefault(x => x.Key.Id == msgId);
            if (queryResult.Value != null && _UnlockedRegions.Contains(queryResult.Value))
            {
                region = queryResult.Value;
                return true;
            }
            else
            {
                region = null;
                return false;
            }
        }

        private async Task<IUserMessage> GenerateMessage(IMessageChannel channel, Region item)
        {
            string regionEmoji = await DataAccess.EmojiLookups.GetRegionIcon(item, FallbackEmoji);
            StringBuilder bd = new StringBuilder();
            bd.AppendLine($"{regionEmoji} __{item.Name}__");
            bd.AppendLine(item.Description);
            bd.Append($"Possible resources:");
            foreach (var res in item.MaterialDistribution.Take(5))
            {
                string resourceEmoji = await DataAccess.EmojiLookups.GetItemIcon(res.Block.Item, FallbackEmoji);
                bd.Append($" {resourceEmoji},");
            }
            bd.Append("...");

            var msg = await channel.SendMessageAsync(bd.ToString());
            _ViewMessages.Add(msg, item);
            return msg;
        }

        private void GetUnlockedRegion()
        {
            foreach (var item in _CurrentProfile.UnlockedRegions)
            {
                if (item.Dimension.Id == _SelectedDimension.Id)
                {
                    _UnlockedRegions.Add(item);
                }
            }
        }


        public event EventHandler<ValueEventArgs<Region>> Success;
        public event EventHandler<EventArgs> Cancelled;

        protected virtual void OnSuccess(Region r) => Success?.Invoke(this, new ValueEventArgs<Region>(r));
        protected virtual void OnCancel() => Cancelled?.Invoke(this, EventArgs.Empty);
    }
}
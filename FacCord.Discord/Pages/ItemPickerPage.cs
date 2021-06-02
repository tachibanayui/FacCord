using Discord;
using Discord.WebSocket;
using IsekaiTechnologies.FacCord.Core.Models.Storages;
using IsekaiTechnologies.FacCord.Discord.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsekaiTechnologies.FacCord.Discord.Pages
{
    public class ItemPickerPage : FaccordPage
    {
        public const int ItemPerPage = 10;

        public ItemPickerRequest Request { get; protected set; }
        private List<StorageItem> SelectedItems = new List<StorageItem>();
        private List<StorageItem> CurrentItemPoolPage = new List<StorageItem>();
        private List<StorageItem> CurrentSelectedItemPage = new List<StorageItem>();
        private IUserMessage _ViewMessage;
        private IUserMessage _ResponseMessage;

        private IStorageQuerier _StorageQuerier;
        private string _SubmitEmoji;
        private string _PageUpEmoji;
        private string _PageDownEmoji;
        private string _PageUpSelectedEmoji;
        private string _PageDownSelectedEmoji;
        private string _BackEmoji;
        private string _SearchEmoji;
        private string _FilterEmoji;

        private string _InstructionsContent;
        private string _PickerFooter;

        private int _ItemPoolPage;
        private int _SelectedItemPage;
        private string _SearchKeyword;
        private int _LastSearchResultCount;
        private string _FilterListString;

        public override async Task OnNavigatedTo(Page destinationPage, PageManager manager, object args)
        {
            await base.OnNavigatedTo(destinationPage, manager, args);
            if (args is ItemPickerRequest ipr)
            {
                _StorageQuerier = Manager.Services.GetRequiredService<IStorageQuerier>();
                foreach (var item in ipr.TypeFilters)
                {
                    _StorageQuerier.AddItemFilter(item);
                }
                _StorageQuerier.SetItemPool(ipr.ItemPool);
                _StorageQuerier.SetKeywords(ipr.InitialKeyword);
                Request = ipr;
                _SearchKeyword = ipr.InitialKeyword;
                _FilterListString = GenerateFilterListString();

                _SubmitEmoji = await UIEmoji.GetUIEmoji("Sprites.Generic.Tick", FallbackEmoji);
                _PageUpEmoji = await UIEmoji.GetUIEmoji("ItemPicker.Buttons.PageUp", FallbackEmoji);
                _PageDownEmoji = await UIEmoji.GetUIEmoji("ItemPicker.Buttons.PageDown", FallbackEmoji);
                _PageUpSelectedEmoji = await UIEmoji.GetUIEmoji("ItemPicker.Buttons.PageUpSelected", FallbackEmoji);
                _PageDownSelectedEmoji = await UIEmoji.GetUIEmoji("ItemPicker.Buttons.PageDownSelected", FallbackEmoji);
                _BackEmoji = await UIEmoji.GetUIEmoji("Sprites.Generic.Back", FallbackEmoji);
                _SearchEmoji = await UIEmoji.GetUIEmoji("Sprites.Generic.Search", FallbackEmoji);
                _SearchEmoji = await UIEmoji.GetUIEmoji("Sprites.Generic.Filter", FallbackEmoji);

                Manager.Client.MessageReceived += Client_MessageReceived;
            }
        }

        public override async Task OnNavigatedFrom(Page destinationPage, PageManager manager, object args)
        {
            await base.OnNavigatedFrom(destinationPage, manager, args);
            Manager.Client.MessageReceived -= Client_MessageReceived;
        }

        // TODO: Create another way to beautify commands
        private async Task Client_MessageReceived(SocketMessage arg)
        {
            if (arg.Channel is SocketTextChannel stc && arg.Author.Id == Manager.User.Id)
            {
                var serverSettings = await Manager.GameManager.GetServerSettings((long) stc.Guild.Id);

                if (Manager.GameManager.ValidateGameCommand(arg.Content, serverSettings.ServerPrefix, out int argPos))
                {
                    await arg.DeleteAsync();

                    string trimPrefix = arg.Content.Substring(argPos).Trim();
                    if (trimPrefix.StartsWith("sel"))
                    {
                        await AddSelectionCommand(trimPrefix);
                    }
                    else if (trimPrefix.StartsWith("desel"))
                    {
                        await RemoveSelectionCommand(trimPrefix);
                    }
                    else if (trimPrefix.StartsWith("qs"))
                    {
                        await SearchCommand(trimPrefix);
                    }
                }
            }
        }

        private async Task SearchCommand(string trimPrefix)
        {
            try
            {
                string keywords = trimPrefix.Substring(trimPrefix.IndexOf(' ') + 1);
                _StorageQuerier.SetKeywords(keywords);
                await UpdateView();
            }
            catch
            {
                await UpdateResponseMessage("We looked far and wide but it seem like there is a problem at our end, please try again!");
            }
        }

        private async Task RemoveSelectionCommand(string trimPrefix)
        {
            var argSplit = trimPrefix.Split(' ');
            if (int.TryParse(argSplit[1], out var index))
            {
                if (CurrentSelectedItemPage.Count <= index)
                {
                    await UpdateResponseMessage(CurrentSelectedItemPage.Count == 0 ? "Nothing to select" : $"Invalid index, expected from 0 to {CurrentSelectedItemPage.Count - 1}");
                    return;
                }
                
                SelectedItems.Remove(CurrentSelectedItemPage[index]);
                await UpdateView();
            }
        }

        private async Task AddSelectionCommand(string trimPrefix)
        {
            var argSplit = trimPrefix.Split(' ');
            if (int.TryParse(argSplit[1], out var index))
            {
                if (int.TryParse(argSplit[2], out var count))
                {
                    if (await ValidateSelectionInput(CurrentItemPoolPage, index, count))
                    {
                        AddSelection(CurrentItemPoolPage[index], count);
                        await UpdateView();
                    }
                }
            }
        }

        private async Task<bool> ValidateSelectionInput(List<StorageItem> pool, int index, int count)
        {
            if (pool.Count <= index)
            {
                await UpdateResponseMessage(pool.Count == 0 ? "Nothing to select" : $"Invalid index, expected from 0 to {pool.Count - 1}");
                return false;
            }

            if (pool[index].Count < count)
            {
                await UpdateResponseMessage($"You don't have enough {pool[index].Item.Name} to select that many.");
                return false;
            }

            return true;
        }

        private void AddSelection(StorageItem item, int count)
        {
            var exist = SelectedItems.FirstOrDefault(x => x.Id == item.Id);
            if (exist == null)
            {
                exist = item.Clone() as StorageItem;
                exist.Count = count;
                SelectedItems.Add(exist);
            }
            else
            {
                exist.Count = count;
            }
        }

        private async Task UpdateResponseMessage(string content = null, Embed eb = null)
        {
            if (_ResponseMessage != null)
                await _ResponseMessage.ModifyAsync(x => { x.Content = content; x.Embed = eb; });
            else
                _ResponseMessage = await _ViewMessage.Channel.SendMessageAsync(content, embed: eb);

        }

        private string GenerateFilterListString()
        {
            StringBuilder bd = new StringBuilder();
            foreach (var item in _StorageQuerier.GetFilters())
            {
                bd.Append($"{item.Name} ");
            }
            return bd.ToString();
        }

        public override async Task LoadView(IMessageChannel channel)
        {
            await base.LoadView(channel);
            GenerateInstruction();
            GeneratePickerFooter();

            _ViewMessage = await channel.SendMessageAsync(embed: await GenerateEmbed());

            await AddReactionButton(_PageUpEmoji, _ViewMessage, btnPageUp_Click);
            await AddReactionButton(_PageDownEmoji, _ViewMessage, btnPageDown_Click);
            await AddReactionButton(_PageUpSelectedEmoji, _ViewMessage, btnSelectedPageUp_Click);
            await AddReactionButton(_PageDownSelectedEmoji, _ViewMessage, btnSelectedPageDown_Click);
            await AddReactionButton(_SubmitEmoji, _ViewMessage, btnSubmit_Click);
            await AddReactionButton(_BackEmoji, _ViewMessage, btnCancel_Click);
        }

        private Task btnCancel_Click(Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2, SocketReaction arg3)
        {
            OnCanceled();
            return Task.CompletedTask;
        }

        private async Task btnSubmit_Click(Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2, SocketReaction arg3)
        {
            long sum = SelectedItems.Sum(x => x.Count);

            if (SelectedItems.Count > Request.MaxStorageItemCount)
                await UpdateResponseMessage("You have picked too many item slots, please remove some item slots and try again");
            else if (sum > Request.MaxItemCount)
                await UpdateResponseMessage("You have picked too many items, please remove some items and try again");
            else if (SelectedItems.Count < Request.MinStorageItemCount)
                await UpdateResponseMessage("You have picked too few item slots, please add some item slots and try again");
            else if (sum < Request.MinItemCount)
                await UpdateResponseMessage("You have picked too few items, please add some items and try again");
            else
                OnSuccess(SelectedItems);
        }

        private async Task btnSelectedPageDown_Click(Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2, SocketReaction arg3)
        {
            _ =  _ViewMessage.RemoveReactionAsync(arg3.Emote, arg3.User.Value);
            if (PageDownable(SelectedItems.Count))
            {
                _SelectedItemPage++;
                await UpdateView();
            }
        }

        private async Task btnSelectedPageUp_Click(Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2, SocketReaction arg3)
        {
            _ = _ViewMessage.RemoveReactionAsync(arg3.Emote, arg3.User.Value);
            if (_SelectedItemPage > 0)
            {
                _SelectedItemPage--;
                await UpdateView();
            }
        }

        private async Task btnPageDown_Click(Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2, SocketReaction arg3)
        {
            _ = _ViewMessage.RemoveReactionAsync(arg3.Emote, arg3.User.Value);
            if (PageDownable(_LastSearchResultCount))
            {
                _ItemPoolPage++;
                await UpdateView();
            }
        }

        private async Task btnPageUp_Click(Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2, SocketReaction arg3)
        {
            _ = _ViewMessage.RemoveReactionAsync(arg3.Emote, arg3.User.Value);
            if (_ItemPoolPage > 0)
            {
                _ItemPoolPage--;
                await UpdateView();
            }
        }

        private async Task UpdateView()
        {
            var embed = await GenerateEmbed();
            await _ViewMessage.ModifyAsync(x => x.Embed = embed);
        }

        private void GenerateInstruction()
        {
            StringBuilder instructionBuilder = new StringBuilder();
            instructionBuilder.AppendLine("To select an item, use: `<prefix>sel <index> <quantity>`");
            instructionBuilder.AppendLine("To de-select an item use: `<prefix>desel <index>");
            instructionBuilder.AppendLine("To search for an item, use: `<prefix>qs <search keyword>`");
            instructionBuilder.AppendLine($"React with {_PageUpEmoji} and {_PageDownEmoji} to navigate between item pool pages");
            instructionBuilder.AppendLine($"React with {_PageUpSelectedEmoji} and {_PageDownEmoji} to navigate between selected item pages");
            if (Request.FiltersChangeableByUser)
            {
                instructionBuilder.AppendLine("To change type filter, use `<prefix>tf` to see a list of type filters");
            }
            if (Request.Cancelable)
            {
                instructionBuilder.AppendLine($"React with {_BackEmoji} to cancel");
            }

            _InstructionsContent = instructionBuilder.ToString();
        }

        private void GeneratePickerFooter()
        {
            StringBuilder bd = new StringBuilder();
            bd.AppendLine("How to use the picker use: `<prefix>iph`");
            if (Request.Cancelable)
                bd.AppendLine($"React with {_BackEmoji} to cancel");

            _PickerFooter = bd.ToString();
        }

        private async Task<Embed> GenerateEmbed()
        {
            IAsyncEnumerable<StorageItem> result = _StorageQuerier.Search();
            CurrentItemPoolPage.Clear();
            CurrentSelectedItemPage.Clear();
            _LastSearchResultCount = await result.CountAsync();
            var interatedEnumerable = result.Skip(ItemPerPage * _ItemPoolPage).Take(ItemPerPage);

            StringBuilder itemPoolSb = new StringBuilder();
            int i = 0;
            await foreach (var sitem in interatedEnumerable)
            {
                CurrentItemPoolPage.Add(sitem);
                var itemEmoji = await DataAccess.EmojiLookups.GetItemIcon(sitem.Item, FallbackEmoji);
                itemPoolSb.AppendLine($"{i}. {itemEmoji} {sitem.Item.Name} (**{sitem.Count}**)");
                i++;
            }
            if (i == 0)
            {
                itemPoolSb.AppendLine("The items pool should be shown here. Idk where did they go though :hmm:");
            }

            StringBuilder selectedItemSb = new StringBuilder();
            i = 0;
            long selectedItemCount = 0;
            long selectedStorageItemCount = 0;
            foreach(var sitem in SelectedItems)
            {
                CurrentSelectedItemPage.Add(sitem);
                var itemEmoji = await DataAccess.EmojiLookups.GetItemIcon(sitem.Item, FallbackEmoji);
                selectedItemSb.AppendLine($"{i}. {itemEmoji} {sitem.Item.Name} (**{sitem.Count}**)");
                i++;
                selectedItemCount += sitem.Count;
                selectedStorageItemCount++;
            }
            if (SelectedItems.Count == 0)
            {
                selectedItemSb.AppendLine("Your selected items will be shown here");
            }


            string itemPoolHeader = $"{(_ItemPoolPage > 0 ? _PageUpEmoji : "")} Page {_ItemPoolPage} {(PageDownable(_LastSearchResultCount) ? _PageDownEmoji : "")}";
            string seletedItemHeader = $"{(_SelectedItemPage > 0 ? _PageUpSelectedEmoji : "")} Selected item (Page {_SelectedItemPage}) {(PageDownable(SelectedItems.Count) ? _PageDownSelectedEmoji : "")}";

            EmbedBuilder eb = new EmbedBuilder()
                .WithTitle($"**{Request.PickerTitle}**")
                .WithDescription($"{_SearchEmoji}: {(string.IsNullOrEmpty(_SearchKeyword) ? "`<prefix>qs <keywords>` to search" : _SearchEmoji )}\n{_FilterEmoji} Filters: {_FilterListString}")
                .AddField("Requirements", $"Item count: {Request.MinItemCount}-{Request.MaxItemCount} (**{selectedItemCount}**)\nSlot count: {Request.MinStorageItemCount}-{Request.MaxStorageItemCount} (**{selectedStorageItemCount}**)" )
                .AddField(itemPoolHeader, itemPoolSb.ToString(), true)
                .AddField(seletedItemHeader, selectedItemSb.ToString(), true)
                .WithFooter(_PickerFooter);
            return eb.Build();
        }

        private bool PageDownable(int entryCount)
        {
            return entryCount > (_ItemPoolPage + 1) * ItemPerPage;
        }

        protected virtual void OnSuccess(List<StorageItem> items) => Success?.Invoke(this, new ValueEventArgs<List<StorageItem>>(items));
        protected virtual void OnCanceled() => Canceled?.Invoke(this, EventArgs.Empty);

        public event EventHandler<ValueEventArgs<List<StorageItem>>> Success;
        public event EventHandler<EventArgs> Canceled;
    }
}

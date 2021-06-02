using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsekaiTechnologies.FacCord.Discord.Utils
{
    internal class UIEmojiLookupTable : IUIEmojiLookupTable
    {
        public UIEmojiLookupTable()
        {

        }

        public UIEmojiLookupTable(string externalConfig)
        {
            _ExternalConfig = externalConfig;
        }

        protected string _ExternalConfig;
        protected UIEmojiLookupTableModel _Data;
        public bool IsLoaded { get; set; }

        public async Task<string> GetUIEmoji(string name, string fallbackEmoji)
        {
            if (!IsLoaded)
            {
                await Reload();
            }
            return _Data.NamedEmojis.ContainsKey(name) ? _Data.NamedEmojis[name] : fallbackEmoji;
        }

        public async Task Reload()
        {
            string content = await File.ReadAllTextAsync(_ExternalConfig ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UIEmojiLookupTable.json"));
            _Data = JsonConvert.DeserializeObject<UIEmojiLookupTableModel>(content);
            IsLoaded = true;
        }

        public IAsyncEnumerable<string> GetEmptyEmojis()
        {
            return _Data.EmptyEmojis.ToAsyncEnumerable();
        }
    }

    class UIEmojiLookupTableModel
    {
        public Dictionary<string, string> NamedEmojis { get; set; }
        public List<string> EmptyEmojis { get; set; }
    }
}

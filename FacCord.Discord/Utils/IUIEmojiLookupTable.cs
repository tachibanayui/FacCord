using System.Collections.Generic;
using System.Threading.Tasks;

namespace IsekaiTechnologies.FacCord.Discord.Utils
{
    public interface IUIEmojiLookupTable
    {
        bool IsLoaded { get; set; }

        Task<string> GetUIEmoji(string name, string fallbackEmoji);
        IAsyncEnumerable<string> GetEmptyEmojis();
        Task Reload();
    }
}
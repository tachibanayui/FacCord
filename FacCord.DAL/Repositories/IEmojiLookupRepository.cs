using IsekaiTechnologies.FacCord.Core.Models;
using IsekaiTechnologies.FacCord.Core.Models.Mines;
using IsekaiTechnologies.FacCord.Core.Models.Storages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IsekaiTechnologies.FacCord.Core.DAL.Repositories
{
    public interface IEmojiLookupRepository : IRepository<EmojiLookup>
    {
        Task<EmojiLookup> GetEmoji(long targetId, int type);

        Task<string> GetItemIcon(Item item, string fallbackIcon);
        Task<string> GetBlockIcon(Block block, string fallbackIcon);
        Task<string> GetRegionIcon(Region region, string fallbackIcon);
        Task<string> GetDimensionIcon(Dimension dimension, string fallbackIcon);
    }
}

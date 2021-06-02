using IsekaiTechnologies.FacCord.Core.DAL.Repositories;
using IsekaiTechnologies.FacCord.Core.Models;
using IsekaiTechnologies.FacCord.Core.Models.Mines;
using IsekaiTechnologies.FacCord.Core.Models.Storages;
using System.Linq;
using System.Threading.Tasks;

namespace IsekaiTechnologies.FacCord.Core.DAL.EntityFramework.Repositories
{
    public class EmojiLookupRepository : Repository<EmojiLookup>, IEmojiLookupRepository
    {
        public EmojiLookupRepository(FacCordContext ctx) : base(ctx)
        {
        }

        public async Task<EmojiLookup> GetEmoji(long targetId, int type)
        {
            return await FindAsync(p => p.Type == type && p.TargetId == targetId).FirstOrDefaultAsync();
        }

        public async Task<string> GetBlockIcon(Block block, string fallbackIcon)
        {
            if (block == null)
                return fallbackIcon;

            var res = await GetEmoji(block.Item.Id, 1);
            return res == null ? fallbackIcon : res.Emoji;
        }

        public async Task<string> GetItemIcon(Item item, string fallbackIcon)
        {
            if (item == null)
                return fallbackIcon;

            var res = await GetEmoji(item.Id, 0);
            return res == null ? fallbackIcon : res.Emoji;
        }

        public async Task<string> GetRegionIcon(Region region, string fallbackIcon)
        {
            if (region == null)
                return fallbackIcon;

            var res = await GetEmoji(region.Id, 0);
            return res == null ? fallbackIcon : res.Emoji;
        }

        public async Task<string> GetDimensionIcon(Dimension dimension, string fallbackIcon)
        {
            if (dimension == null)
                return fallbackIcon;

            var res = await GetEmoji(dimension.Id, 0);
            return res == null ? fallbackIcon : res.Emoji;
        }
    }
}

using IsekaiTechnologies.FacCord.Core.DAL.Repositories;
using IsekaiTechnologies.FacCord.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace IsekaiTechnologies.FacCord.Core.DAL.EntityFramework.Repositories
{
    public class ProfileRepository : Repository<Profile>, IProfileRepository
    {
        public ProfileRepository(FacCordContext ctx) : base(ctx)
        {
        }

        public Task<Profile> GetActiveProfileAsync(Player player)
        {
            return Task.FromResult(Context.Profiles.FirstOrDefault(x => x.Player.Id == player.Id && x.IsActive));
        }

        public async Task<Profile> GetActiveProfileForMineController(Player player)
        {
            return await Context.Profiles
               .Include(x => x.CurrentProspector).ThenInclude(x => x.Item)
               .Include(x => x.CurrentDrill).ThenInclude(x => x.Item)
               .Include(x => x.SelectedRegion)
               .Include(x => x.SelectedRegion.Dimension)
               .Include(x => x.SelectedRegion.MaterialDistribution)
               .Include(x => x.SelectedRegion.MaterialDistribution).ThenInclude(x => x.Block)
               .Include(x => x.SelectedRegion.MaterialDistribution).ThenInclude(x => x.Block.Item)
               .Include(x => x.UnlockedRegions)
               .FirstOrDefaultAsync(x => x.Player.Id == player.Id && x.IsActive);
        }

        public async Task<Profile> GetActiveProfileForMineDiscoveryAsync(Player player)
        {
            return await Context.Profiles
                .Include(x => x.CurrentProspector)
                .ThenInclude(x => x.Item)
                .Include(x => x.CurrentDrill)
                .ThenInclude(x => x.Item)
                .Include(x => x.SelectedRegion)
                .Include(x => x.SelectedRegion.Dimension)
                .Include(x => x.SelectedRegion.MaterialDistribution)
                .Include(x => x.SelectedRegion.MaterialDistribution).ThenInclude(x => x.Block)
                .Include(x => x.SelectedRegion.MaterialDistribution).ThenInclude(x => x.Block.Item)
                .Include(x => x.UnlockedRegions)
                .Include(x => x.Workshop)
                .Include(x => x.Workshop.Storage).ThenInclude(x => x.Item)
                .FirstOrDefaultAsync(x => x.Player.Id == player.Id && x.IsActive);
        }

        public async Task<Profile> GetActiveProfileForRegionSelectionAsync(Player player)
        {
            return await Context.Profiles
                .Include(x => x.CurrentProspector).ThenInclude(x => x.Item)
                .Include(x => x.CurrentDrill).ThenInclude(x => x.Item)
                .Include(x => x.SelectedRegion)
                .Include(x => x.SelectedRegion.Dimension)
                .Include(x => x.SelectedRegion.Dimension.PossibleResources)
                .Include(x => x.SelectedRegion.MaterialDistribution)
                .Include(x => x.SelectedRegion.MaterialDistribution).ThenInclude(x => x.Block)
                .Include(x => x.SelectedRegion.MaterialDistribution).ThenInclude(x => x.Block.Item)
                .Include(x => x.UnlockedRegions)
                .FirstOrDefaultAsync(x => x.Player.Id == player.Id && x.IsActive);
        }

        public async Task<Profile> GetActiveProfileForTitleAsync(Player player)
        {
            return await Context.Profiles
                .Include(x => x.CurrentProspector)
                .ThenInclude(x => x.Item)
                .Include(x => x.CurrentDrill)
                .ThenInclude(x => x.Item)
                .Include(x => x.SelectedRegion)
                .ThenInclude(x => x.Dimension)
                .Include(x => x.UnlockedRegions)
                .Include(x => x.RuptureChemical)
                .Include(x => x.Workshop)
                .FirstOrDefaultAsync(x => x.Player.Id == player.Id && x.IsActive);
        }
    }
}

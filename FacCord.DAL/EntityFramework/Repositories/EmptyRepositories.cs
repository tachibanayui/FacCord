using IsekaiTechnologies.FacCord.Core.DAL.Repositories;
using IsekaiTechnologies.FacCord.Core.DAL.Repositories.Discord;
using IsekaiTechnologies.FacCord.Core.DAL.Repositories.Equipments;
using IsekaiTechnologies.FacCord.Core.DAL.Repositories.Machines;
using IsekaiTechnologies.FacCord.Core.DAL.Repositories.Mines;
using IsekaiTechnologies.FacCord.Core.DAL.Repositories.Shops;
using IsekaiTechnologies.FacCord.Core.DAL.Repositories.Storages;
using IsekaiTechnologies.FacCord.Core.DAL.Repositories.Storages.ItemMetas;
using IsekaiTechnologies.FacCord.Core.Models;
using IsekaiTechnologies.FacCord.Core.Models.Discord;
using IsekaiTechnologies.FacCord.Core.Models.Equipments;
using IsekaiTechnologies.FacCord.Core.Models.Machines;
using IsekaiTechnologies.FacCord.Core.Models.Mines;
using IsekaiTechnologies.FacCord.Core.Models.Shops;
using IsekaiTechnologies.FacCord.Core.Models.Storages;
using IsekaiTechnologies.FacCord.Core.Models.Storages.ItemMetas;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace IsekaiTechnologies.FacCord.Core.DAL.EntityFramework.Repositories
{
    public class PlayerRepository : Repository<Player>, IPlayerRepository
    {
        public PlayerRepository(FacCordContext ctx) : base(ctx)
        {
        }

        public async Task<Player> GetPlayerByDiscordId(long discordId)
        {
            return await FindAsync(x => x.DiscordId == discordId).FirstAsync();
        }
    }

    public class PlayerSettingsRepository : Repository<PlayerSettings>, IPlayerSettingsRepository
    {
        public PlayerSettingsRepository(FacCordContext ctx) : base(ctx)
        {
        }
    }

    public class QuestRepository : Repository<Quest>, IQuestRepository
    {
        public QuestRepository(FacCordContext ctx) : base(ctx)
        {
        }
    }

    public class RecipeRepository : Repository<Recipe>, IRecipeRepository
    {
        public RecipeRepository(FacCordContext ctx) : base(ctx)
        {
        }
    }

    public class ProspectorRepository : Repository<Prospector>, IProspectorRepository
    {
        public ProspectorRepository(FacCordContext ctx) : base(ctx)
        {
        }

        public async Task<Prospector> GetStarterProspector()
        {
            return await FindAsync(x => x.IdName == "faccord:prospector_test").FirstAsync();
        }
    }

    public class RuptureChemicalRepository : Repository<RuptureChemical>, IRuptureChemicalRepository
    {
        public RuptureChemicalRepository(FacCordContext ctx) : base(ctx)
        {
        }
    }

    public class EnergyProductionMachineUnitRepository : Repository<EnergyProductionMachineUnit>, IEnergyProductionMachineUnitRepository
    {
        public EnergyProductionMachineUnitRepository(FacCordContext ctx) : base(ctx)
        {
        }
    }

    public class MachineRepository : Repository<Machine>, IMachineRepository
    {
        public MachineRepository(FacCordContext ctx) : base(ctx)
        {
        }
    }

    public class MachineUnitRepository : Repository<MachineUnit>, IMachineUnitRepository
    {
        public MachineUnitRepository(FacCordContext ctx) : base(ctx)
        {
        }
    }

    public class ProcessingMachineUnitRepository : Repository<ProcessingMachineUnit>, IProcessingMachineUnitRepository
    {
        public ProcessingMachineUnitRepository(FacCordContext ctx) : base(ctx)
        {
        }
    }

    public class BlockRepository : Repository<Block>, IBlockRepository
    {
        public BlockRepository(FacCordContext ctx) : base(ctx)
        {
        }

        public async Task<Block> GetAsyncEagerly(long v)
        {
            return await Context.Blocks.Include(x => x.Item)
                .Include(x => x.DropTable)
                .ThenInclude(x => x.Item)
                .FirstAsync(x => x.Id == v);
        }

        public async Task<Block> GetBlockByItemIdNameAsync(string id)
        {
            return await Context.Blocks.Include(x => x.Item).FirstAsync(x => x.Item.IdName == id);
        }
    }

    public class BlockSpawnChanceRepository : Repository<BlockSpawnChance>, IBlockSpawnChanceRepository
    {
        public BlockSpawnChanceRepository(FacCordContext ctx) : base(ctx)
        {
        }
    }

    public class DimensionRepository : Repository<Dimension>, IDimensionRepository
    {
        public DimensionRepository(FacCordContext ctx) : base(ctx)
        {
        }

        public IAsyncEnumerable<Dimension> GetAllWidthPossibleResourcesAsync()
        {
            return Context.Dimensions.Include(x => x.PossibleResources).AsAsyncEnumerable();
        }
    }

    public class RegionRepository : Repository<Region>, IRegionRepository
    {
        public RegionRepository(FacCordContext ctx) : base(ctx)
        {
        }

        public async Task<Region> GetStarterRegion()
        {
            return await FindAsync(x => x.IdName == "faccord:overworld_surface").FirstAsync();
        }
    }

    public class ExclusiveListingRepository : Repository<ExclusiveListings>, IExclusiveListingsRepository
    {
        public ExclusiveListingRepository(FacCordContext ctx) : base(ctx)
        {
        }
    }

    public class ListingRepository : Repository<Listing>, IListingRepository
    {
        public ListingRepository(FacCordContext ctx) : base(ctx)
        {
        }
    }

    public class TransactionRepository : Repository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(FacCordContext ctx) : base(ctx)
        {
        }
    }

    public class ItemRepository : Repository<Item>, IItemRepository
    {
        public ItemRepository(FacCordContext ctx) : base(ctx)
        {
        }

        public async Task<Item> GetItemByIdName(string idName)
        {
            return await FindAsync(x => x.IdName == idName).FirstAsync();
        }
    }

    public class ServerSettingsRepository : Repository<ServerSettings>, IServerSettingsRepository
    {
        public ServerSettingsRepository(FacCordContext ctx) : base(ctx)
        {

        }

        public async Task<bool> ExistByDiscordId(long id)
        {
            return await FindAsync(x => x.DiscordServerId == id).AnyAsync();
        }

        public async Task<ServerSettings> GetServerSettingsByDiscordId(long discordId)
        {
            return await FindAsync(x => x.DiscordServerId == discordId).FirstAsync();
        }
    }

    public class ItemMetaRepository : Repository<ItemMeta>, IItemMetaRepository
    {
        public ItemMetaRepository(FacCordContext ctx) : base(ctx)
        {
        }
    }

    public class DrillItemMetaRepository : Repository<DrillItemMeta>, IDrillItemMetaRepository
    {
        public DrillItemMetaRepository(FacCordContext ctx) : base(ctx)
        {
        }
    }
}

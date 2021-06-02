using IsekaiTechnologies.FacCord.Core.DAL.EntityFramework.Repositories;
using IsekaiTechnologies.FacCord.Core.DAL.Repositories;
using IsekaiTechnologies.FacCord.Core.DAL.Repositories.Discord;
using IsekaiTechnologies.FacCord.Core.DAL.Repositories.Equipments;
using IsekaiTechnologies.FacCord.Core.DAL.Repositories.Machines;
using IsekaiTechnologies.FacCord.Core.DAL.Repositories.Mines;
using IsekaiTechnologies.FacCord.Core.DAL.Repositories.Shops;
using IsekaiTechnologies.FacCord.Core.DAL.Repositories.Storages;
using IsekaiTechnologies.FacCord.Core.DAL.Repositories.Storages.ItemMetas;
using System;
using System.Collections.Generic;
using System.Text;

namespace IsekaiTechnologies.FacCord.Core.DAL.EntityFramework
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(FacCordContext ctx)
        {
            Context = ctx;
        }

        public FacCordContext Context { get; protected set; }

        public IDrillRepository Drills => new DrillRepository(Context);
        public IProspectorRepository Prospectors => new ProspectorRepository(Context);
        public IRuptureChemicalRepository RuptureChemicals => new RuptureChemicalRepository(Context);
        public IEnergyProductionMachineUnitRepository EnergyProductionMachineUnits => new EnergyProductionMachineUnitRepository(Context);
        public IMachineUnitRepository MachineUnits => new MachineUnitRepository(Context);
        public IMachineRepository Machines => new MachineRepository(Context);
        public IProcessingMachineUnitRepository ProcessingMachineUnits => new ProcessingMachineUnitRepository(Context);
        public IStorageMachineRepository StorageMachines => new StorageMachineRepository(Context);
        public IBlockRepository Blocks => new BlockRepository(Context);
        public IBlockSpawnChanceRepository BlockSpawnChances => new BlockSpawnChanceRepository(Context);
        public IDimensionRepository Dimensions => new DimensionRepository(Context);
        public IRegionRepository Regions => new RegionRepository(Context);
        public IExclusiveListingsRepository ExclusiveListingses => new ExclusiveListingRepository(Context);
        public IListingRepository Listings => new ListingRepository(Context);
        public ITransactionRepository Transactions => new TransactionRepository(Context);
        public IStorageItemRepository StorageItems => new StorageItemRepository(Context);
        public IItemRepository Items => new ItemRepository(Context);
        public IPlayerRepository Players => new PlayerRepository(Context);
        public IPlayerSettingsRepository PlayerSettingses => new PlayerSettingsRepository(Context);
        public IProfileRepository Profiles => new ProfileRepository(Context);
        public IQuestRepository Quests => new QuestRepository(Context);
        public IRecipeRepository Recipes => new RecipeRepository(Context);
        public IWorkshopRepository Workshops => new WorkshopRepository(Context);
        public IEmojiLookupRepository EmojiLookups => new EmojiLookupRepository(Context);
        public IServerSettingsRepository ServerSettings => new ServerSettingsRepository(Context);
        public IItemMetaRepository ItemMetaRepository => new ItemMetaRepository(Context);
        public IDrillItemMetaRepository DrillItemMetaRepository => new DrillItemMetaRepository(Context);

        public int Complete()
        {
            return Context.SaveChanges();
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}

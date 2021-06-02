using IsekaiTechnologies.FacCord.Core.DAL.Repositories.Equipments;
using IsekaiTechnologies.FacCord.Core.DAL.Repositories.Machines;
using IsekaiTechnologies.FacCord.Core.DAL.Repositories.Mines;
using IsekaiTechnologies.FacCord.Core.DAL.Repositories.Shops;
using IsekaiTechnologies.FacCord.Core.DAL.Repositories.Storages;
using IsekaiTechnologies.FacCord.Core.DAL.Repositories;
using System;
using IsekaiTechnologies.FacCord.Core.DAL.Repositories.Discord;
using IsekaiTechnologies.FacCord.Core.DAL.Repositories.Storages.ItemMetas;

namespace IsekaiTechnologies.FacCord.Core.DAL
{
    public interface IUnitOfWork : IDisposable
    {
        IDrillRepository Drills { get; }
        IProspectorRepository Prospectors { get; }
        IRuptureChemicalRepository RuptureChemicals { get; }
        IEnergyProductionMachineUnitRepository EnergyProductionMachineUnits { get; }
        IMachineRepository Machines { get; }
        IStorageMachineRepository StorageMachines { get; }
        IMachineUnitRepository MachineUnits { get; }
        IProcessingMachineUnitRepository ProcessingMachineUnits { get; }
        IBlockRepository Blocks { get; }
        IBlockSpawnChanceRepository BlockSpawnChances { get; }
        IDimensionRepository Dimensions { get; }
        IRegionRepository Regions { get; }
        IExclusiveListingsRepository ExclusiveListingses { get; }
        IListingRepository Listings { get; }
        ITransactionRepository Transactions { get; }
        IStorageItemRepository StorageItems { get; }
        IItemRepository Items { get; }
        IPlayerRepository Players { get; }
        IPlayerSettingsRepository PlayerSettingses { get; }
        IProfileRepository Profiles { get; }
        IQuestRepository Quests { get; }
        IRecipeRepository Recipes { get; }
        IWorkshopRepository Workshops { get; }
        IEmojiLookupRepository EmojiLookups { get; }
        IServerSettingsRepository ServerSettings { get; }
        IItemMetaRepository ItemMetaRepository { get; }
        IDrillItemMetaRepository DrillItemMetaRepository { get; }
        
        int Complete();
    }
}

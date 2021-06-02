using IsekaiTechnologies.FacCord.Core.DAL;
using IsekaiTechnologies.FacCord.Core.Models;
using IsekaiTechnologies.FacCord.Core.Models.Equipments;
using IsekaiTechnologies.FacCord.Core.Models.Machines;
using IsekaiTechnologies.FacCord.Core.Models.Mines;
using IsekaiTechnologies.FacCord.Core.Models.Storages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IsekaiTechnologies.FacCord.Core.Providers
{
    public class ProfileProvider : IProfileProvider
    {
        public IUnitOfWork DataAccess { get; private set; }

        public ProfileProvider(IUnitOfWork dataAccess)
        {
            DataAccess = dataAccess;
        }

        public async Task<Profile> CreateNewProfile(Player player, string name)
        {
            Profile profile = new Profile();

            profile.Player = player;
            profile.PlayerEmoji = "⛏️";
            profile.RuptureChemical = RuptureChemical.CreateNew();
            var starterStorageMachine = await DataAccess.StorageMachines.GetStarterStorageMachine();
            var starterRegion = await DataAccess.Regions.GetStarterRegion();
            profile.Workshop = new Workshop()
            {
                SizeX = 5,
                SizeY = 5,
                WeightCapacity = 25,
                Units = new List<MachineUnit>()
                {
                    new MachineUnit() { PosX = 0, PosY = 0, Machine = starterStorageMachine }
                },
                StorageCapacity = starterStorageMachine.Capacity,
                SupportedWeight = starterStorageMachine.Weight,
                LastUpdated = DateTime.Now,
                Storage = new List<StorageItem>(),
            };
            profile.CurrentProspector = new StorageItem() { Count = 1, Item = await DataAccess.Prospectors.GetStarterProspector() };
            profile.CurrentDrill = new StorageItem() { Count = 1, Item = await DataAccess.Drills.GetStarterDrill() };
            profile.SelectedRegion = starterRegion;
            profile.UnlockedRegions = new List<Region>() { starterRegion };

            DataAccess.Complete();
            return profile;
        }
    }
}

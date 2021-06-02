using IsekaiTechnologies.FacCord.Core.DAL;
using IsekaiTechnologies.FacCord.Core.EquipmentLogic;
using IsekaiTechnologies.FacCord.Core.MineGeneration;
using IsekaiTechnologies.FacCord.Core.Providers;
using IsekaiTechnologies.FacCord.Core.StorageManagements;
using System;
using System.Collections.Generic;
using System.Text;

namespace IsekaiTechnologies.FacCord.Core
{
    public class GameSettings
    {
        public GameConfigs Configurations { get; set; }
        public IUnitOfWork DataAccess { get; set; }
    }

    public class GameConfigs
    {
        public IMineConfig MineConfiguration { get; set; }
        public IEquipmentConfig EquipmentConfiguraton { get; set; }
        public IExperienceLevelInfoProvider ExpToLevelConfiguration { get; set; }
        public IProfileProvider ProfileProvider { get; set; }
        public IControllerProvider ControllerProvider { get; set; }
        public IStoragManagementProvider StoragManagementProvider { get; set; }

        public static GameConfigs GetDefaultConfiguration(IUnitOfWork dataAccess) 
        {
            return new GameConfigs()
            {
                MineConfiguration = new MineConfig(),
                EquipmentConfiguraton = new EquipmentConfig(),
                ExpToLevelConfiguration = new ExperienceLevelInfoProvider(),
                ProfileProvider = new ProfileProvider(dataAccess),
                ControllerProvider = new ControllerProvider(),
                StoragManagementProvider = new StorageManagementProvider()
            };
        }
    }
}

using IsekaiTechnologies.FacCord.Core.DAL;
using IsekaiTechnologies.FacCord.Core.Models.Equipments;
using IsekaiTechnologies.FacCord.Core.Models.Machines;
using IsekaiTechnologies.FacCord.Core.Models.Mines;
using IsekaiTechnologies.FacCord.Core.Models.Storages;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TempProject
{
    [TestGallery("Generate Sql Sample Data", Description = "Run this to add sample data")]
    public class FacCordSqlSampleData : FacCordTestGallery
    {
        public override string Report()
        {
            return "";
        }

        public override void Run(string[] args)
        {
            Console.WriteLine("This will generate new sample records, proceed with caution! Type \"YES\" to regerate");
            if (Console.ReadLine() == "YES")
            {
                RunAsync(args).GetAwaiter().GetResult();
            }
        }

        public async Task RunAsync(string[] args)
        {
            var sc = new ServiceCollection();
            ConfigureServices(sc);
            Service = sc.BuildServiceProvider();

            var dataAccess = Service.GetRequiredService<IUnitOfWork>();

            #region Block & Item
            Block air = new Block()
            {
                Hardness = -1,
                Item = new Item() { Name = "Air", IdName = "faccord:air", Description = "Nothing but o2!" },
                IsSolid = false,
            };
            await dataAccess.Blocks.AddAsync(air);

            Block blockRedstone = new Block()
            {
                DropTable = new List<StorageItem>()
                {
                    new StorageItem() { Item = new Item() { Name = "Redstone", IdName = "faccord:redstone", Description = "The core of automation" } }
                },
                Hardness = 2,
                IsSolid = true,
                Item = new Item() { Name = "Redstone block", Description = "Glowing red" },
                Replacement = air,
                Toughness = 2
            };
            await dataAccess.Blocks.AddAsync(blockRedstone);

            for (int i = 0; i < 100; i++)
            {
                Block block = new Block()
                {
                    Hardness = 2,
                    IsSolid = true,
                    Item = new Item() { Name = $"Block #{i}", IdName = "faccord:item_{i}", Description = "Sample data for testing" },
                    Replacement = air,
                    Toughness = 2
                };

                await dataAccess.Blocks.AddAsync(block);
            }
            #endregion

            #region Machines
            StorageMachine storage1k = new StorageMachine()
            {
                Name = "1k storage machine",
                IdName = "faccord:storage_1k",
                Description = "This machine can hold 1000 item",
                Weight = 5,
                Width = 1,
                Height = 1,
                Capacity = 1000
            };
            await dataAccess.StorageMachines.AddAsync(storage1k);
            #endregion

            #region Dimensions & Regions
            Dimension overworld = new Dimension()
            {
                Name = "Overworld",
                IdName = "faccord:overworld",
                Description = "What a peaceful place. This is a home to many friendly creatures, or is it?",
            };
            await dataAccess.Dimensions.AddAsync(overworld);

            Dimension nether = new Dimension()
            {
                Name = "Nether",
                IdName = "faccord:nether",
                Description = "You shouldn't visit *_*",
            };
            await dataAccess.Dimensions.AddAsync(nether);

            Region surface = new Region()
            {
                IdName = "faccord:overworld_surface",
                Name = "Overworld Surface",
                Description = "Your adventure start here!",
                Dimension = overworld,
                RegionLevel = 1,
                MaterialDistribution = new List<BlockSpawnChance>()
                {
                    new BlockSpawnChance() { Block = await dataAccess.Blocks.GetAsync(1), Chance = 0.5 },
                    new BlockSpawnChance() { Block = await dataAccess.Blocks.GetAsync(2), Chance = 0.3 },
                    new BlockSpawnChance() { Block = await dataAccess.Blocks.GetAsync(3), Chance = 0.2 },
                }
            };
            await dataAccess.Regions.AddAsync(surface);

            Region underground = new Region()
            {
                IdName = "faccord:overworld_underground",
                Name = "Overworld Underground",
                Description = "Where most people go after 10min of their minecraft survival world. Sometimes even sooner",
                Dimension = overworld,
                RegionLevel = 2,
                MaterialDistribution = new List<BlockSpawnChance>()
                {
                    new BlockSpawnChance() { Block = await dataAccess.Blocks.GetAsync(1), Chance = 0.5 },
                    new BlockSpawnChance() { Block = await dataAccess.Blocks.GetAsync(2), Chance = 0.3 },
                    new BlockSpawnChance() { Block = await dataAccess.Blocks.GetAsync(3), Chance = 0.2 },
                }
            };
            await dataAccess.Regions.AddAsync(underground);

            Region hell = new Region()
            {
                IdName = "faccord:nether_hell",
                Name = "Nether Hell",
                Description = "Play with ghast",
                Dimension = nether,
                RegionLevel = 3,
                MaterialDistribution = new List<BlockSpawnChance>()
                {
                    new BlockSpawnChance() { Block = await dataAccess.Blocks.GetAsync(1), Chance = 0.5 },
                    new BlockSpawnChance() { Block = await dataAccess.Blocks.GetAsync(2), Chance = 0.3 },
                    new BlockSpawnChance() { Block = await dataAccess.Blocks.GetAsync(3), Chance = 0.2 },
                }
            };
            await dataAccess.Regions.AddAsync(hell);
            #endregion

            Prospector pp = new Prospector()
            {
                IdName = "faccord:prospector_test",
                Name = "Test prospector",
                Description = "Sample data for testing",
                MineRichness = 2,
                OreVein = 2,
                RegionLevel = 10,
                RangeX = 100,
                RangeY = 100
            };
            await dataAccess.Prospectors.AddAsync(pp);

            Prospector pp2 = new Prospector()
            {
                IdName = "faccord:prospector_test_2",
                Name = "Test prospector2",
                Description = "Sample data for testing",
                MineRichness = 10,
                OreVein = 10,
                RegionLevel = 1,
                RangeX = 100,
                RangeY = 100
            };
            await dataAccess.Prospectors.AddAsync(pp2);

            Drill drill = new Drill()
            {
                IdName = "faccord:drill_test",
                Name = "Test drill",
                Description = "Sample data for testing",
                Durability = 100,
                Efficiency = 10,
                Hardness = 1,
                RangeX = 3,
                RangeY = 3,
            };
            await dataAccess.Drills.AddAsync(drill);

            Drill drill2 = new Drill()
            {
                IdName = "faccord:drill_test_2",
                Name = "Test drill 2",
                Description = "Sample data for testing",
                Durability = 10000,
                Efficiency = 10,
                Hardness = 10,
                RangeX = 5,
                RangeY = 5,
            };
            await dataAccess.Drills.AddAsync(drill2);

            dataAccess.Complete();
        }
    }
}

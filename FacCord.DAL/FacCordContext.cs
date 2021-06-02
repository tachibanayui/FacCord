using IsekaiTechnologies.FacCord.Core.Models;
using IsekaiTechnologies.FacCord.Core.Models.Discord;
using IsekaiTechnologies.FacCord.Core.Models.Equipments;
using IsekaiTechnologies.FacCord.Core.Models.Machines;
using IsekaiTechnologies.FacCord.Core.Models.Mines;
using IsekaiTechnologies.FacCord.Core.Models.Shops;
using IsekaiTechnologies.FacCord.Core.Models.Storages;
using IsekaiTechnologies.FacCord.Core.Models.Storages.ItemMetas;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace IsekaiTechnologies.FacCord.Core.DAL
{
    public class FacCordContext : DbContext
    {
        public FacCordContext(DbContextOptions options) : base(options)
        {
        }

        public FacCordContext()
        {
        }

        public DbSet<Drill> Drills { get; set; }
        public DbSet<Prospector> Prospectors { get; set; }
        public DbSet<RuptureChemical> RuptureChemicals { get; set; }
        public DbSet<EnergyProductionMachineUnit> EnergyProductionMachineUnits { get; set; }
        public DbSet<MachineUnit> MachineUnits { get; set; }
        public DbSet<Machine> Machines { get; set; }
        public DbSet<ProcessingMachineUnit> ProcessingMachineUnits { get; set; }
        public DbSet<StorageMachine> StorageMachines { get; set; }
        public DbSet<Block> Blocks { get; set; }
        public DbSet<BlockSpawnChance> BlockSpawnChances { get; set; }
        public DbSet<Dimension> Dimensions { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<ExclusiveListings> ExclusiveListings { get; set; }
        public DbSet<Listing> Listings { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<StorageItem> StorageItems { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerSettings> PlayerSettings { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Quest> Quests { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Workshop> Workshops { get; set; }
        public DbSet<EmojiLookup> EmojiLookups { get; set; }
        public DbSet<ServerSettings> ServerSettings { get; set; }
        public DbSet<ItemMeta> ItemMetas { get; set; }
        public DbSet<DrillItemMeta> DrillItemMetas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-G5043LK\\SQLEXPRESS;Initial Catalog=TestFacCord;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
        }
    }
}

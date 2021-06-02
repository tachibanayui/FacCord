using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IsekaiTechnologies.FacCord.Core.DAL.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dimensions",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdName = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dimensions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmojiLookups",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TargetId = table.Column<long>(nullable: false),
                    Emoji = table.Column<string>(maxLength: 50, nullable: true),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmojiLookups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdName = table.Column<string>(maxLength: 100, nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    Durability = table.Column<long>(nullable: true),
                    RangeX = table.Column<short>(nullable: true),
                    RangeY = table.Column<short>(nullable: true),
                    Efficiency = table.Column<short>(nullable: true),
                    Hardness = table.Column<short>(nullable: true),
                    Prospector_RangeX = table.Column<short>(nullable: true),
                    Prospector_RangeY = table.Column<short>(nullable: true),
                    RegionLevel = table.Column<short>(nullable: true),
                    MineRichness = table.Column<short>(nullable: true),
                    OreVein = table.Column<short>(nullable: true),
                    Weight = table.Column<short>(nullable: true),
                    Width = table.Column<short>(nullable: true),
                    Height = table.Column<short>(nullable: true),
                    Capacity = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlayerSettings",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Width = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RuptureChemicals",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Count = table.Column<short>(nullable: false),
                    NextRefresh = table.Column<TimeSpan>(nullable: false),
                    Capacity = table.Column<short>(nullable: false),
                    RefreshInterval = table.Column<TimeSpan>(nullable: false),
                    LastUpdated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RuptureChemicals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Workshops",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SizeX = table.Column<int>(nullable: false),
                    SizeY = table.Column<int>(nullable: false),
                    WeightCapacity = table.Column<short>(nullable: false),
                    SupportedWeight = table.Column<short>(nullable: false),
                    StorageCapacity = table.Column<long>(nullable: false),
                    CurrentStorageItemCount = table.Column<long>(nullable: false),
                    EnergyCapacity = table.Column<double>(nullable: false),
                    CurrentEnergy = table.Column<double>(nullable: false),
                    LastUpdated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workshops", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Blocks",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<long>(nullable: true),
                    Hardness = table.Column<int>(nullable: false),
                    Toughness = table.Column<int>(nullable: false),
                    IsSolid = table.Column<bool>(nullable: false),
                    ReplacementId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Blocks_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Blocks_Blocks_ReplacementId",
                        column: x => x.ReplacementId,
                        principalTable: "Blocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SettingsId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Players_PlayerSettings_SettingsId",
                        column: x => x.SettingsId,
                        principalTable: "PlayerSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BlockSpawnChances",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BlockId = table.Column<long>(nullable: true),
                    Chance = table.Column<double>(nullable: false),
                    RegionId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlockSpawnChances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlockSpawnChances_Blocks_BlockId",
                        column: x => x.BlockId,
                        principalTable: "Blocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StorageItems",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<long>(nullable: true),
                    Count = table.Column<long>(nullable: false),
                    HasDurability = table.Column<bool>(nullable: false),
                    Durability = table.Column<long>(nullable: false),
                    BlockId = table.Column<long>(nullable: true),
                    ListingId = table.Column<long>(nullable: true),
                    ProcessingMachineUnitId = table.Column<long>(nullable: true),
                    ProcessingMachineUnitId1 = table.Column<long>(nullable: true),
                    QuestId = table.Column<long>(nullable: true),
                    QuestId1 = table.Column<long>(nullable: true),
                    RecipeId = table.Column<long>(nullable: true),
                    RecipeId1 = table.Column<long>(nullable: true),
                    WorkshopId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StorageItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StorageItems_Blocks_BlockId",
                        column: x => x.BlockId,
                        principalTable: "Blocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StorageItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StorageItems_Workshops_WorkshopId",
                        column: x => x.WorkshopId,
                        principalTable: "Workshops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdName = table.Column<string>(nullable: true),
                    RegionLevel = table.Column<short>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    DimensionId = table.Column<long>(nullable: true),
                    ProfileId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Regions_Dimensions_DimensionId",
                        column: x => x.DimensionId,
                        principalTable: "Dimensions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProfileName = table.Column<string>(nullable: true),
                    PlayerId = table.Column<long>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Fund = table.Column<long>(nullable: false),
                    Level = table.Column<short>(nullable: false),
                    Exp = table.Column<int>(nullable: false),
                    RuptureChemicalId = table.Column<long>(nullable: true),
                    CurrentProspectorId = table.Column<long>(nullable: true),
                    CurrentDrillId = table.Column<long>(nullable: true),
                    SelectedRegionId = table.Column<long>(nullable: true),
                    WorkshopId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Profiles_StorageItems_CurrentDrillId",
                        column: x => x.CurrentDrillId,
                        principalTable: "StorageItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Profiles_StorageItems_CurrentProspectorId",
                        column: x => x.CurrentProspectorId,
                        principalTable: "StorageItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Profiles_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Profiles_RuptureChemicals_RuptureChemicalId",
                        column: x => x.RuptureChemicalId,
                        principalTable: "RuptureChemicals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Profiles_Regions_SelectedRegionId",
                        column: x => x.SelectedRegionId,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Profiles_Workshops_WorkshopId",
                        column: x => x.WorkshopId,
                        principalTable: "Workshops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Listings",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Category = table.Column<string>(nullable: true),
                    Price = table.Column<long>(nullable: false),
                    MinimumLevel = table.Column<short>(nullable: false),
                    Limit = table.Column<int>(nullable: false),
                    LimitResetInterval = table.Column<TimeSpan>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    OfferEnds = table.Column<DateTime>(nullable: true),
                    ProfileId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Listings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Listings_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Quests",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    From = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Category = table.Column<string>(nullable: true),
                    RewardExp = table.Column<int>(nullable: false),
                    RewardCoin = table.Column<int>(nullable: false),
                    UnlockAfterId = table.Column<long>(nullable: true),
                    MininumLevel = table.Column<short>(nullable: false),
                    ProfileId = table.Column<long>(nullable: true),
                    ProfileId1 = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Quests_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Quests_Profiles_ProfileId1",
                        column: x => x.ProfileId1,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Quests_Quests_UnlockAfterId",
                        column: x => x.UnlockAfterId,
                        principalTable: "Quests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BuyerId = table.Column<long>(nullable: true),
                    PurchasedListingId = table.Column<long>(nullable: true),
                    Amount = table.Column<int>(nullable: false),
                    Cost = table.Column<long>(nullable: false),
                    PurchasedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Profiles_BuyerId",
                        column: x => x.BuyerId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transactions_Listings_PurchasedListingId",
                        column: x => x.PurchasedListingId,
                        principalTable: "Listings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipeName = table.Column<string>(nullable: true),
                    ProcessedById = table.Column<long>(nullable: true),
                    ProfileId = table.Column<long>(nullable: true),
                    QuestId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recipes_Items_ProcessedById",
                        column: x => x.ProcessedById,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Recipes_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Recipes_Quests_QuestId",
                        column: x => x.QuestId,
                        principalTable: "Quests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MachineUnits",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MachineId = table.Column<long>(nullable: true),
                    PosX = table.Column<short>(nullable: false),
                    PosY = table.Column<short>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    WorkshopId = table.Column<long>(nullable: true),
                    BufferSize = table.Column<long>(nullable: true),
                    ProcedureId = table.Column<long>(nullable: true),
                    Progress = table.Column<double>(nullable: true),
                    ProductionRate = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MachineUnits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MachineUnits_Items_MachineId",
                        column: x => x.MachineId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MachineUnits_Workshops_WorkshopId",
                        column: x => x.WorkshopId,
                        principalTable: "Workshops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MachineUnits_Recipes_ProcedureId",
                        column: x => x.ProcedureId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Blocks_ItemId",
                table: "Blocks",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Blocks_ReplacementId",
                table: "Blocks",
                column: "ReplacementId");

            migrationBuilder.CreateIndex(
                name: "IX_BlockSpawnChances_BlockId",
                table: "BlockSpawnChances",
                column: "BlockId");

            migrationBuilder.CreateIndex(
                name: "IX_BlockSpawnChances_RegionId",
                table: "BlockSpawnChances",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_ProfileId",
                table: "Listings",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_MachineUnits_MachineId",
                table: "MachineUnits",
                column: "MachineId");

            migrationBuilder.CreateIndex(
                name: "IX_MachineUnits_WorkshopId",
                table: "MachineUnits",
                column: "WorkshopId");

            migrationBuilder.CreateIndex(
                name: "IX_MachineUnits_ProcedureId",
                table: "MachineUnits",
                column: "ProcedureId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_SettingsId",
                table: "Players",
                column: "SettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_CurrentDrillId",
                table: "Profiles",
                column: "CurrentDrillId");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_CurrentProspectorId",
                table: "Profiles",
                column: "CurrentProspectorId");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_PlayerId",
                table: "Profiles",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_RuptureChemicalId",
                table: "Profiles",
                column: "RuptureChemicalId");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_SelectedRegionId",
                table: "Profiles",
                column: "SelectedRegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_WorkshopId",
                table: "Profiles",
                column: "WorkshopId");

            migrationBuilder.CreateIndex(
                name: "IX_Quests_ProfileId",
                table: "Quests",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Quests_ProfileId1",
                table: "Quests",
                column: "ProfileId1");

            migrationBuilder.CreateIndex(
                name: "IX_Quests_UnlockAfterId",
                table: "Quests",
                column: "UnlockAfterId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_ProcessedById",
                table: "Recipes",
                column: "ProcessedById");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_ProfileId",
                table: "Recipes",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_QuestId",
                table: "Recipes",
                column: "QuestId");

            migrationBuilder.CreateIndex(
                name: "IX_Regions_DimensionId",
                table: "Regions",
                column: "DimensionId");

            migrationBuilder.CreateIndex(
                name: "IX_Regions_ProfileId",
                table: "Regions",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_StorageItems_BlockId",
                table: "StorageItems",
                column: "BlockId");

            migrationBuilder.CreateIndex(
                name: "IX_StorageItems_ItemId",
                table: "StorageItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_StorageItems_ListingId",
                table: "StorageItems",
                column: "ListingId");

            migrationBuilder.CreateIndex(
                name: "IX_StorageItems_ProcessingMachineUnitId",
                table: "StorageItems",
                column: "ProcessingMachineUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_StorageItems_ProcessingMachineUnitId1",
                table: "StorageItems",
                column: "ProcessingMachineUnitId1");

            migrationBuilder.CreateIndex(
                name: "IX_StorageItems_QuestId",
                table: "StorageItems",
                column: "QuestId");

            migrationBuilder.CreateIndex(
                name: "IX_StorageItems_QuestId1",
                table: "StorageItems",
                column: "QuestId1");

            migrationBuilder.CreateIndex(
                name: "IX_StorageItems_RecipeId",
                table: "StorageItems",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_StorageItems_RecipeId1",
                table: "StorageItems",
                column: "RecipeId1");

            migrationBuilder.CreateIndex(
                name: "IX_StorageItems_WorkshopId",
                table: "StorageItems",
                column: "WorkshopId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_BuyerId",
                table: "Transactions",
                column: "BuyerId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_PurchasedListingId",
                table: "Transactions",
                column: "PurchasedListingId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlockSpawnChances_Regions_RegionId",
                table: "BlockSpawnChances",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StorageItems_Recipes_RecipeId",
                table: "StorageItems",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StorageItems_Recipes_RecipeId1",
                table: "StorageItems",
                column: "RecipeId1",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StorageItems_Quests_QuestId",
                table: "StorageItems",
                column: "QuestId",
                principalTable: "Quests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StorageItems_Quests_QuestId1",
                table: "StorageItems",
                column: "QuestId1",
                principalTable: "Quests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StorageItems_Listings_ListingId",
                table: "StorageItems",
                column: "ListingId",
                principalTable: "Listings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StorageItems_MachineUnits_ProcessingMachineUnitId",
                table: "StorageItems",
                column: "ProcessingMachineUnitId",
                principalTable: "MachineUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StorageItems_MachineUnits_ProcessingMachineUnitId1",
                table: "StorageItems",
                column: "ProcessingMachineUnitId1",
                principalTable: "MachineUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Regions_Profiles_ProfileId",
                table: "Regions",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blocks_Items_ItemId",
                table: "Blocks");

            migrationBuilder.DropForeignKey(
                name: "FK_MachineUnits_Items_MachineId",
                table: "MachineUnits");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_Items_ProcessedById",
                table: "Recipes");

            migrationBuilder.DropForeignKey(
                name: "FK_StorageItems_Items_ItemId",
                table: "StorageItems");

            migrationBuilder.DropForeignKey(
                name: "FK_StorageItems_Blocks_BlockId",
                table: "StorageItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Profiles_Regions_SelectedRegionId",
                table: "Profiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Listings_Profiles_ProfileId",
                table: "Listings");

            migrationBuilder.DropForeignKey(
                name: "FK_Quests_Profiles_ProfileId",
                table: "Quests");

            migrationBuilder.DropForeignKey(
                name: "FK_Quests_Profiles_ProfileId1",
                table: "Quests");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_Profiles_ProfileId",
                table: "Recipes");

            migrationBuilder.DropTable(
                name: "BlockSpawnChances");

            migrationBuilder.DropTable(
                name: "EmojiLookups");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Blocks");

            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.DropTable(
                name: "Dimensions");

            migrationBuilder.DropTable(
                name: "Profiles");

            migrationBuilder.DropTable(
                name: "StorageItems");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "RuptureChemicals");

            migrationBuilder.DropTable(
                name: "Listings");

            migrationBuilder.DropTable(
                name: "MachineUnits");

            migrationBuilder.DropTable(
                name: "PlayerSettings");

            migrationBuilder.DropTable(
                name: "Workshops");

            migrationBuilder.DropTable(
                name: "Recipes");

            migrationBuilder.DropTable(
                name: "Quests");
        }
    }
}

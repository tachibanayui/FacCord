using IsekaiTechnologies.FacCord.Core.DAL;
using IsekaiTechnologies.FacCord.Core.EquipmentLogic;
using IsekaiTechnologies.FacCord.Core.Mine;
using IsekaiTechnologies.FacCord.Core.MineGeneration;
using IsekaiTechnologies.FacCord.Core.Models;
using IsekaiTechnologies.FacCord.Core.Models.Equipments;
using IsekaiTechnologies.FacCord.Core.Models.Mines;
using IsekaiTechnologies.FacCord.Core.Models.Storages;
using IsekaiTechnologies.FacCord.Core.Models.Storages.ItemMetas;
using IsekaiTechnologies.FacCord.Core.StorageManagements;
using IsekaiTechnologies.FacCord.Core.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace IsekaiTechnologies.FacCord.Core.Controllers
{
    public class MineController2 : PlayerController, IMineController
    {
        public IMineConfig MineConfig { get; protected set; }
        public IEquipmentConfig EquipmentConfig { get; protected set; }
        protected IStorageManagement _Storage;
        protected Profile _ActiveProfile;

        public MineController2(PlayerContext ctx) : base(ctx)
        {
            MineConfig = ctx.Manager.Settings.Configurations.MineConfiguration;
            EquipmentConfig = ctx.Manager.Settings.Configurations.EquipmentConfiguraton;
        }

        public async Task<AbstractMine> FindMineAsync()
        {
            Context.MiningContext = null;

            // Verify player mine region and dimension
            _ = (await GetActiveProfile()).SelectedRegion ??
                throw new InvalidOperationException("Player have not chose any region");

            Context.LastDiscoveredMine = await new MineBuilder(DataAccess, MineConfig)
                .WithPropector((await GetActiveProfile()).CurrentProspector.Item as Prospector)
                .InRegion((await GetActiveProfile()).SelectedRegion)
                .BuildAsync();

            return Context.LastDiscoveredMine;
        }

        public async Task GoToMine(AbstractMine mine)
        {
            // Data validation
            if (mine == null || mine != Context.LastDiscoveredMine)
                throw new ArgumentException("Invalid mine provided!", nameof(mine));

            if (mine.IsGenerated)
                throw new InvalidOperationException("This mine already been mined!");

            Context.MiningContext = new MiningContext();
            Context.MiningContext.Position = new Position2DInt((int)Math.Ceiling(mine.Width / 2d) + 1, (int)Math.Ceiling(mine.Height / 2d) + 1);
            await mine.GenerateMap();
        }

        public async Task<List<StorageItem>> UseRuptureChemical()
        {
            var rc = (await GetActiveProfile()).RuptureChemical;
            GameManager.UpdateRuptureChemicalStatus(rc);
            List<StorageItem> result = new List<StorageItem>();

            if (rc.Count > 0)
            {
                if (rc.Count == rc.Capacity)
                    rc.NextRefresh = rc.RefreshInterval;

                rc.Count--;

                for (int h = 0; h < Context.LastDiscoveredMine.Height; h++)
                {
                    for (int w = 0; w < Context.LastDiscoveredMine.Width; w++)
                    {
                        if (await Context.LastDiscoveredMine.IsValidPositionAsync(w, h) && Context.LastDiscoveredMine[w, h].IsUnbreakable)
                        {
                            var loot = Context.LastDiscoveredMine[w, h].Break((await GetActiveProfile()).CurrentDrill.Item as Drill);
                            result.AddRange(loot);
                            Context.LastDiscoveredMine[w, h] = Context.LastDiscoveredMine[w, h].Replacement;
                        }
                    }
                }
            }
            else
            {
                throw new InvalidOperationException("Profile don't have any rupture chemical");
            }

            return result;
        }

        public Task MoveAsync(Direction dir)
        {
            Position2DInt desiredPos = Context.MiningContext.Position.Offset(dir, 1);
            desiredPos.X = MathExtension.Clamp(0, Context.LastDiscoveredMine.Width, desiredPos.X);
            desiredPos.Y = MathExtension.Clamp(0, Context.LastDiscoveredMine.Height, desiredPos.Y);

            if (!Context.LastDiscoveredMine[desiredPos].IsSolid)
            {
                Context.MiningContext.Position = desiredPos;
            }

            Context.MiningContext.Facing = dir;
            return Task.CompletedTask;
        }

        public async Task<List<StorageItem>> MineBlockAsync(Direction dir)
        {
            var drillData = (await GetActiveProfile()).CurrentDrill.Item as Drill;
            var currentDrill = (await GetActiveProfile()).CurrentDrill;
            Position2DInt hitBlockPos = Context.MiningContext.Position.Offset(dir, 1);
            // Validate targeted block position
            if (!await Context.LastDiscoveredMine.IsValidPosition(hitBlockPos).ConfigureAwait(false))
                return new List<StorageItem>();

            Position2DInt lastHitBlPos = Context.MiningContext.LastHitBlockPosition;
            if (lastHitBlPos != null && hitBlockPos == lastHitBlPos)
            {
                if (drillData != null)
                    Context.MiningContext.LastHitBlockCount++;
            }
            else
            {
                await ChangeLastHitBlock(hitBlockPos);
            }

            if (Context.MiningContext.LastHitBlockCount >= Context.MiningContext.LastHitBlockBreakCount)
            {
                // Block broken!

                // We update the lastHisPos with the new instance in our context in case player 1 hit mine this block
                lastHitBlPos = Context.MiningContext.LastHitBlockPosition;

                Rect2DInt mineBound = null;
                int midPtY = drillData.RangeY / 2;
                int midPtX = drillData.RangeX / 2;
                switch (Context.MiningContext.Facing)
                {
                    case Direction.North:
                        mineBound = new Rect2DInt(
                            Context.MiningContext.Position.Offset(-drillData.RangeX / 2, -drillData.RangeY),
                            drillData.RangeX, drillData.RangeY);
                        break;
                    case Direction.East:
                        mineBound = new Rect2DInt(
                            Context.MiningContext.Position.Offset(1, -drillData.RangeY / 2),
                            drillData.RangeX, drillData.RangeY);
                        break;
                    case Direction.South:
                        mineBound = new Rect2DInt(
                            Context.MiningContext.Position.Offset(-drillData.RangeX / 2, 1),
                            drillData.RangeX, drillData.RangeY);
                        break;
                    case Direction.West:
                        mineBound = new Rect2DInt(
                            Context.MiningContext.Position.Offset(-drillData.RangeX, -drillData.RangeY / 2),
                            drillData.RangeX, drillData.RangeY);
                        break;
                }

                if (currentDrill.ItemMeta is IDamageable dim)
                {
                    if (await dim.GetDurabilityAsync() <= 0) return new List<StorageItem>();

                    await dim.DamageAsync(1);
                }
                List<StorageItem> droppedItem = new List<StorageItem>();

                // Iterate blocks about to be broken to verify that they are breakable
                for (int h = 0; h < mineBound.Height; h++)
                {
                    for (int w = 0; w < mineBound.Width; w++)
                    {
                        if (await Context.LastDiscoveredMine.IsValidPosition(mineBound.Position.Offset(w, h)))
                        {
                            var blockData = Context.LastDiscoveredMine[mineBound.Position.Offset(w, h)];
                            if (!blockData.IsUnbreakable && blockData.Hardness <= drillData.Hardness)
                            {
                                var loot = blockData.Break(drillData);
                                droppedItem.AddRange(loot);
                                Context.LastDiscoveredMine[mineBound.Position.Offset(w, h)] = blockData.Replacement;
                            }
                        }
                    }
                }

                DataAccess.Complete();
                return droppedItem;
            }

            return new List<StorageItem>();
        }

        protected async Task<Profile> GetActiveProfile()
        {
            if (_ActiveProfile == null)
            {
                _ActiveProfile = await DataAccess.Profiles.GetActiveProfileForMineController(Context.CurrentPlayer);
            }

            return _ActiveProfile;
        }

        protected async Task<IStorageManagement> GetStorageManagementAsync()
        {
            if (_Storage == null )
            {
                _Storage = await Context.Manager.Settings.Configurations.StoragManagementProvider.GetStorageManagement(DataAccess, await GetActiveProfile());
            }

            return _Storage;
        }

        private async Task ChangeLastHitBlock(Position2DInt hitBlockPos)
        {
            Block hitBlock = Context.LastDiscoveredMine[hitBlockPos];
            Context.MiningContext.LastHitBlockPosition = hitBlockPos;

            // Harness: The minimum hardness level to mine
            Drill drill = (await GetActiveProfile()).CurrentDrill.Item as Drill;

            if (drill.Hardness >= hitBlock.Hardness && !hitBlock.IsUnbreakable)
            {
                Context.MiningContext.LastHitBlockCount = 1;
            }
            else
            {
                // Unbreakable
                Context.MiningContext.LastHitBlockBreakCount = -1;
                return;
            }

            // Toughness: How long it takes to mine
            Context.MiningContext.LastHitBlockBreakCount = EquipmentConfig.GetBreakHitCount(drill, hitBlock.Toughness);
        }
    }
}

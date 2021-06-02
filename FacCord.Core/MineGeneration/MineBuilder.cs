using IsekaiTechnologies.FacCord.Core.DAL;
using IsekaiTechnologies.FacCord.Core.Mine;
using IsekaiTechnologies.FacCord.Core.Models.Equipments;
using IsekaiTechnologies.FacCord.Core.Models.Mines;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace IsekaiTechnologies.FacCord.Core.MineGeneration
{
    public class MineBuilder
    {
        public Dimension Dimension { get; set; }
        public Region Region { get; set; }
        public Prospector Prospector { get; set; }

        private IUnitOfWork _DataAccess;
        private IMineConfig _MineConfig;

        public MineBuilder(IUnitOfWork dataAccess, IMineConfig dist)
        {
            _DataAccess = dataAccess;
            _MineConfig = dist;
        }

        public async Task<AbstractMine> BuildAsync()
        {
            // If dimension, region, or prospector are not specified we will create a void mine
            if (Region == null || Prospector == null)
            {
                return await VoidMine.VoidMap(Prospector, await _DataAccess.Blocks.GetAsync(0)).ConfigureAwait(false);
            }
            else
            {
                // Calculate the final probability
                var finalDisturbution = await _MineConfig.GetMaterialDistributionAsync(this);

                // 9 blocks will be air for the player to stand
                int avaiableTileCount = Prospector.RangeX * Prospector.RangeY - 9;
                int filledTile = 0;
                Dictionary<Block, int> matCount = new Dictionary<Block, int>();

                foreach (var item in finalDisturbution)
                {
                    var count = (int) Math.Floor(item.Value * avaiableTileCount);
                    filledTile += count;   
                    matCount.Add(item.Key, count);
                }

                // Remainder will be added to the last entry
                matCount[matCount.Last().Key] = matCount.Last().Value + avaiableTileCount - filledTile;

                AbstractMine mine = new Mine.Mine(
                    Prospector,
                    await _MineConfig.GetOreVeinAsync(this),
                    await _DataAccess.Blocks.GetBlockByItemIdNameAsync("faccord:air")
                    );
                mine.MaterialCounts = matCount;

                return mine;
            }
        }

        public MineBuilder InRegion(Region region)
        {
            Region = region;
            return this;
        }

        public MineBuilder WithPropector(Prospector prospector)
        {
            Prospector = prospector;
            return this;
        }
    }
}

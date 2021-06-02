using IsekaiTechnologies.FacCord.Core.Models.Equipments;
using IsekaiTechnologies.FacCord.Core.Models.Mines;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IsekaiTechnologies.FacCord.Core.Mine
{
    public class VoidMine : AbstractMine
    {
        protected Block _BlockToFill;

        public VoidMine(Prospector prospector, int oreVein, Block blockToFill) : base(prospector, oreVein) 
        {
            _BlockToFill = blockToFill;
            MaterialCounts = new Dictionary<Block, int>();
            MaterialCounts.Add(_BlockToFill, Width * Height);
        }

        /// <summary>
        /// This Factory method use to create Mine with null params MineBuilder
        /// </summary>
        /// <returns></returns>
        public static Task<VoidMine> VoidMap(Prospector prospector, Block blockToFill)
        {
            return Task.FromResult(new VoidMine(prospector, 0, blockToFill));
        }

        public override Task GenerateMap() => Task.CompletedTask;

        protected override Block GetBlockAt(int x, int y)
        {
            return _BlockToFill;
        }

        protected override void SetBlockAt(int x, int y, Block id) { }
    }
}

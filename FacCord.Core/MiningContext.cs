using IsekaiTechnologies.FacCord.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace IsekaiTechnologies.FacCord.Core
{
    /// <summary>
    /// Store information about mining
    /// </summary>
    public class MiningContext
    {
        public Position2DInt Position { get; set; }
        public Direction Facing { get; set; }

        public Position2DInt LastHitBlockPosition { get; set; }
        public int LastHitBlockCount { get; set; }
        public int LastHitBlockBreakCount { get; set; }
        public int LastHitBlockId { get; set; }
    }
}

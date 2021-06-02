using IsekaiTechnologies.FacCord.Core.Models;
using IsekaiTechnologies.FacCord.Core.Models.Equipments;
using IsekaiTechnologies.FacCord.Core.Models.Mines;
using IsekaiTechnologies.FacCord.Core.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IsekaiTechnologies.FacCord.Core.Mine
{
    public abstract class AbstractMine
    {
        public Dictionary<Block, int> MaterialCounts { get; set; }
        public Rect2DInt SpawnArea { get; set; }
        public bool IsGenerated { get; protected set; }
        public int OreVein { get; private set; }
        public int Width => Prospector.RangeX;
        public int Height => Prospector.RangeY;

        protected Prospector Prospector;

        public AbstractMine(Prospector prospector, int oreVein)
        {
            Prospector = prospector;
            OreVein = oreVein;
            // In this case is it expected that division result might be floored
            SpawnArea = new Rect2DInt(Width / 2, Height / 2, 3, 3);
        }

        public Task<bool> IsValidPosition(Position2DInt pos)
            => Task.FromResult(new Rect2DInt(0, 0, Width, Height).Contain(pos));
        public async Task<bool> IsValidPositionAsync(int x, int y) => await IsValidPosition(new Position2DInt(x, y)).ConfigureAwait(false);
            

        public Block this[Position2DInt pos]
        {
            get => this[pos.X, pos.Y];
            set => this[pos.X, pos.Y] = value;
        }

        public Block this[int x, int y]
        {
            get 
            {
                if (!IsGenerated)
                    throw new InvalidOperationException("Map is not generated!");

                return GetBlockAt(x, y);
            }
            set
            {
                if (!IsGenerated)
                    throw new InvalidOperationException("Map is not generated!");

                SetBlockAt(x, y, value);
            }
        }

        public abstract Task GenerateMap();
        protected abstract Block GetBlockAt(int x, int y);
        protected abstract void SetBlockAt(int x, int y, Block id);

        // TODO: We might need a event to notify the map has been changed
    }
}
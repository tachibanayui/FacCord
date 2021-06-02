using IsekaiTechnologies.FacCord.Core.Models;
using IsekaiTechnologies.FacCord.Core.Models.Equipments;
using IsekaiTechnologies.FacCord.Core.Models.Mines;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace IsekaiTechnologies.FacCord.Core.Mine
{
    /// <summary>
    /// Standrand mine
    /// </summary>
    public class Mine : AbstractMine
    {
        const int SpreadAddition = 5;
        
        private Block[,] _Map;
        private bool _SetSeed;
        private Block _SpawnFiller;

        public int MineSeed { get; private set; }

        public Mine(Prospector prospector, int oreVein, Block fillSpawnArea) : base(prospector, oreVein) 
        {
            _SpawnFiller = fillSpawnArea;   
        }
        public Mine(Prospector prospector, int oreVein, Block fillSpawnArea, int seed) : this(prospector, oreVein, fillSpawnArea) 
        {
            MineSeed = seed;
            _SetSeed = true;
        }

        public override async Task GenerateMap()
        {
            await Task.Run(async () =>
            {
                Random rand = _SetSeed ? new Random(MineSeed) : new Random();
                _Map = new Block[Width, Height];

                // Generate material map
                foreach (var item in MaterialCounts)
                {
                    int matLeft = item.Value;
                    while (matLeft > 0)
                    {
                        // The nuclearation site
                        Position2DInt pos = new Position2DInt(rand.Next(0, Width), rand.Next(0, Height));
                        if (GetBlockAt(pos.X, pos.Y) != null || SpawnArea.Contain(pos))
                            continue;

                        SetBlockAt(pos.X, pos.Y, item.Key);
                        matLeft--;

                        // Spread
                        int spreadCount = OreVein + rand.Next(0, SpreadAddition);
                        Position2DInt lastSP = pos;
                        while (spreadCount > 0)
                        {
                            // check if we have any vacant coordinate. We will break out of the loop if none were found
                            if (!await CheckSpreadAvailability(lastSP.Offset(Direction.North, 1)) &&
                                !await CheckSpreadAvailability(lastSP.Offset(Direction.East, 1)) &&
                                !await CheckSpreadAvailability(lastSP.Offset(Direction.South, 1)) &&
                                !await CheckSpreadAvailability(lastSP.Offset(Direction.West, 1)))
                                break;

                            Position2DInt spreadPos = lastSP.Offset((Direction)rand.Next(0, 4), 1);
                            if (await CheckSpreadAvailability(spreadPos))
                            {
                                if (matLeft <= 0)
                                    break;
                                SetBlockAt(spreadPos.X, spreadPos.Y, item.Key);
                                lastSP = spreadPos;
                                spreadCount--;
                                matLeft--;
                            }
                        }
                    }
                }

                // Fill spawn area
                for (int h = 0; h < SpawnArea.Height; h++)
                {
                    for (int w = 0; w < SpawnArea.Width; w++)
                    {
                        SetBlockAt(SpawnArea.X + w, SpawnArea.Y + h, _SpawnFiller);
                    }
                }

                IsGenerated = true;
            });
        }

        public string DebugVisualStudio()
        {
            StringBuilder bd = new StringBuilder();
            for (int h = 0; h < Height; h++)
            {
                for (int w = 0; w < Width; w++)
                {
                    if (_Map[h,w] == null)
                    {
                        bd.Append(' ');
                    }
                    else
                    {
                        bd.Append(_Map[h, w]);
                    }
                }
                bd.AppendLine();
            }

            return bd.ToString();
        }

        private async Task<bool> CheckSpreadAvailability(Position2DInt pos)
            => (await IsValidPosition(pos)) && !SpawnArea.Contain(pos) && GetBlockAt(pos.X, pos.Y) == null;

        protected override Block GetBlockAt(int x, int y)
        {
            return _Map[y, x];
        }

        protected override void SetBlockAt(int x, int y, Block id)
        {
            _Map[y, x] = id;
        }
    }
}

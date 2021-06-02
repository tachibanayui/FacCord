using IsekaiTechnologies.FacCord.Core.Models.Mines;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IsekaiTechnologies.FacCord.Core.MineGeneration
{
    public class MineConfig : IMineConfig
    {
        // Prevent div 0, also adjust the magnitude
        const int YShiftQuadratic = 3;
        const double DisturbulanceWeight = 0.1;

        const double OreVeinDenominator = 9;
        const int OreVeinBase = 1;

        private Random _Rand = new Random();

        public async Task<Dictionary<Block, double>> GetMaterialDistributionAsync(MineBuilder builder)
        {
            return await Task.Run(() =>
            {
                var materialEntryCount = builder.Region.MaterialDistribution.Count;

                Dictionary<Block, double> finalDist = new Dictionary<Block, double>();

                double remainder = 1;
                double lastDebt = 0;
                foreach (var item in builder.Region.MaterialDistribution)
                {
                    // The probability after calculating prospector level 
                    // FacCord Spreadsheet has example, plot and famula for calcualting these
                    var ppLeveling = item.Chance + (1 - item.Chance) / (Math.Pow(builder.Prospector.MineRichness - 11, 2) + YShiftQuadratic);

                    remainder -= ppLeveling;

                    var noise = (_Rand.NextDouble() * (remainder - lastDebt + ppLeveling) - ppLeveling);
                    var finalPropability = ppLeveling + noise * DisturbulanceWeight;
                    finalDist.Add(item.Block, finalPropability);

                    lastDebt += noise;
                }

                return finalDist;
            });
        }

        public Task<int> GetOreVeinAsync(MineBuilder builder)
            => Task.FromResult((int) Math.Round(1 / OreVeinDenominator * Math.Pow(builder.Prospector.OreVein, 2) + OreVeinBase));
    }
}

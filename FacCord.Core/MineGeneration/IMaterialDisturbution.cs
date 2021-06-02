using IsekaiTechnologies.FacCord.Core.Models.Equipments;
using IsekaiTechnologies.FacCord.Core.Models.Mines;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IsekaiTechnologies.FacCord.Core.MineGeneration
{
    /// <summary>
    /// Use to calculate the probability to get certain materials 
    /// </summary>
    public interface IMineConfig
    {
        Task<Dictionary<Block, double>> GetMaterialDistributionAsync(MineBuilder builder);
        Task<int> GetOreVeinAsync(MineBuilder builder);
    }
}

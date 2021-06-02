using IsekaiTechnologies.FacCord.Core.Mine;
using IsekaiTechnologies.FacCord.Core.Models;
using IsekaiTechnologies.FacCord.Core.Models.Storages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IsekaiTechnologies.FacCord.Core.Controllers
{
    public interface IMineController
    {
        Task<AbstractMine> FindMineAsync();
        Task GoToMine(AbstractMine mine);
        Task<List<StorageItem>> MineBlockAsync(Direction dir);
        Task MoveAsync(Direction dir);
        Task<List<StorageItem>> UseRuptureChemical();
    }
}
// Generated by struct?.GetType() simple repo parttern generator!
using IsekaiTechnologies.FacCord.Core.Models;
using System.Threading.Tasks;

namespace IsekaiTechnologies.FacCord.Core.DAL.Repositories
{
    public interface IProfileRepository : IRepository<Profile>
    {
        Task<Profile> GetActiveProfileAsync(Player player);
        Task<Profile> GetActiveProfileForTitleAsync(Player player);
        Task<Profile> GetActiveProfileForMineDiscoveryAsync(Player player);
        Task<Profile> GetActiveProfileForRegionSelectionAsync(Player currentPlayer);
        Task<Profile> GetActiveProfileForMineController(Player currentPlayer);
    }
}

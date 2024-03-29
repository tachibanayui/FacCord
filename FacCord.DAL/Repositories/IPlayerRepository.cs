// Generated by struct?.GetType() simple repo parttern generator!
using IsekaiTechnologies.FacCord.Core.Models;
using System.Threading.Tasks;

namespace IsekaiTechnologies.FacCord.Core.DAL.Repositories
{
    public interface IPlayerRepository : IRepository<Player>
    {
        Task<Player> GetPlayerByDiscordId(long discordId);
    }
}

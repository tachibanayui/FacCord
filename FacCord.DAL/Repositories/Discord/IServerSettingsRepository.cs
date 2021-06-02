using IsekaiTechnologies.FacCord.Core.Models.Discord;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IsekaiTechnologies.FacCord.Core.DAL.Repositories.Discord
{
    public interface IServerSettingsRepository : IRepository<ServerSettings>
    {
        Task<ServerSettings> GetServerSettingsByDiscordId(long discordId);
        Task<bool> ExistByDiscordId(long id);
    }
}

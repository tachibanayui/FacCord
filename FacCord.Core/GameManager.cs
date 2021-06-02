using IsekaiTechnologies.FacCord.Core.Controllers;
using IsekaiTechnologies.FacCord.Core.Models;
using IsekaiTechnologies.FacCord.Core.Models.Equipments;
using IsekaiTechnologies.FacCord.Core.StorageManagements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsekaiTechnologies.FacCord.Core
{
    public class GameManager
    {
        protected List<PlayerContext> _PlayerContexts = new List<PlayerContext>();

        public GameSettings Settings { get; protected set; }

        public GameManager(GameSettings settings)
        {
            Settings = settings;
        }

        public async Task<PlayerContext> GetPlayerContext(Player player)
        {
            var ctx = _PlayerContexts.FirstOrDefault(x => player.Id == x.CurrentPlayer.Id);
            if (ctx == null)
            {
                ctx = new PlayerContext(player);
                ctx.Manager = this;
                ctx.MineController = await Settings.Configurations.ControllerProvider.GetMineController(this, ctx).ConfigureAwait(false);
                _PlayerContexts.Add(ctx);
            }

            return ctx;
        }

        public async Task<Profile> CreateNewProfileAsync(Player player, string name = "Unnamed Profile")
        {
            return await Settings.Configurations.ProfileProvider.CreateNewProfile(player, name);
        }

        public static void UpdateRuptureChemicalStatus(RuptureChemical rc)
        {
            var timeDelta = DateTime.Now - rc.LastUpdated;
            rc.Count = (short)Math.Clamp(0, rc.Capacity, rc.Count + Math.Floor((timeDelta + rc.NextRefresh) / rc.RefreshInterval));
            rc.NextRefresh = rc.Capacity == rc.Count ? TimeSpan.Zero : TimeSpan.FromMilliseconds((timeDelta.TotalMilliseconds + rc.NextRefresh.TotalMilliseconds) % rc.RefreshInterval.TotalMilliseconds);
        }

        public async Task UpdateLevelStatus(Profile profile)
        {
            var expToLevelUp = await GetExpToLevelUp(profile.Level).ConfigureAwait(false);
            var delta = profile.Exp - expToLevelUp;
            if (delta > 0)
            {
                profile.Level++;
                profile.Exp = delta;
            }
        }

        public async Task<int> GetExpToLevelUp(int level)
        {
            return await Settings.Configurations.ExpToLevelConfiguration.GetExperienceForLevel(level).ConfigureAwait(false);
        }
    }
}

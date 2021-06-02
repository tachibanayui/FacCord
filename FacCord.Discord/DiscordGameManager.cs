using Discord;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
using IsekaiTechnologies.FacCord.Core;
using IsekaiTechnologies.FacCord.Core.DAL;
using IsekaiTechnologies.FacCord.Core.Models;
using IsekaiTechnologies.FacCord.Core.Models.Discord;
using IsekaiTechnologies.FacCord.Core.Models.Equipments;
using IsekaiTechnologies.FacCord.Core.Models.Machines;
using IsekaiTechnologies.FacCord.Core.Models.Mines;
using IsekaiTechnologies.FacCord.Core.Models.Storages;
using IsekaiTechnologies.FacCord.Discord.Pages;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IsekaiTechnologies.FacCord.Discord
{
    public class DiscordGameManager : GameManager
    {
        public DiscordSocketClient Client { get; set; }
        public IServiceProvider Services { get; set; }

        public List<PageManager> UIs = new List<PageManager>();

        public DiscordGameManager(DiscordSocketClient client, IServiceProvider sp) : base(sp.GetRequiredService<GameSettings>())
        {
            Client = client;
            Services = sp;
        }

        public async Task<Player> CreateOrResetAccount(IUser user) 
        {
            var dal = Services.GetRequiredService<IUnitOfWork>();
            var existUser = await dal.Players.FindAsync(p => p.Id == (long)user.Id).FirstOrDefaultAsync();
            if (existUser != null)
                await dal.Players.RemoveAsync(existUser);
            // Remove player data here

            Player p = new Player();
            p.DiscordId = (long)user.Id;
            p.Profiles = new List<Profile>();
            var profile = await CreateNewProfileAsync(p);
            profile.IsActive = true;
            p.Profiles.Add(profile);

            await dal.Players.AddAsync(p);
            
            dal.Complete();
            return p;
        }

        public async Task StartAsync()
        {
            Client.MessageReceived += Client_MessageReceived;
        }       

        private async Task Client_MessageReceived(SocketMessage arg)
        {
            if(arg is SocketUserMessage msg && !arg.Author.IsBot)
            {
                var dal = Services.GetRequiredService<IUnitOfWork>();
                var player = await dal.Players.GetPlayerByDiscordId((long)arg.Author.Id)
                    ?? await CreateOrResetAccount(arg.Author);

                PageManager pm = GetPageManager(arg.Author);
                if (!pm.StartedUp)
                    _ = pm.OnStartup(arg);
            }
        }

        public PageManager GetPageManager(IUser user)
        {
            var ui = UIs.FirstOrDefault(x => x.User.Id == user.Id);
            if (ui == null)
            {
                ui = new PageManager() { Client = Client, GameManager = this, Services = Services, User = user };
                UIs.Add(ui);
            }

            return ui;
        }

        public bool ValidateGameCommand(string msg, string serverPrefix, out int argPos)
        {
            var dal = Services.GetRequiredService<IUnitOfWork>();
            if( msg.Trim().StartsWith(serverPrefix))
            {
                argPos = msg.IndexOf(serverPrefix);
                return true;
            }
            else if (msg.Trim().StartsWith(Client.CurrentUser.Mention))
            {
                argPos = msg.IndexOf(Client.CurrentUser.Mention) + Client.CurrentUser.Mention.Length;
                return true;
            }
            else
            {
                argPos = -1;
                return false;
            }

        }

        public async Task<ServerSettings> GetServerSettings(long id)
        {
            var dal = Services.GetRequiredService<IUnitOfWork>();
            if(await dal.ServerSettings.ExistByDiscordId(id))
            {
                return await dal.ServerSettings.GetServerSettingsByDiscordId(id);
            }
            else
            {
                var newSettings = new ServerSettings()
                {
                    DiscordServerId = id,
                    ServerPrefix = "!"
                };

                await dal.ServerSettings.AddAsync(newSettings);
                return newSettings;
            }
        }
    }
}

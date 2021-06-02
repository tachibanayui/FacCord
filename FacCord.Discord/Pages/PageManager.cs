using Discord;
using Discord.WebSocket;
using IsekaiTechnologies.FacCord.Core.DAL;
using IsekaiTechnologies.FacCord.Core.Models;
using IsekaiTechnologies.FacCord.Core.Providers;
using IsekaiTechnologies.FacCord.Core.StorageManagements;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsekaiTechnologies.FacCord.Discord.Pages
{
    public class PageManager
    {
        private const string CategoryName = "Faccord Temp Channels";

        public Page ActivePage { get; set; }
        public DiscordSocketClient Client { get; set; }
        public IUser User { get; set; }
        public IServiceProvider Services { get; set; }
        public DiscordGameManager GameManager { get; set; }
        public bool StartedUp { get; set; }
        public ITextChannel DrawChannel { get; set; }

        public async Task<IStorageManagement> CreateStorageManagement(Profile profile)
        {
            var dal = Services.GetRequiredService<IUnitOfWork>();
            return await Services.GetRequiredService<StorageManagementProvider>().GetStorageManagement(dal, profile);
        }

        public async Task CreateDrawChannel(IGuild guild)
        {
            var categories = await guild.GetCategoriesAsync();
            var faccordCategory = (categories).FirstOrDefault(x => x.Name == CategoryName);
            if (faccordCategory == null)
            {

                faccordCategory = await guild.CreateCategoryAsync(CategoryName, x => x.Position = categories.Count);
            }

            DrawChannel = await guild.CreateTextChannelAsync($"{User.Username}-channel", x => x.CategoryId = faccordCategory.Id);
        }

        public async Task Navigate (Page page, IMessageChannel channel, object args)
        {
            if (ActivePage != null)
            {
                await ActivePage.OnNavigatedFrom(page, this, args);
            }

            await page.OnNavigatedTo(ActivePage, this, args);
            ActivePage = page;
            await page.LoadView(channel);
        }

        public virtual async Task OnStartup(object args)
        {
            if (args is SocketMessage sm)
            {
                await Navigate(new TitlePage(), sm.Channel, args);
            }
            StartedUp = true;
        }
    }
}

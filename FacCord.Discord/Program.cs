using Discord;
using Discord.Webhook;
using Discord.WebSocket;
using IsekaiTechnologies.FacCord.Core;
using IsekaiTechnologies.FacCord.Core.DAL;
using IsekaiTechnologies.FacCord.Core.DAL.EntityFramework;
using IsekaiTechnologies.FacCord.Core.EquipmentLogic;
using IsekaiTechnologies.FacCord.Core.MineGeneration;
using IsekaiTechnologies.FacCord.Core.Providers;
using IsekaiTechnologies.FacCord.Discord.Factories;
using IsekaiTechnologies.FacCord.Discord.Utils;
using IsekaiTechnologies.ImagiTextedDiscordGUI.Renderer;
using IsekaiTechnologies.ImagiTextedDiscordGUI.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace IsekaiTechnologies.FacCord.Discord
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        private static async Task MainAsync(string[] args)
        {
            var sp = ConfigureServices();

            SortedList<Position2DInt, string> a = new SortedList<Position2DInt, string>(new Sorter());
            a.Add(new Position2DInt(1, 231), null);
            a.Add(new Position2DInt(1, 2321), null);

            //var dal = sp.GetRequiredService<IUnitOfWork>();
            var directDal = sp.GetRequiredService<FacCordContext>();
            var b = directDal.Regions.Include(x => x.MaterialDistribution).ThenInclude(x=>x.Block).ThenInclude(x => x.Item).ToList();
            //var a = directDal.Profiles
            //    .Include(x => x.Workshop).ThenInclude(x => x.Storage)
            //    .First(x => x.Id == 1);
            //Random r = new Random();

            //foreach (var item in await Queryable.Where( directDal.Items, x => x.Name.Contains("Block")).Take(45).ToListAsync()   )
            //{
            //    a.Workshop.Storage.Add(new Core.Models.Storages.StorageItem() { Count = r.Next(1, 10), Item = item });
            //}
            //directDal.SaveChanges();
            var client = sp.GetRequiredService<DiscordSocketClient>();
            await client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("BOT_TOKEN"));
            await client.StartAsync();
            await sp.GetRequiredService<DiscordGameManager>().StartAsync();

            await Task.Delay(-1);
        }

        private static IServiceProvider ConfigureServices()
        {
            var sc = new ServiceCollection()
                .AddScoped<FacCordContext>()
                .AddScoped<IUnitOfWork, UnitOfWork>()
                .AddSingleton<IUIEmojiLookupTable, UIEmojiLookupTable>()
                .AddSingleton<IDrawingContextFactory, DrawingContextFactory>()
                .AddScoped<StorageManagementProvider>()
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<DiscordGameManager>()
                .AddSingleton<IStorageQuerier, StorageQuerier>()
                .AddSingleton(x =>
                {
                    var dal = x.GetRequiredService<IUnitOfWork>();
                    return new GameSettings()
                    {
                        DataAccess = dal,
                        Configurations = GameConfigs.GetDefaultConfiguration(dal)
                    };
                });
                
            
            return sc.BuildServiceProvider();
        }

        class Sorter : IComparer<Position2DInt>
        {
            public int Compare(Position2DInt x, Position2DInt y)
            {
                var yCompare = x.Y - y.Y;
                return yCompare == 0 ? x.X - y.X : yCompare; 
            }
        }
    }
}

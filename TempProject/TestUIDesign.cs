using Discord;
using Discord.WebSocket;
using IsekaiTechnologies.FacCord.Core;
using IsekaiTechnologies.FacCord.Core.DAL;
using IsekaiTechnologies.FacCord.Core.Mine;
using IsekaiTechnologies.FacCord.Core.MineGeneration;
using IsekaiTechnologies.FacCord.Core.Models.Equipments;
using IsekaiTechnologies.FacCord.Core.Models.Mines;
using IsekaiTechnologies.FacCord.Core.Models.Storages;
using IsekaiTechnologies.FacCord.Discord.Pages;
using IsekaiTechnologies.ImagiTextedDiscordGUI.DrawingContexts;
using IsekaiTechnologies.ImagiTextedDiscordGUI.Renderer;
using IsekaiTechnologies.ImagiTextedDiscordGUI.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TempProject
{
    [TestGallery("Test GUI Design")]
    class TestUIDesign : FacCordTestGallery
    {
        const int ViewportHeight = 30;
        private static List<string> EmptyBrushes = new List<string>(0);

        public DiscordSocketClient Client { get; set; }
        public IDrawingContext DrawingContext { get; set; }
        public IRenderer Renderer { get; set; }

        public override string Report()
        {
            return "";
        }

        public override void Run(string[] args)
        {
            var sc = new ServiceCollection();
            ConfigureServices(sc);
            Service = sc.BuildServiceProvider();

            MainAsync(args).GetAwaiter().GetResult();
        }

        private async Task MainAsync(string[] args)
        {
            var dal =  Service.GetRequiredService<IUnitOfWork>();

            Client = new DiscordSocketClient();
            Client.MessageReceived += Client_MessageReceived;
            #region Ser
            await Client.LoginAsync(TokenType.Bot, "NjY1NDU2Mjc0NzI0MzU2MDk2.Xjxd_A.FxzUcGrEmHE-zfQG9d00b_ziWHw");
            #endregion
            await Client.StartAsync();
            Client.Ready += Client_Ready;

            while (true)
            {
                Console.WriteLine("type e to exit");
                if (Console.ReadLine() == "e")
                {
                    await Client.StopAsync();
                }
            }
        }

        private async Task Client_Ready()
        {
            Renderer = new Renderer(Client.GetChannel(668006633020063756) as ITextChannel, new Region2DInt(1000, ViewportHeight), EmptyBrushes);
            DrawingContext = new DrawingContext(Renderer);
        }

        private async Task Client_MessageReceived(SocketMessage arg)
        {
            if (arg.Content.StartsWith("!d"))
            {
                using(var t = await DrawingContext.Begin())
                {
                    var p = await GetTestPlayerContext();
                    p.LastDiscoveredMine = await CreateFakeMine();

                    MinePage m = new MinePage(
                        DrawingContext);
                }
            }
        }

        private async Task<Mine> CreateFakeMine()
        {
            var dataAccess = Service.GetRequiredService<IUnitOfWork>();
            var pros = await dataAccess.Prospectors.GetAsync(16);
            var air = await dataAccess.Blocks.GetAsyncEagerly(1);
            var stone = await dataAccess.Blocks.GetAsyncEagerly(2);
            var dirt = await dataAccess.Blocks.GetAsyncEagerly(3);
            var grass = await dataAccess.Blocks.GetAsyncEagerly(4);
            var log = await dataAccess.Blocks.GetAsyncEagerly(5);
            var log2 = await dataAccess.Blocks.GetAsyncEagerly(6);
            var log3 = await dataAccess.Blocks.GetAsyncEagerly(7);
            var log4 = await dataAccess.Blocks.GetAsyncEagerly(8);
            var log5 = await dataAccess.Blocks.GetAsyncEagerly(9);

            Mine mine = new Mine(pros, 1, air); 
            mine.MaterialCounts = new Dictionary<Block, int>()
            {
                [dirt] = 100,
                [grass] = 100,
                [log] = 100,
                [log2] = 100,
                [log3] = 100,
                [log4] = 100,
                [log5] = 100,
                [stone] = 9300 - 9
            };

            await mine.GenerateMap();
            return mine;
        }
    }
}

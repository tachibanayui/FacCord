using IsekaiTechnologies.FacCord.Core;
using IsekaiTechnologies.FacCord.Core.DAL;
using IsekaiTechnologies.FacCord.Core.DAL.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TempProject
{
    public abstract class FacCordTestGallery : ITestGallery
    {
        public IServiceProvider Service { get; set; }

        public void ConfigureServices(ServiceCollection sc)
        {
            sc.AddDbContext<FacCordContext>(options => options.UseSqlServer("Data Source=DESKTOP-G5043LK\\SQLEXPRESS;Initial Catalog=TestFacCord;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"));
            sc.AddScoped<IUnitOfWork, UnitOfWork>();
        }


        public abstract string Report();
        public abstract void Run(string[] args);

        public async Task<PlayerContext> GetTestPlayerContext()
        {
            var uow = Service.GetRequiredService<IUnitOfWork>();
            var player = await uow.Players.GetAsync(3);
            return new PlayerContext(player);
        }
    }
}

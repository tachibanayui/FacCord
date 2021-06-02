using IsekaiTechnologies.FacCord.Core.DAL.Repositories;
using IsekaiTechnologies.FacCord.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace IsekaiTechnologies.FacCord.Core.DAL.EntityFramework.Repositories
{
    public class WorkshopRepository : Repository<Workshop>, IWorkshopRepository
    {
        public WorkshopRepository(FacCordContext ctx) : base(ctx)
        {
        }

        public async Task FetchStorageAsync(Workshop workshop)
        {
            var ws = await Context.Set<Workshop>().Include(x => x.Storage).FirstAsync(x => x.Id == workshop.Id);
            workshop.Storage = ws.Storage;
        }
    }
}

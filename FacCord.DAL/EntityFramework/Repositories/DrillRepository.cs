using IsekaiTechnologies.FacCord.Core.DAL.Repositories.Equipments;
using IsekaiTechnologies.FacCord.Core.Models.Equipments;
using IsekaiTechnologies.FacCord.Core.Models.Storages;
using System.Linq;
using System.Threading.Tasks;

namespace IsekaiTechnologies.FacCord.Core.DAL.EntityFramework.Repositories
{
    public class DrillRepository : Repository<Drill>, IDrillRepository
    {
        public DrillRepository(FacCordContext ctx) : base(ctx)
        {
        }

        public async Task<Item> GetStarterDrill()
        {
            return await FindAsync(x => x.IdName == "faccord:drill_test").FirstAsync();
        }
    }
}

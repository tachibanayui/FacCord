using IsekaiTechnologies.FacCord.Core.DAL.Repositories.Machines;
using IsekaiTechnologies.FacCord.Core.Models.Machines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsekaiTechnologies.FacCord.Core.DAL.EntityFramework.Repositories
{
    public class StorageMachineRepository :  Repository<StorageMachine>, IStorageMachineRepository
    {
        public StorageMachineRepository(FacCordContext ctx) : base(ctx)
        {
        }

        public async Task<StorageMachine> GetStarterStorageMachine()
        {
            return await FindAsync(x => x.IdName == "faccord:storage_1k").FirstAsync();
        }
    }
}

using IsekaiTechnologies.FacCord.Core.Models.Machines;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IsekaiTechnologies.FacCord.Core.DAL.Repositories.Machines
{
    public interface IStorageMachineRepository : IRepository<StorageMachine>
    {
        Task<StorageMachine> GetStarterStorageMachine();
    }
}

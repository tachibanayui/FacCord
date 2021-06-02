using IsekaiTechnologies.FacCord.Core.DAL;
using IsekaiTechnologies.FacCord.Core.Models;
using IsekaiTechnologies.FacCord.Core.StorageManagements;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IsekaiTechnologies.FacCord.Core.Providers
{
    public interface IStoragManagementProvider
    {
        Task<IStorageManagement> GetStorageManagement(IUnitOfWork dataAccess, Profile profile);
    }
}

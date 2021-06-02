using IsekaiTechnologies.FacCord.Core.DAL;
using IsekaiTechnologies.FacCord.Core.Models;
using IsekaiTechnologies.FacCord.Core.StorageManagements;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IsekaiTechnologies.FacCord.Core.Providers
{
    public class StorageManagementProvider : IStoragManagementProvider
    {
        public Task<IStorageManagement> GetStorageManagement(IUnitOfWork dataAccess, Profile profile)
        {
            return Task.FromResult(new StorageManagement(dataAccess, profile) as IStorageManagement);
        }
    }
}

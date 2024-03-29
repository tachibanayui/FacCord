// Generated by struct?.GetType() simple repo parttern generator!
using IsekaiTechnologies.FacCord.Core.Models;
using IsekaiTechnologies.FacCord.Core.Models.Storages;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace IsekaiTechnologies.FacCord.Core.DAL.Repositories.Storages
{
    public interface IStorageItemRepository : IRepository<StorageItem>
    {
        IAsyncEnumerable<StorageItem> GetItemFromProfileStorage(Profile profile, Expression<Func<StorageItem, bool>> predicate);
    }
}

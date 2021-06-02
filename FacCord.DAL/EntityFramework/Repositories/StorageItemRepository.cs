using IsekaiTechnologies.FacCord.Core.DAL.Repositories.Storages;
using IsekaiTechnologies.FacCord.Core.Models;
using IsekaiTechnologies.FacCord.Core.Models.Storages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace IsekaiTechnologies.FacCord.Core.DAL.EntityFramework.Repositories
{
    public class StorageItemRepository : Repository<StorageItem>, IStorageItemRepository
    {
        public StorageItemRepository(FacCordContext ctx) : base(ctx)
        {
        }

        public IAsyncEnumerable<StorageItem> GetItemFromProfileStorage(Profile profile, Expression<Func<StorageItem, bool>> predicate)
        {
            Expression<Func<StorageItem, bool>> expr = x => (long)Context.Entry(x).Property("WorkshopId").CurrentValue == profile.Workshop.Id;
            return Context.StorageItems.Where(expr).Where(predicate).AsAsyncEnumerable();
        }
    }
}

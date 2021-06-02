using IsekaiTechnologies.FacCord.Core.Models.Storages;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IsekaiTechnologies.FacCord.Core.StorageManagements
{
    public interface IStorageManagement : IDisposable
    {
        Task AddItemAsync(StorageItem item);
        Task AddItemRangeAsync(IEnumerable<StorageItem> items);

        Task RemoveItemAsync(long itemId);
        Task RemoveItemRangeAsync(IEnumerable<long> itemIds);

        IAsyncEnumerable<StorageItem> FindItemAsync(Expression<Func<StorageItem, bool>> predicate);
        Task<long> GetAvailableSlotCountAsync();

        Task<bool> TransferItemStackAsync(long itemId, StorageItem other, long count);
        Task ModifyItemAsync(long itemId, Action<StorageItem> action);
        Task ModifyItemAsync(long itemId, long count);
    }
}
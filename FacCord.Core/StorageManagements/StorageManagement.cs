using IsekaiTechnologies.FacCord.Core.DAL;
using IsekaiTechnologies.FacCord.Core.Models;
using IsekaiTechnologies.FacCord.Core.Models.Storages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IsekaiTechnologies.FacCord.Core.StorageManagements
{
    /// <summary>
    /// An ultility class to read/write/fetch from Player storage
    /// </summary>
    public class StorageManagement : IStorageManagement
    {
        public static object StorageLock = new object();
        public IUnitOfWork DataAccess { get; set; }
        public Profile Profile { get; set; }
        public TimeSpan RequestTimeout { get; set; } = TimeSpan.FromSeconds(60);

        public StorageManagement(IUnitOfWork dataAccess, Profile profile)
        {
            DataAccess = dataAccess;
            Profile = profile;
        }

        public IAsyncEnumerable<StorageItem> FindItemAsync(Expression<Func<StorageItem, bool>> predicate)
        {
            return DataAccess.StorageItems.GetItemFromProfileStorage(Profile, predicate).AsAsyncEnumerable();
        }

        public Task<long> GetAvailableSlotCountAsync()
        {
            return Task.FromResult(Profile.Workshop.StorageCapacity - Profile.Workshop.CurrentStorageItemCount);
        }

        // Arg Not required to be in the database
        public async Task AddItemAsync(StorageItem item)
        {
            await Task.Run(async () =>
            {
                bool lockTaken = false;
                Monitor.TryEnter(StorageLock, RequestTimeout, ref lockTaken);
                if (!lockTaken) throw new IOException("Requested timeout!");
                try
                {
                    // We will not prevent user from exceeding the limit because some forcefully rewards 
                    // might be stuck on a infinite loop eg. Hayday
                    //if (await GetAvailableSlotCountAsync() < item.Count)
                    //{
                       
                    //}

                    var target = await FindItemAsync(x => x.Item == item.Item)
                        .WhereAwait(async (x) => await x.ItemMeta.IsMergeable(item.ItemMeta))
                        .FirstOrDefaultAsync();

                    if (target == null)
                    {
                        var itemToAdd = item.Clone() as StorageItem;
                        itemToAdd.ItemMeta = itemToAdd.ItemMeta ?? await itemToAdd.Item.CreateItemMeta();

                        await DataAccess.StorageItems.AddAsync(itemToAdd);
                    }
                    else
                    {
                        await target.Merge(item);
                    }

                    Profile.Workshop.CurrentStorageItemCount += item.Count;
                }
                finally
                {
                    Monitor.Exit(StorageLock);
                }
            });
        }
        public async Task AddItemRangeAsync(IEnumerable<StorageItem> items)
        {
            foreach (var item in items)
            {
                await AddItemAsync(item);
            }
        }

        // Arg required to be in the database
        public async Task RemoveItemAsync(long itemId)
        {
            await Task.Run(async () =>
            {
                bool lockTaken = false;
                Monitor.TryEnter(StorageLock, RequestTimeout, ref lockTaken);
                if (!lockTaken) throw new IOException("Requested timeout!");
                try
                {
                    var dbItem = await FindItemAsync(x => x.Id == itemId).FirstAsync();
                    if (dbItem != null)
                    {
                        await DataAccess.StorageItems.RemoveAsync(dbItem);
                        Profile.Workshop.CurrentStorageItemCount -= dbItem.Count;
                    }
                    else
                    {
                        throw new ArgumentException("The specified itemId is not exist in the player storage");
                    }
                }
                finally
                {
                    Monitor.Exit(StorageLock);
                }
            });
        }
        public async Task RemoveItemRangeAsync(IEnumerable<long> itemIds)
        {
            foreach (var item in itemIds)
            {
                await RemoveItemAsync(item);
            }
        }

        // If other StorageItem is in the profile storage item, you need to substract the StorageItemCount manually
        public async Task<bool> TransferItemStackAsync(long itemId, StorageItem other, long count)
        {
            if (other.Count < count) throw new ArgumentException("item count stored in other is smaller than count");
            var dbItem = await FindItemAsync(x => x.Id == itemId && x.Item == other.Item).FirstOrDefaultAsync();
            if (dbItem == null) return false;

            if(await dbItem.ItemMeta.IsMergeable(other.ItemMeta))
            {
                dbItem.Count += other.Count;
                Profile.Workshop.CurrentStorageItemCount += other.Count;
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task ModifyItemAsync(long itemId, Action<StorageItem> action)
        {
            action(await FindItemAsync(x => x.Id == itemId).FirstOrDefaultAsync());
        }

        public async Task ModifyItemAsync(long itemId, long count)
        {
            await ModifyItemAsync(itemId, x => { Profile.Workshop.CurrentStorageItemCount -= x.Count; x.Count = count; });
            Profile.Workshop.CurrentStorageItemCount += count;
        }


        public void Dispose()
        {

        }
    }
}

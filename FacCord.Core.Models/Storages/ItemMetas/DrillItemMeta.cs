using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IsekaiTechnologies.FacCord.Core.Models.Storages.ItemMetas
{
    public class DrillItemMeta : ItemMeta, IDamageable
    {
        public long Durability { get; set; }

        public Task<long> GetDurabilityAsync()
        {
            return Task.FromResult(Durability);
        }

        public Task SetDurabilityAsync(long durability)
        {
            Durability = durability;
            return Task.CompletedTask;
        }

        public override Task<bool> IsMergeable(ItemMeta other)
        {
            return Task.FromResult(false);
        }

        public Task DamageAsync(long amount) => SetDurabilityAsync(-amount);
    }
}

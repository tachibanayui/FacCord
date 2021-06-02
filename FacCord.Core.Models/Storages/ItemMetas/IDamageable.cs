using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IsekaiTechnologies.FacCord.Core.Models.Storages.ItemMetas
{
    public interface IDamageable
    {
        Task<long> GetDurabilityAsync();
        Task SetDurabilityAsync(long durability);
        Task DamageAsync(long amount);
    }
}

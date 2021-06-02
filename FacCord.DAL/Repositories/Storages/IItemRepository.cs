using IsekaiTechnologies.FacCord.Core.Models.Storages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IsekaiTechnologies.FacCord.Core.DAL.Repositories.Storages
{
    public interface IItemRepository : IRepository<Item>
    {
        Task<Item> GetItemByIdName(string idName);
    }
}

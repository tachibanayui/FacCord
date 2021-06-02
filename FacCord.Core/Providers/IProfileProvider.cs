using IsekaiTechnologies.FacCord.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IsekaiTechnologies.FacCord.Core.Providers
{
    public interface IProfileProvider
    {
        Task<Profile> CreateNewProfile(Player player, string name);
    }
}

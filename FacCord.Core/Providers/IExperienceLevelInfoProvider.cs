using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IsekaiTechnologies.FacCord.Core.Providers
{
    public interface IExperienceLevelInfoProvider
    {
        Task<int> GetExperienceForLevel(int level);
    }
}

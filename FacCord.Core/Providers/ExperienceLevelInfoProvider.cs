using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IsekaiTechnologies.FacCord.Core.Providers
{
    public class ExperienceLevelInfoProvider : IExperienceLevelInfoProvider
    {
        public Task<int> GetExperienceForLevel(int level)
        {
            return Task.FromResult((int)Math.Round(100 + 0.1 * Math.Pow(level, 3)));
        }
    }
}

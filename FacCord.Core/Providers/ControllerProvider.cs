using IsekaiTechnologies.FacCord.Core.Controllers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IsekaiTechnologies.FacCord.Core.Providers
{
    public class ControllerProvider : IControllerProvider
    {
        public Task<IMineController> GetMineController(GameManager manager, PlayerContext context)
        {
            return Task.FromResult(new MineController2(context) as IMineController);
        }
    }
}

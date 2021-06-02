using IsekaiTechnologies.FacCord.Core.Controllers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IsekaiTechnologies.FacCord.Core.Providers
{
    public interface IControllerProvider
    {
        Task<IMineController> GetMineController(GameManager manager, PlayerContext context);
    }
}

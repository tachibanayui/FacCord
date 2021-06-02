using IsekaiTechnologies.FacCord.Core.DAL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IsekaiTechnologies.FacCord.Core.Controllers
{
    public abstract class PlayerController
    {
        public PlayerController(PlayerContext ctx)
        {
            Context = ctx;
            DataAccess = ctx.Manager.Settings.DataAccess;
        }

        public PlayerContext Context { get; protected set; }
        public IUnitOfWork DataAccess { get; protected set; }
    }
}

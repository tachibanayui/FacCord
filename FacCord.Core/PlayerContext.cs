using IsekaiTechnologies.FacCord.Core.Controllers;
using IsekaiTechnologies.FacCord.Core.DAL;
using IsekaiTechnologies.FacCord.Core.Mine;
using IsekaiTechnologies.FacCord.Core.MineGeneration;
using IsekaiTechnologies.FacCord.Core.Models;
using IsekaiTechnologies.FacCord.Core.Models.Storages;
using IsekaiTechnologies.FacCord.Core.StorageManagements;
using IsekaiTechnologies.FacCord.Core.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace IsekaiTechnologies.FacCord.Core
{
    public class PlayerContext
    {
        public GameManager Manager { get; set; }
        public Player CurrentPlayer { get; protected set; }

        public AbstractMine LastDiscoveredMine { get; set; }
        public MiningContext MiningContext { get; set; }

        public IMineController MineController { get; set; }

        public PlayerContext(Player player)
        {
            CurrentPlayer = player;
        }

    }
}

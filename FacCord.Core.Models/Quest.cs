using IsekaiTechnologies.FacCord.Core.Models.Storages;
using System;
using System.Collections.Generic;
using System.Text;

namespace IsekaiTechnologies.FacCord.Core.Models
{
    public class Quest
    {
        /// <summary>
        /// The quest id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// The client for this quest
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// The quest decription
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The quest category. For example: Main quest, Daily quest
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// The items Which the client requested in order to finish this quest
        /// </summary>
        public List<StorageItem> RequestedItems { get; set; }

        /// <summary>
        /// The Exp reward for this quest
        /// </summary>
        public int RewardExp { get; set; }

        /// <summary>
        /// The Coin reward for this quest
        /// </summary>
        public int RewardCoin { get; set; }

        /// <summary>
        /// The Items reward for this quest
        /// </summary>
        public List<StorageItem> RewardItems { get; set; }

        /// <summary>
        /// The recipe reward for this quest
        /// </summary>
        public List<Recipe> RewardRecipes { get; set; }

        /// <summary>
        /// Unlock this quest after complete another quest
        /// </summary>
        public Quest UnlockAfter { get; set; }

        /// <summary>
        /// The mininum level to accept this quest
        /// </summary>
        public short MininumLevel { get; set; }
    }
}

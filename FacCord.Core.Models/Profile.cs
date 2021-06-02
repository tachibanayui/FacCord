using IsekaiTechnologies.FacCord.Core.Models.Equipments;
using IsekaiTechnologies.FacCord.Core.Models.Mines;
using IsekaiTechnologies.FacCord.Core.Models.Shops;
using IsekaiTechnologies.FacCord.Core.Models.Storages;
using System;
using System.Collections.Generic;

namespace IsekaiTechnologies.FacCord.Core.Models
{
    public class Profile
    {
        public long Id { get; set; }
        public string ProfileName { get; set; }
        public Player Player { get; set; }
        public bool IsActive { get; set; }
        public long Fund { get; set; } 
        public short Level { get; set; }
        public int Exp { get; set; }
        public DateTime LastFindMine { get; set; }

        public string PlayerEmoji { get; set; }

        public RuptureChemical RuptureChemical { get; set; }
        public StorageItem CurrentProspector { get; set; }
        public StorageItem CurrentDrill { get; set; }

        public Region SelectedRegion { get; set; }
        public List<Region> UnlockedRegions { get; set; }

        public Workshop Workshop { get; set; }
        public List<Recipe> UnlockedRecipes { get; set; }
        public List<Quest> AvailableQuests { get; set; }
        public List<Quest> FinishedQuests { get; set; }

        public List<Transaction> PurchaseHistory { get; set; }
        public List<ExclusiveListings> ExclusiveOffers { get; set; }
    }
}
using IsekaiTechnologies.FacCord.Core.Models.Storages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IsekaiTechnologies.FacCord.Core.Models.Shops
{
    public class Listing
    {
        /// <summary>
        /// The id of this listing
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// The shop category. For example: Raw material, Machines
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// The items offered in this listing
        /// </summary>
        public List<StorageItem> Offerings { get; set; }

        /// <summary>
        /// The price for this item
        /// </summary>
        public long Price { get; set; }

        /// <summary>
        /// The minimum level to purchase items in this listing
        /// </summary>
        public short MinimumLevel { get; set; }

        [NotMapped]
        public bool IsListingLimited => Limit != -1;

        /// <summary>
        /// The amount can be purchased until reset
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// The amount of time to reset this listings. The interval is based on server system time
        /// </summary>
        public TimeSpan LimitResetInterval { get; set; }
    }
}

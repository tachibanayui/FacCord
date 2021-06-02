using System;
using System.Collections.Generic;
using System.Text;

namespace IsekaiTechnologies.FacCord.Core.Models.Shops
{
    public class Transaction
    {
        /// <summary>
        /// The id of this transaction 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// The buyer for this tranaction
        /// </summary>
        public Profile Buyer { get; set; }

        /// <summary>
        /// The listing which the buyer purchased in this tranaction
        /// </summary>
        public Listing PurchasedListing { get; set; }

        /// <summary>
        /// The purchased amount for PurchasedListing
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// How much the buyer pays for this transaction
        /// </summary>
        public long Cost { get; set; }

        /// <summary>
        /// When the buyer made the tracsation
        /// </summary>
        public DateTime PurchasedAt { get; set; }
    }
}

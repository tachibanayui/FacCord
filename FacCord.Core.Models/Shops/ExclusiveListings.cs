using System;
using System.Collections.Generic;
using System.Text;

namespace IsekaiTechnologies.FacCord.Core.Models.Shops
{
    public class ExclusiveListings : Listing
    {
        /// <summary>
        /// When this offer will be taken down from player's shop listings
        /// </summary>
        public DateTime OfferEnds { get; set; }
    }
}

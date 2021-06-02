using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IsekaiTechnologies.FacCord.Core.Models
{
    public class EmojiLookup
    {
        public long Id { get; set; }
        [Required]
        public long TargetId { get; set; }
        [MaxLength(50)]
        public string Emoji { get; set; }
        /// <summary>
        /// 0 for item, 1 for block, 2 for region, 3 for dimension
        /// </summary>
        [Required]
        public int Type { get; set; }
    }
}

using IsekaiTechnologies.FacCord.Core.Models.Storages;
using System;
using System.Collections.Generic;
using System.Text;

namespace IsekaiTechnologies.FacCord.Discord.Models
{
    public class ItemPickerRequest
    {
        public List<StorageItem> ItemPool { get; set; }
        public string PickerTitle { get; set; }

        public List<Type> TypeFilters { get; set; }
        public bool FiltersChangeableByUser { get; set; }
        public bool Cancelable { get; set; } = true;
        public string InitialKeyword { get; set; }

        public long MinItemCount { get; set; } 
        public long MaxItemCount { get; set; } 
        public long MinStorageItemCount { get; set; }
        public long MaxStorageItemCount { get; set; }

        public static ItemPickerRequest CreateSingleItemRequest()
        {
            return new ItemPickerRequest()
            {
                MinItemCount = 1,
                MaxItemCount = 1,
                MinStorageItemCount = 1,
                MaxStorageItemCount = 1
            };
        }
    }
}

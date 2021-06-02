using System;
using System.Collections.Generic;
using System.Text;

namespace IsekaiTechnologies.FacCord.Discord.Utils
{
    public static class TimeSpanUtils
    {
        public static string FormatHourAndMinute(this TimeSpan ts)
        {
            return $"{Math.Floor(ts.TotalHours):00}:{ts:mm}";
        }

        
    }
}

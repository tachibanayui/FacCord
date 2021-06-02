using System;
using System.Collections.Generic;
using System.Text;

namespace IsekaiTechnologies.FacCord.Core.Utils
{
    public static class MathExtension
    {
        public static int Clamp(int min, int max, int value) => Math.Max(min, Math.Min(max, value));
        public static long Clamp(long min, long max, long value) => Math.Max(min, Math.Min(max, value));
        public static bool IsInRange(int min, int max, int value) => value <= max && value >= min;
    }
}

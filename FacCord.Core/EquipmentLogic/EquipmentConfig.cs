using IsekaiTechnologies.FacCord.Core.Models.Equipments;
using IsekaiTechnologies.FacCord.Core.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace IsekaiTechnologies.FacCord.Core.EquipmentLogic
{
    public class EquipmentConfig : IEquipmentConfig
    {
        const double Modifier = 0.114;
        public List<double> EfficiencyByLevels = new List<double>();

        public EquipmentConfig()
        {
            EfficiencyByLevels.Add(0);
            for (int i = 1; i < 20; i++)
            {
                EfficiencyByLevels.Add(EfficiencyByLevels[i - 1] + (1 - EfficiencyByLevels[i - 1]) * Modifier);
            }
        }

        public int GetBreakHitCount(Drill drill, int toughness)
        {
            return MathExtension.Clamp(1, int.MaxValue, (int) Math.Ceiling(EfficiencyByLevels[drill.Efficiency - 1] * toughness));
        }
    }
}

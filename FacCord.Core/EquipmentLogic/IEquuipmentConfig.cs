using IsekaiTechnologies.FacCord.Core.Models.Equipments;
using System;
using System.Collections.Generic;
using System.Text;

namespace IsekaiTechnologies.FacCord.Core.EquipmentLogic
{
    // Calculation of equipment stats such as Drill Efficiency
    public interface IEquipmentConfig
    {
        int GetBreakHitCount(Drill drill, int toughness);
    }
}

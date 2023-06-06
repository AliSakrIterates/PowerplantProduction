using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerplantProduction.Services.ProductionPlan
{
    public class PowerPlantCost
    {
        public PowerPlant PowerPlant { get; set; }
        public int Cost { get; set; }
        public int Power { get; set; }
    }

    public class CombinationCosts
    {
        public List<PowerPlantCost> PowerPlantCosts { get; set; }
        public int TotalCost { get; set; }
    }
}

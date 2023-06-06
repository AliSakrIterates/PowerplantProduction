using MediatR;
using System.Numerics;

namespace PowerplantProduction.Services.ProductionPlan
{
    public class PowerPlanHandler : IRequestHandler<ProductionPlanRequest, ProductionPlanResponse>
    {
        public PowerPlanHandler() { }

        public async Task<ProductionPlanResponse> Handle(ProductionPlanRequest request, CancellationToken cancellationToken)
        {
            var fuelPowerplants = new List<PowerPlantCost>();
            int targetLoadWithWind = request.Load;
            List<PowerPlantOutput> minimalCostCombination = new List<PowerPlantOutput>();

            SortPowerPlants(request, fuelPowerplants);

            var combinationCosts = GetCombinationCosts(request, fuelPowerplants, targetLoadWithWind);

            var minimalCombination = combinationCosts.OrderBy(obj => obj.TotalCost).First();

            foreach (var cost in minimalCombination.PowerPlantCosts)
            {
                minimalCostCombination.Add(new PowerPlantOutput()
                {
                    Name = cost.PowerPlant.Name,
                    P = cost.Power
                });
            }

            return new ProductionPlanResponse
            {
                Outputs = minimalCostCombination
            };
        }

        private List<CombinationCosts> GetCombinationCosts(ProductionPlanRequest request, List<PowerPlantCost> fuelPowerplants, int targetLoadWithWind)
        {
            var combinationCosts = new List<CombinationCosts>();

            for (int i = 0; i < fuelPowerplants.Count; i++)
            {
                var powerPlant = fuelPowerplants[i];
                var power = GetPowerProduced(powerPlant.PowerPlant.Pmax, targetLoadWithWind, powerPlant.PowerPlant.Type == Enums.PowerplantType.WindTurbine ? request.Fuels.Wind : null);

                var combination = new CombinationCosts
                {
                    PowerPlantCosts = new List<PowerPlantCost>
                     {
                         new PowerPlantCost
                         {
                             PowerPlant = powerPlant.PowerPlant,
                             Cost = powerPlant.Cost,
                             Power = powerPlant.PowerPlant.Type == Enums.PowerplantType.WindTurbine ? power : powerPlant.PowerPlant.Pmin
                         }
                     },
                    TotalCost = powerPlant.Cost * power
                };

                var currentLoad = targetLoadWithWind - combination.PowerPlantCosts[0].Power;

                for (int j = 0; j < fuelPowerplants.Count && currentLoad > 0; j++)
                {
                    if (j != i)
                    {
                        var fuelPowerPlant = fuelPowerplants[j];
                        var fuelPower = GetPowerProduced(fuelPowerPlant.PowerPlant.Pmax, currentLoad, fuelPowerPlant.PowerPlant.Type == Enums.PowerplantType.WindTurbine ? request.Fuels.Wind : null);

                        if (fuelPowerPlant.PowerPlant.Pmin <= currentLoad)
                        {
                            combination.PowerPlantCosts.Add(new PowerPlantCost
                            {
                                PowerPlant = fuelPowerPlant.PowerPlant,
                                Cost = fuelPowerPlant.Cost,
                                Power = fuelPower
                            });

                            combination.TotalCost += fuelPowerPlant.Cost * fuelPower;
                            currentLoad -= fuelPower;
                        }
                    }
                }

                if (currentLoad == 0)
                {
                    combinationCosts.Add(combination);
                }
            }

            return combinationCosts;
        }

        private void SortPowerPlants(ProductionPlanRequest powerPlantsDetails, List<PowerPlantCost> fuelPowerPlants)
        {
            var load = powerPlantsDetails.Load;

            foreach (var powerPlant in powerPlantsDetails.PowerPlants.Where(pp => pp.Pmin <= load))
            {
                fuelPowerPlants.Add(new PowerPlantCost()
                {
                    Cost = GetCost(powerPlant.Type, powerPlantsDetails.Fuels, powerPlant.Efficiency),
                    PowerPlant = powerPlant
                });
            }

            fuelPowerPlants.Sort((a, b) => a.Cost.CompareTo(b.Cost));
        }

       
        private int GetPowerProduced(int pmax, int targetload, int? wind = null)
        {
            int powerProduced;

            if (wind.HasValue)
            {
                int windLoad = (pmax * wind.Value) / 100;
                powerProduced = Math.Min(windLoad, targetload);
            }
            else
            {
                powerProduced = Math.Min(pmax, targetload);
            }

            return powerProduced;
        }


        private int GetCost(Enums.PowerplantType type, Fuels pricePerHour, double efficiency)
        {
            switch (type)
            {
                case Enums.PowerplantType.TurboJet:
                    return (int)Math.Round(pricePerHour.Kerosine / efficiency);

                case Enums.PowerplantType.GasFired:
                    return (int)Math.Round(pricePerHour.Gas / efficiency);
            }
            return 0;
        }
    }
}

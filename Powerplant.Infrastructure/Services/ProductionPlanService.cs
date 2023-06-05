using Powerplant.Domain.Interfaces;
using Powerplant.Domain.Models;
using Powerplant.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Powerplant.Infrastructure.Services
{
    public class ProductionPlanService : IProductionPlanService
    {
        public Task<List<ProductionPlanModel>> CalculateProductionPlan(List<PowerplantModel> powerPlants, decimal load, FuelInfoModel fuelInfo)
        {
            List<ProductionPlanModel> result = new();

            Dictionary<PowerplantModel, decimal> costPerMWh = new Dictionary<PowerplantModel, decimal>();
            decimal co2TonsPerMWh = 0.3M;

            powerPlants.ForEach(p =>
            {
                switch (p.Type)
                {
                    case Domain.Enums.PowerplantType.GasFired:
                        costPerMWh.Add(p, (fuelInfo.GasEuroPerMWh / p.Efficiency) + (fuelInfo.Co2EuroPerTon * co2TonsPerMWh));
                        break;
                    case Domain.Enums.PowerplantType.TurboJet:
                        costPerMWh.Add(p, fuelInfo.KerosineEuroPerMWh / p.Efficiency);
                        break;
                    case Domain.Enums.PowerplantType.WindTurbine:
                        p.Pmax *= (fuelInfo.WindEfficiencyPercent / 100);
                        if (p.Pmax > 0)
                            costPerMWh.Add(p, 0);
                        break;
                    default:
                        break;
                }
            });

            // Sort by cost
            costPerMWh = costPerMWh.OrderBy(p => p.Value).ToDictionary(kv => kv.Key, kv => kv.Value);

            return Task.FromResult(SelectPowerPlants(costPerMWh.Keys.ToList(), load));
        }

        private List<ProductionPlanModel> SelectPowerPlants(List<PowerplantModel> powerPlants, decimal load)
        {
            List<ProductionPlanModel> selectedPlants = new List<ProductionPlanModel>();

            while (load > 0 && powerPlants.Count > 0)
            {
                // Calculate the remaining load to be fulfilled
                decimal remainingLoad = load - selectedPlants.Sum(p => p.P);
                if (remainingLoad == 0)
                    break;

                // Find the cheapest usable plant
                var plant = powerPlants.FirstOrDefault(p => (p.Pmin <= remainingLoad && remainingLoad <= p.Pmax) || p.Pmin <= remainingLoad) ?? throw new FulfillmentException("Unable to fulfill");

                if (remainingLoad <= plant.Pmax)
                    selectedPlants.Add(new ProductionPlanModel() { Name = plant.Name, P = remainingLoad });
                else
                    selectedPlants.Add(new ProductionPlanModel() { Name = plant.Name, P = plant.Pmax });

                powerPlants.Remove(plant);
            }

            return selectedPlants;
        }
    }
}

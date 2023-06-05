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
        public Task<IEnumerable<ProductionPlanModel>> CalculateProductionPlan(List<PowerplantModel> powerPlants, decimal load, FuelInfoModel fuelInfo)
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

            // Optimize
            List<Tuple<PowerplantModel, decimal>> allocations = new();
            OptimizePlantAllocations(allocations, costPerMWh.Keys.ToList(), load);

            return Task.FromResult(allocations.Select(p => new ProductionPlanModel()
            {
                Name = p.Item1.Name,
                P = p.Item2
            }));
        }

        public void OptimizePlantAllocations(List<Tuple<PowerplantModel, decimal>> allocations, List<PowerplantModel> powerPlants, decimal load)
        {
            var nextPlant = powerPlants.FirstOrDefault() ?? throw new FulfillmentException("Unable to fulfill");

            if (nextPlant.Pmin <= load)
            {
                var power = Math.Min(nextPlant.Pmax, load);
                if (power == nextPlant.Pmax)
                    powerPlants.Remove(nextPlant);

                load -= power;
                allocations.Add(new Tuple<PowerplantModel, decimal>(nextPlant, power));
            }
            else
            {
                // Pick a previous plant who's assignment can be modified
                var lastPlant = allocations.LastOrDefault(p => p.Item2 > load && p.Item1.Pmin >= nextPlant.Pmin - load) ?? throw new FulfillmentException("Unable to fulfill");

                // Reduce load on lastPlant
                allocations.Remove(lastPlant);
                allocations.Add(new Tuple<PowerplantModel, decimal>(lastPlant.Item1, lastPlant.Item2 - (nextPlant.Pmin - load)));

                // Add nextPlant
                allocations.Add(new Tuple<PowerplantModel, decimal>(nextPlant, nextPlant.Pmin));
                load = 0;
            }

            if (load > 0)
                OptimizePlantAllocations(allocations, powerPlants, load);
        }
    }
}

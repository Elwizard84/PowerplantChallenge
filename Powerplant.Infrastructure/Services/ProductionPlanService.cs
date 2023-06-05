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

            // Probe feasibility
            var sortedPlants = costPerMWh.Keys.ToList();
            List<Tuple<PowerplantModel, decimal>> allocations = new();

            var nextPlant = sortedPlants.FirstOrDefault(p => p.Pmin <= load) ?? throw new FulfillmentException("Unable to fulfill");
            AddAllocation(allocations, sortedPlants, nextPlant, load, out load);

            // Optimize
            OptimizePlantAllocations(allocations, sortedPlants, load);

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
                AddAllocation(allocations, powerPlants, nextPlant, load, out load);
            }
            else
            {
                // Pick a previous plant who's assignment can be modified
                var lastPlant = allocations.LastOrDefault() ?? throw new FulfillmentException("Unable to fulfill");

                // Correction
                var correction = nextPlant.Pmin - load;

                if (lastPlant.Item2 - correction > lastPlant.Item1.Pmin)
                {
                    // Reduce load on lastPlant
                    allocations.Remove(lastPlant);
                    allocations.Add(new Tuple<PowerplantModel, decimal>(lastPlant.Item1, lastPlant.Item2 - correction));

                    // Add nextPlant
                    allocations.Add(new Tuple<PowerplantModel, decimal>(nextPlant, nextPlant.Pmin));
                    load = 0;
                }
                else
                {
                    // Discard the no-good plant
                    load += lastPlant.Item2;
                    allocations.Remove(lastPlant);
                }
            }

            if (load > 0)
                OptimizePlantAllocations(allocations, powerPlants, load);
        }

        private void AddAllocation(List<Tuple<PowerplantModel, decimal>> allocations, List<PowerplantModel> powerPlants, PowerplantModel nextPlant, decimal load, out decimal remainingLoad)
        {
            var power = Math.Min(nextPlant.Pmax, load);
            if (power == nextPlant.Pmax)
                powerPlants.Remove(nextPlant);

            remainingLoad = load - power;
            allocations.Add(new Tuple<PowerplantModel, decimal>(nextPlant, power));
        }
    }
}

using Powerplant.Domain.Interfaces;
using Powerplant.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Powerplant.Infrastructure.Services
{
    public class ProductionPlanService : IProductionPlanService
    {
        public Task<List<ProductionPlan>> CalculateProductionPlan(List<PowerplantModel> powerPlants, int load, FuelInfoModel fuelInfo)
        {
            List<ProductionPlan> result = new List<ProductionPlan>
                {
                    new ProductionPlan { Name = "Gas", P = 100 },
                    new ProductionPlan { Name = "Wind", P = 200 }
                };

            return Task.FromResult(result);
        }
    }
}

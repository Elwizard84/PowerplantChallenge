using Powerplant.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Powerplant.Domain.Interfaces
{
    public interface IProductionPlanService
    {
        Task<List<ProductionPlan>> CalculateProductionPlan(List<PowerplantModel> powerPlants, int load, FuelInfoModel fuelInfo);
    }
}

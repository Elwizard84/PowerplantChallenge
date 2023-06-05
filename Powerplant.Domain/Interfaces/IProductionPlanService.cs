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
        Task<List<ProductionPlanModel>> CalculateProductionPlan(List<PowerplantModel> powerPlants, decimal load, FuelInfoModel fuelInfo);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Powerplant.Domain.Models
{
    public class FuelInfoModel
    {
        public decimal GasEuroPerMWh { get; set; }
        public decimal KerosineEuroPerMWh { get; set; }
        public decimal Co2EuroPerTon { get; set; }
        public decimal WindEfficiencyPercent { get; set; }
    }
}

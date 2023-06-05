using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Powerplant.Domain.Models
{
    public class ProductionPlanModel
    {
        public string Name { get; set; }
        public decimal P { get; set; }
    }
}

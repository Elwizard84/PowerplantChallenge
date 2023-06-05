using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Powerplant.Domain.Models
{
    public class ProductionPlan
    {
        public string Name { get; set; }
        public double P { get; set; }
    }
}

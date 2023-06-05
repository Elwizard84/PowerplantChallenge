using Powerplant.Domain.Enums;

namespace Powerplant.Domain.Models
{
    public class PowerplantModel
    {
        public string Name { get; set; }
        public PowerplantType Type { get; set; }
        public decimal Efficiency { get; set; }
        public decimal Pmin { get; set; }
        public decimal Pmax { get; set; }
    }
}

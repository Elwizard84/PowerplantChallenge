using Powerplant.Domain.Enums;

namespace Powerplant.Domain.Models
{
    public class PowerplantModel
    {
        public string Name { get; set; }
        public PowerplantType Type { get; set; }
        public double Efficiency { get; set; }
        public int Pmin { get; set; }
        public int Pmax { get; set; }
    }
}

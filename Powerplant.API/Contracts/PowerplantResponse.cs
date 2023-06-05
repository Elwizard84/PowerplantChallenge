using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Powerplant.API.Contracts
{
    public class PowerplantResponse
    {
        public List<ProductionPlan> Production { get; set; }
    }

    public class ProductionPlan
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("p")]
        public double P { get; set; }
    }
}

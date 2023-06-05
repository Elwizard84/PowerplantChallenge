using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Powerplant.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PowerplantType
    {
        GasFired,
        TurboJet,
        WindTurbine
    }
}

using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace KiloWattNavigator.Domain
{
    public class ProductionPlanRequest
    {
        [JsonPropertyName("load")]
        public double Load { get; set; }
        [JsonPropertyName("fuels")]
        public Fuels Fuels { get; set; }
        [JsonPropertyName("powerplants")]
        public List<Powerplant> Powerplants { get; set; }
    }
}

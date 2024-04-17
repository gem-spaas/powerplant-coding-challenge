using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace powerplant.Models
{
    public class PowerPlantRequest
    {
        public double Load { get; set; }
        [JsonPropertyName("fuels")]
        public Fuels Fuels { get; set; }
        public List<Powerplant> PowerPlants { get; set; }

    }
}

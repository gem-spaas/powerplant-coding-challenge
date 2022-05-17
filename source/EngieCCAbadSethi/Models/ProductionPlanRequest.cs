using Newtonsoft.Json;

namespace CalculatePowerGenerationByPowerPlants.Models
{
    public class ProductionPlanRequest
    {
        [JsonProperty("load")]
        public double Load { get; set; }

        [JsonProperty("fuels")]
        public Dictionary<string, double> Fuels { get; set; }

        [JsonProperty("powerplants")]
        public List<PowerPlant> Powerplants { get; set; }

    }
}

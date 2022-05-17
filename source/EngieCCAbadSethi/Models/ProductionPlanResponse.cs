using Newtonsoft.Json;

namespace CalculatePowerGenerationByPowerPlants.Models
{
    public class ProductionPlanResponse
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("p")]
        public double P { get; set; }

    }
}

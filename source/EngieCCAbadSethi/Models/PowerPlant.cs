using Newtonsoft.Json;

namespace CalculatePowerGenerationByPowerPlants.Models
{
    public class PowerPlant
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("efficiency")]
        public double Efficiency { get; set; }

        [JsonProperty("pmin")]
        public int Pmin { get; set; }

        [JsonProperty("pmax")]
        public int Pmax { get; set; }

    }
}

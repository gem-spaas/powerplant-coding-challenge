using System.Text.Json.Serialization;

namespace PowerplantChallenge.Model {
    public class ProductionPlanRequest
    {
        [JsonPropertyName("load")] 
        public int Load { get; set; }
        [JsonPropertyName("fuels")] 
        public FuelInput Fuels { get; set; }
        [JsonPropertyName("powerplants")] 
        public List<PowerplantInput> PowerPlants { get; set; }
    }
}
using System.Text.Json.Serialization;

namespace EngieApi;

public class ProductionPlanRequest
{
    [JsonPropertyName("load")]
    public int Load { get; set; }
    [JsonPropertyName("fuels")]
    public Fuel Fuels { get; set; }
    [JsonPropertyName("powerplants")]
    public List<PowerPlant> PowerPlants { get; set; }

}

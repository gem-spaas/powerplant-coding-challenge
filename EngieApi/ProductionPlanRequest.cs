using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace EngieApi;

public class ProductionPlanRequest
{
    [JsonPropertyName("load")] public int Load { get; set; }
    [JsonPropertyName("fuels")] public Fuels Fuels { get; set; }
    [JsonPropertyName("powerplants")] public List<PowerPlant> PowerPlants { get; set; }

}

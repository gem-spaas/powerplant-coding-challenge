using System.Text.Json.Serialization;

namespace EngieApi;

public class ProductionPlanResponse
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("p")]
    public int P { get; set; }
}

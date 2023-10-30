using System.Text.Json.Serialization;

namespace PowerPlant.Models;

public record Powerplant(
    [property: JsonPropertyName("name")] string Name, 
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("efficiency")] float Efficiency,
    [property: JsonPropertyName("pmin")] int Pmin,
    [property: JsonPropertyName("pmax")] int PMax);

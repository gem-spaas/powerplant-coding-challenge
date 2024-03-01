using System.Text.Json.Serialization;

namespace GlobalEnergyManagement.Application.DTOs;

public record PowerPlants(
    string Name,
    string Type,
    double Efficiency,
    [property: JsonPropertyName("pmin")] double PowerMin,
    double PowerMax,
    double Cost
)
{
    [JsonPropertyName("pmax")] public double PowerMax { get; set; } = PowerMax;
    [JsonIgnore] public double Cost { get; set; } = Cost;
}
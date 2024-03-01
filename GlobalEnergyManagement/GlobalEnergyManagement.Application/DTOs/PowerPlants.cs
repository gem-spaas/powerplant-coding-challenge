using System.Text.Json.Serialization;

namespace GlobalEnergyManagement.Application.DTOs;

public record PowerPlants(
    string Name,
    string Type,
    float Efficiency,
    [property: JsonPropertyName("pmin")] int PowerMin,
    [property: JsonPropertyName("pmax")] int PowerMax
);
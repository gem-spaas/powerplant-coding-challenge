using System.Text.Json.Serialization;

namespace GlobalEnergyManagement.Application.DTOs;

public record PowerPlantPower(
    string Name,
    [property: JsonPropertyName("p")] double Power,
    [property: JsonIgnore] double Cost
);
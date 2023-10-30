using System.Text.Json.Serialization;

namespace PowerPlant.Models;

public record RequestedLoad(
    [property: JsonPropertyName("load")] double Load,
    [property: JsonPropertyName("fuels")] Fuels Fuels, 
    [property: JsonPropertyName("powerplants")] Powerplant[] Powerplants
    );
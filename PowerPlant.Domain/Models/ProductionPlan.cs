using System.Text.Json.Serialization;

namespace PowerPlant.Models;

public record ProductionPlan(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("p")] double P
    );

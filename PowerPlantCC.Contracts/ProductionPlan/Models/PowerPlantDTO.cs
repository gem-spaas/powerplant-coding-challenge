using System.Text.Json.Serialization;
using PowerPlantCC.Contracts.ProductionPlan.Enums;

namespace PowerPlantCC.Contracts.ProductionPlan.Models;

public record PowerPlantDTO(
    string Name,
    [property: JsonConverter(typeof(JsonStringEnumConverter))] PowerPlantTypeDTO Type,
    decimal Efficiency,
    decimal Pmin,
    decimal Pmax
);
using System.Text.Json.Serialization;

namespace PowerPlantCC.Contracts.ProductionPlan.Models;

public record FuelsDTO(
    [property: JsonPropertyName("gas(euro/MWh)")] decimal Gas,
    [property: JsonPropertyName("kerosine(euro/MWh)")] decimal Kerosine,
    [property: JsonPropertyName("co2(euro/ton)")] decimal Co2,
    [property: JsonPropertyName("wind(%)")] decimal Wind
);
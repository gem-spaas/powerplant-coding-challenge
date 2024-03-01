using System.Text.Json.Serialization;

namespace GlobalEnergyManagement.Application.DTOs;

public record PowerPlantFuels(
    [property: JsonPropertyName("gas(euro/MWh)")]
    float GasEuroMegaWattHour,
    [property: JsonPropertyName("kerosine(euro/MWh)")]
    float KerosineEuroMegaWattHour,
    [property: JsonPropertyName("co2(euro/ton)")]
    float CarbonDioxideEuroTon,
    [property: JsonPropertyName("wind(%)")]
    int WindPercentage);
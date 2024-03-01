using System.Text.Json.Serialization;

namespace GlobalEnergyManagement.Application.DTOs;

public record PowerPlantFuels(
    [property: JsonPropertyName("gas(euro/MWh)")]
    double GasEuroMegaWattHour,
    [property: JsonPropertyName("kerosine(euro/MWh)")]
    double KerosineEuroMegaWattHour,
    [property: JsonPropertyName("co2(euro/ton)")]
    double CarbonDioxideEuroTon,
    [property: JsonPropertyName("wind(%)")]
    int WindPercentage);
using System.Text.Json.Serialization;

namespace PowerPlant.Models;

public record Fuels(
    [property: JsonPropertyName("gas(euro/MWh)")] float GaseuroMWh, 
    [property: JsonPropertyName("kerosine(euro/MWh)")] float KerosineeuroMWh,
    [property: JsonPropertyName("co2(euro/ton)")] float Co2Euroton,
    [property: JsonPropertyName("wind(%)")] float Wind
    );

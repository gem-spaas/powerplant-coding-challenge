using System.Text.Json.Serialization;

namespace PowerPlant.Api.Models;

public record FuelsDTO
{
    [JsonPropertyName("gas(euro/MWh)")]
    public decimal GasPricePerMWh { get; set; }
    
    [JsonPropertyName("kerosine(euro/MWh)")]
    public decimal KerosinePricePerMWh { get; set; }
    
    [JsonPropertyName("co2(euro/ton)")]
    public decimal Co2PricePerTon { get; set; }
    
    [JsonPropertyName("wind(%)")]
    public decimal WindPercentage { get; set; }
}
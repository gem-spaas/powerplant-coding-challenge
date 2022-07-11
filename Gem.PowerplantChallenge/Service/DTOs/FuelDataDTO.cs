using System.Text.Json.Serialization;

namespace Gem.PowerplantChallenge.Service.DTOs;

public class FuelDataDTO
{
    [JsonPropertyName("gas(euro/MWh)")]
    public double GasPrice { get; set; }

    [JsonPropertyName("kerosine(euro/MWh)")]
    public double KerosinePrice { get; set; }

    [JsonPropertyName("co2(euro/ton)")]
    public double CO2EmissionPrice { get; set; }

    [JsonPropertyName("wind(%)")]
    public double WindPercentage { get; set; }
}
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PowerplantCodingChallenge.Application.DTOs;

/// <summary>
/// Market data: fuel prices and environmental conditions
/// </summary>
public class MarketDataDto
{
    /// <summary>
    /// Gas price in euro per MWh
    /// </summary>
    [JsonPropertyName("gas(euro/MWh)")]
    [Range(0, double.MaxValue)]
    public double GasPrice { get; set; }

    /// <summary>
    /// Kerosine price in euro per MWh
    /// </summary>
    [JsonPropertyName("kerosine(euro/MWh)")]
    [Range(0, double.MaxValue)]
    public double KerosinePrice { get; set; }

    /// <summary>
    /// CO2 emission allowance price in euro per ton
    /// </summary>
    [JsonPropertyName("co2(euro/ton)")]
    [Range(0, double.MaxValue)]
    public double Co2Price { get; set; }

    /// <summary>
    /// Wind percentage (0-100%). E.g. 60 means 60% wind available
    /// </summary>
    [JsonPropertyName("wind(%)")]
    [Range(0, 100)]
    public double WindPercentage { get; set; }
}

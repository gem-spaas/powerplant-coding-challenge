using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PowerplantCodingChallenge.Application.DTOs;

/// <summary>
/// Request to calculate the production plan
/// </summary>
public class ProductionPlanRequest
{
    /// <summary>
    /// Load (in MWh) to be satisfied during one hour
    /// </summary>
    [JsonPropertyName("load")]
    [Range(0, double.MaxValue)]
    public double Load { get; set; }

    /// <summary>
    /// Market data: fuel prices and environmental conditions
    /// </summary>
    [JsonPropertyName("fuels")]
    [Required]
    public MarketDataDto Fuels { get; set; } = new();

    /// <summary>
    /// List of available powerplants
    /// </summary>
    [JsonPropertyName("powerplants")]
    [Required]
    public List<PowerPlantDto> Powerplants { get; set; } = [];
}

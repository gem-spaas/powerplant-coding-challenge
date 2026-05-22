using PowerplantCodingChallenge.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PowerplantCodingChallenge.Application.DTOs;

/// <summary>
/// Represents a powerplant with its technical characteristics
/// </summary>
public class PowerPlantDto
{
    /// <summary>
    /// Name of the powerplant
    /// </summary>
    [JsonPropertyName("name")]
    [Required]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Powerplant type: gasfired, turbojet or windturbine
    /// </summary>
    [JsonPropertyName("type")]
    public PowerPlantType Type { get; set; }

    /// <summary>
    /// Conversion efficiency (between 0 and 1). E.g. 0.53 means 53% efficiency
    /// </summary>
    [JsonPropertyName("efficiency")]
    [Range(0, 1)]
    public double Efficiency { get; set; }

    /// <summary>
    /// Minimum power (in MW) the powerplant can produce when switched on
    /// </summary>
    [JsonPropertyName("pmin")]
    [Range(0, double.MaxValue)]
    public double Pmin { get; set; }

    /// <summary>
    /// Maximum power (in MW) the powerplant can produce
    /// </summary>
    [JsonPropertyName("pmax")]
    [Range(0, double.MaxValue)]
    public double Pmax { get; set; }
}

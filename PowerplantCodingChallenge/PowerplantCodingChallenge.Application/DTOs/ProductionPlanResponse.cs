using System.Text.Json.Serialization;

namespace PowerplantCodingChallenge.Application.DTOs;

/// <summary>
/// Response indicating the power to be produced by a given powerplant
/// </summary>
public class ProductionPlanResponse
{
    /// <summary>
    /// Name of the powerplant
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Power (in MW) to be produced by this powerplant. Value rounded to 0.1 MW.
    /// </summary>
    [JsonPropertyName("p")]
    public double P { get; set; }
}

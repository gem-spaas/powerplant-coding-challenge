using PowerplantCodingChallenge.Domain.Enums;

namespace PowerplantCodingChallenge.Application.Models;

/// <summary>
/// Represents a powerplant with its calculated production cost
/// </summary>
public class PowerPlantCost
{
    public required string Name { get; set; }
    public PowerPlantType Type { get; set; }
    public double Efficiency { get; set; }
    public double Pmin { get; set; }
    public double Pmax { get; set; }

    /// <summary>
    /// Production cost per MWh (€/MWh)
    /// </summary>
    public double CostPerMwh { get; set; }

    /// <summary>
    /// Effective maximum power (for wind turbines: Pmax * wind%)
    /// </summary>
    public double EffectivePmax { get; set; }
}

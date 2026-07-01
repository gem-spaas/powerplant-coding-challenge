using power_plant_coding_challenge_domain.Enums;

namespace power_plant_coding_challenge_domain.Models;

public class Powerplant
{
    public required string Name { get; set; }
    public PowerplantType Type { get; set; }
    public decimal Efficiency { get; set; }
    public int Pmin { get; set; }
    public int Pmax { get; set; }

    /// <summary>
    /// returns the actual Pmax of a powerplant based on its type and the fuels available. For wind turbines, it takes into account the wind percentage to calculate the actual Pmax.
    /// </summary>
    /// <returns>The actual Pmax for the given powerplant and fuels.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the PowerplantType is unknown</exception>
    public decimal GetActualPMax(Fuel fuels)
    {
        switch (Type)
        {
            case PowerplantType.Windturbine:
                var windEfficiencyPercentage = fuels.Wind / 100.0m;
                var result = Pmax * windEfficiencyPercentage;
                return Math.Round(result, 1, MidpointRounding.AwayFromZero);
            case PowerplantType.Gasfired:
                return Pmax;
            case PowerplantType.Turbojet:
                return Pmax;
            default:
                throw new ArgumentOutOfRangeException($"Unknown powerplant type: {Type}");
        }
    }

    /// <summary>
    /// returns the cost efficiency of a powerplant based on its type, dividing the fuel price by the powerplant efficiency.
    /// </summary>
    /// <returns>An efficiency measure under a double. the more the value tends to 0, the more efficient the powerplant is.</returns>
    /// <param name="includeCo2Costs">If true, the CO2 cost will be added to the gas cost for gas-fired powerplants.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the PowerplantType is unknown</exception>
    public decimal GetCostEfficiency(Fuel fuels, bool includeCo2Costs = false)
    {
        switch (Type)
        {
            case PowerplantType.Windturbine:
                return 0;
            case PowerplantType.Gasfired:
                var co2Cost = includeCo2Costs ? 0.3m * fuels.Co2 : 0m;
                return (fuels.Gas / Efficiency) + co2Cost;
            case PowerplantType.Turbojet:
                return fuels.Kerosine / Efficiency;
            default:
                throw new ArgumentOutOfRangeException($"Unknown powerplant type: {Type}");
        }
    }
}

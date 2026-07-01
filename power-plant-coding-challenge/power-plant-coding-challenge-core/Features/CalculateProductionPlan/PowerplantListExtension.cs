using power_plant_coding_challenge_domain.Enums;
using power_plant_coding_challenge_domain.Models;

namespace power_plant_coding_challenge_core.Features.CalculateProductionPlan;

public static class PowerplantListExtension
{
    /// <summary>
    /// Gets a list of usable powerplants from a given list that can meet the required load.
    /// </summary>
    /// <returns>A list of powerplants that succeeds meeting the required load. Null if no combination of powerplants can meet the required load.</returns>
    public static List<CalculateProductionPlanResult>? GetUsablePowerplantsFromList(this List<Powerplant> powerplants, Fuel fuels, decimal load)
    {
        var selectedPowerplants = powerplants.CreatePowerplantListWhereLoadInPRange(fuels, load);
        if (selectedPowerplants.Count == 0) return null;

        var result = selectedPowerplants.CreateResultWithPMinOnly(); //This ensures that we respect the Pmin constraints of the selected powerplants.
        load -= result.Sum(result => result.P);
        result.DistributeLoadAcrossPowerplants(selectedPowerplants, fuels, load);
        return result;
    }

    /// <summary>
    /// Creates a list of CalculateProductionPlanResult from a list of powerplants, initializing each with 0.
    /// </summary>
    /// <returns>A list of CalculateProductionPlanResult with all values initialized to 0.</returns>
    public static List<CalculateProductionPlanResult> CreateResultWith0P(this List<Powerplant> powerplants)
    {
        var emptyResult = new List<CalculateProductionPlanResult>();
        emptyResult.AddRange(powerplants.Select(powerplant => new CalculateProductionPlanResult(powerplant.Name, 0m)));
        return emptyResult;
    }

    /// <summary>
    /// Orders a list of powerplants by their cost efficiency, and then by their Pmax value in descending order.
    /// </summary>
    /// <returns>A list of powerplants ordered by cost efficiency and Pmax.</returns>
    public static List<Powerplant> MeritOrder(this List<Powerplant> powerplants, Fuel fuels, bool includeCo2Costs = false) =>
        powerplants.OrderBy(powerplant => powerplant.GetCostEfficiency(fuels, includeCo2Costs))
            .ThenByDescending(powerplant => powerplant.Pmax)
            .ToList();

    /// <summary>
    /// Splits a list of powerplants into two lists: one containing only the powerplants of the specified type, and the other containing all other powerplants.
    /// </summary>
    public static void SplitByPowerplantType(this List<Powerplant> powerplants, PowerplantType type, out List<Powerplant> powerplantsOfType, out List<Powerplant> powerplantsNotOfType)
    {
        powerplantsOfType = powerplants
            .Where(powerplant => powerplant.Type == type)
            .ToList();

        powerplantsNotOfType = powerplants
            .Where(powerplant => powerplant.Type != type)
            .ToList();
    }

    /// <summary>
    /// Creates a list of CalculateProductionPlanResult from a list powerplants, initializing each with its Pmin value.
    /// </summary>
    /// <returns>A list of CalculateProductionPlanResult that contains only the PMin values for the selected powerplants.</returns>
    private static List<CalculateProductionPlanResult> CreateResultWithPMinOnly(this List<Powerplant> powerplants)
    {
        var result = new List<CalculateProductionPlanResult>();
        result.AddRange(powerplants.Select(powerplant => new CalculateProductionPlanResult(powerplant.Name, powerplant.Pmin)));
        return result;
    }

    /// <summary>
    /// Creates a list of powerplants that can meet the required load, while respecting the Pmin and Pmax constraints.
    /// </summary>
    /// <returns>A list of powerplants that can meet the required load, while respecting the Pmin and Pmax constraints.</returns>
    private static List<Powerplant> CreatePowerplantListWhereLoadInPRange(this List<Powerplant> powerplants, Fuel fuels, decimal load)
    {        
        var subset = new List<Powerplant>();

        foreach(var powerplant in powerplants)
        {
            var isAddingAtLeastPMinForCurrentPowerplantWillExceedLoad = 
                subset.Sum(subsetPowerplant => subsetPowerplant.Pmin) + powerplant.Pmin > load;

            if (powerplant.GetActualPMax(fuels) <= 0 || isAddingAtLeastPMinForCurrentPowerplantWillExceedLoad) continue;

            subset.Add(powerplant);

            var totalPmin = subset.Sum(subsetPowerplant => subsetPowerplant.Pmin);
            var totalPmax = subset.Sum(subsetPowerplant => subsetPowerplant.GetActualPMax(fuels));

            if (load >= totalPmin && load <= totalPmax) return subset;
        }

        return new List<Powerplant>();
    }
}

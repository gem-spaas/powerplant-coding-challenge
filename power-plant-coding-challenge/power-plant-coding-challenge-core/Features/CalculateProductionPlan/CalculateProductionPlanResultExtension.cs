using power_plant_coding_challenge_domain.Enums;
using power_plant_coding_challenge_domain.Models;

namespace power_plant_coding_challenge_core.Features.CalculateProductionPlan;

public static class CalculateProductionPlanResultExtension
{
    /// <summary>
    /// Order the results to match the order of the powerplants in the merit-order list.
    /// </summary>
    public static List<CalculateProductionPlanResult> OrderResultsByMeritOrder(this List<CalculateProductionPlanResult> results, List<Powerplant> powerplants, bool useWind, Fuel fuels)
    {
        var orderedResults = new List<CalculateProductionPlanResult>();

        foreach (var powerplant in powerplants)
        {
            if (powerplant.Type == PowerplantType.Windturbine)
            {
                var windProduction = useWind ? powerplant.GetActualPMax(fuels) : 0m;
                orderedResults.Add(new CalculateProductionPlanResult(powerplant.Name, windProduction));
            }
            else
            {
                var existingResult = results.FirstOrDefault(result => result.Name == powerplant.Name);
                orderedResults.Add(existingResult ?? new CalculateProductionPlanResult(powerplant.Name, 0m));
            }
        }

        return orderedResults;
    }

    /// <summary>
    /// Computes the total cost of the selected powerplants for a given dispatch based on their production and fuel costs.
    /// </summary>
    /// <returns>The total cost for the selected powerplants for a given dispatch contained in the results list</returns>
    public static decimal ComputeCost(this List<CalculateProductionPlanResult> results, List<Powerplant> powerplants, Fuel fuels, bool includeCo2Costs = false)
    {
        var totalCost = 0m;

        foreach (var result in results)
        {
            var powerplant = powerplants.First(powerplant => powerplant.Name == result.Name);
            totalCost += result.P * powerplant.GetCostEfficiency(fuels, includeCo2Costs);
        }

        return totalCost;
    }

    /// <summary>
    /// Distributes the remaining load among the selected powerplants, starting with the most efficient ones.
    /// </summary>
    public static void DistributeLoadAcrossPowerplants(this List<CalculateProductionPlanResult> results, List<Powerplant> powerplants, Fuel fuels, decimal load)
    {
        foreach(var powerplant in powerplants)
        {
            if(load == 0) break;

            if (!results.Any(result => result.Name == powerplant.Name)) continue;

            var reelPMax = powerplant.GetActualPMax(fuels);
            var remaingPAvailaibleForCurrentPowerplant = reelPMax - powerplant.Pmin;

            if(remaingPAvailaibleForCurrentPowerplant <= load)
            {
                var resultIndex = results.FindIndex(result => result.Name == powerplant.Name);
                results[resultIndex] = results[resultIndex] with { P = reelPMax };

                load -= remaingPAvailaibleForCurrentPowerplant;
            }
            else
            {
                var resultIndex = results.FindIndex(result => result.Name == powerplant.Name);
                results[resultIndex] = results[resultIndex] with { P = results[resultIndex].P + load };
                load = 0;
            }
        }
    }

    /// <summary>
    /// Compares two production plan results and returns the one with the lowest cost, while also indicating whether wind powerplants were used in the selected result.
    /// </summary>
    /// <returns>The result with the lowest cost</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static List<CalculateProductionPlanResult> CompareResultCostWith(this List<CalculateProductionPlanResult>? firstResult, List<CalculateProductionPlanResult>? secondResult, List<Powerplant> powerplants, Fuel fuels, bool includeCo2Costs = false)
    {
        if (firstResult is not null && secondResult is not null)
        {
            var costForFirstResult = firstResult.ComputeCost(powerplants, fuels, includeCo2Costs);
            var CostForSecondResult = secondResult.ComputeCost(powerplants, fuels, includeCo2Costs);

            if (costForFirstResult <= CostForSecondResult) return firstResult;
            else return secondResult;
        }

        if (firstResult is not null) return firstResult;

        if (secondResult is not null) return secondResult;
        else throw new InvalidOperationException("No combination of powerplants can meet the required load.");
    }
}

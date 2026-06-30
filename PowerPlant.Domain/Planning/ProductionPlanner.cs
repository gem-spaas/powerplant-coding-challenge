using PowerPlant.Domain.Exceptions;
using PowerPlant.Domain.PowerPlants;

namespace PowerPlant.Domain.Planning;

public sealed class ProductionPlanner : IProductionPlanner
{
    public IReadOnlyList<PowerOutput> Plan(decimal load, IReadOnlyList<PowerPlants.PowerPlant> plants, Fuels fuels)
    {
        var cheapestPlan = FindCheapestFeasiblePlan(load, plants, fuels);

        if (cheapestPlan is null)
            throw new InfeasibleLoadException(load);

        var alignedOutputs = AlignToLoad(cheapestPlan, load, fuels);

        return plants
            .Select(plant => new PowerOutput(plant.Name, alignedOutputs.GetValueOrDefault(plant, 0m)))
            .ToList();
    }

    private static DispatchResult? FindCheapestFeasiblePlan(
        decimal load, IReadOnlyList<PowerPlants.PowerPlant> plants, Fuels fuels)
    {
        DispatchResult? cheapest = null;

        foreach (var runningPlants in AllOnOffCombinations(plants))
        {
            var dispatch = TryDispatch(runningPlants, load, fuels);

            if (dispatch is not null && (cheapest is null || dispatch.TotalCost < cheapest.TotalCost))
                cheapest = dispatch;
        }

        return cheapest;
    }

    private static IReadOnlyList<IReadOnlyList<PowerPlants.PowerPlant>> AllOnOffCombinations(
        IReadOnlyList<PowerPlants.PowerPlant> plants)
    {
        var combinations = new List<List<PowerPlants.PowerPlant>> { new() };

        foreach (var plant in plants)
        {
            var withPlantSwitchedOn = combinations
                .Select(existing => new List<PowerPlants.PowerPlant>(existing) { plant })
                .ToList();

            combinations.AddRange(withPlantSwitchedOn);
        }

        return combinations;
    }

    private static DispatchResult? TryDispatch(
        IReadOnlyList<PowerPlants.PowerPlant> runningPlants, decimal load, Fuels fuels)
    {
        var forcedMinimum = runningPlants.Sum(plant => plant.EffectiveMinimum(fuels));
        var availableCapacity = runningPlants.Sum(plant => plant.EffectiveMaximum(fuels));

        if (forcedMinimum > load || availableCapacity < load)
            return null;

        var outputs = runningPlants.ToDictionary(plant => plant, plant => plant.EffectiveMinimum(fuels));
        var remaining = load - forcedMinimum;

        foreach (var plant in runningPlants.OrderBy(plant => plant.CostPerMwh(fuels)))
        {
            if (remaining <= 0m)
                break;

            var room = plant.EffectiveMaximum(fuels) - outputs[plant];
            var added = Math.Min(remaining, room);
            outputs[plant] += added;
            remaining -= added;
        }

        var totalCost = outputs.Sum(entry => entry.Value * entry.Key.CostPerMwh(fuels));

        return new DispatchResult(outputs, totalCost);
    }

    private static Dictionary<PowerPlants.PowerPlant, decimal> AlignToLoad(
        DispatchResult dispatch, decimal load, Fuels fuels)
    {
        var rounded = dispatch.Outputs.ToDictionary(
            entry => entry.Key,
            entry => RoundToNearestTenth(entry.Value));

        var gap = load - rounded.Values.Sum();

        if (gap > 0m)
            FillUp(rounded, gap, fuels);
        else if (gap < 0m)
            TrimDown(rounded, -gap, fuels);

        return rounded;
    }

    private static void FillUp(Dictionary<PowerPlants.PowerPlant, decimal> outputs, decimal amount, Fuels fuels)
    {
        foreach (var plant in outputs.Keys.OrderBy(plant => plant.CostPerMwh(fuels)).ToList())
        {
            if (amount <= 0m)
                break;

            var room = plant.EffectiveMaximum(fuels) - outputs[plant];
            var added = Math.Min(amount, room);
            outputs[plant] += added;
            amount -= added;
        }
    }

    private static void TrimDown(Dictionary<PowerPlants.PowerPlant, decimal> outputs, decimal amount, Fuels fuels)
    {
        foreach (var plant in outputs.Keys.OrderByDescending(plant => plant.CostPerMwh(fuels)).ToList())
        {
            if (amount <= 0m)
                break;

            var room = outputs[plant] - plant.EffectiveMinimum(fuels);
            var removed = Math.Min(amount, room);
            outputs[plant] -= removed;
            amount -= removed;
        }
    }

    private static decimal RoundToNearestTenth(decimal value)
        => Math.Round(value, 1, MidpointRounding.AwayFromZero);

    private sealed record DispatchResult(IReadOnlyDictionary<PowerPlants.PowerPlant, decimal> Outputs, decimal TotalCost);
}
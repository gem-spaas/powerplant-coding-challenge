using PowerplantCodingChallenge.Application.DTOs;
using PowerplantCodingChallenge.Application.Interfaces;
using PowerplantCodingChallenge.Application.Models;
using PowerplantCodingChallenge.Domain.Enums;

namespace PowerplantCodingChallenge.Application.Services;

public class ProductionPlanService : IProductionPlanService
{
    public List<ProductionPlanResponse> CalculateProductionPlan(ProductionPlanRequest request)
    {
        double load = request.Load;
        MarketDataDto fuels = request.Fuels;
        List<PowerPlantDto> powerPlants = request.Powerplants;

        List<PowerPlantCost> plantsWithCost = CalculatePlantCosts(powerPlants, fuels);

        List<PowerPlantCost> sortedPlants = plantsWithCost
            .OrderBy(p => p.CostPerMwh)
            .ThenByDescending(p => p.EffectivePmax)
            .ToList();

        Dictionary<string, double> allocations = AllocatePower(sortedPlants, load);

        return request.Powerplants
            .Select(p => new ProductionPlanResponse
            {
                Name = p.Name,
                P = RoundToOneTenth(allocations.GetValueOrDefault(p.Name, 0))
            })
            .ToList();
    }

    private List<PowerPlantCost> CalculatePlantCosts(List<PowerPlantDto> plants, MarketDataDto fuels)
    {
        return plants.Select(p =>
        {
            var (costPerMwh, effectivePmax) = p.Type switch
            {
                PowerPlantType.windturbine => (
                    0.0,
                    p.Pmax * (fuels.WindPercentage / 100.0)
                ),
                PowerPlantType.gasfired => (
                    fuels.GasPrice / p.Efficiency,
                    p.Pmax
                ),
                PowerPlantType.turbojet => (
                    fuels.KerosinePrice / p.Efficiency,
                    p.Pmax
                ),
                _ => throw new ArgumentException($"Unknown powerplant type: {p.Type}")
            };

            return new PowerPlantCost
            {
                Name = p.Name,
                Type = p.Type,
                Efficiency = p.Efficiency,
                Pmin = p.Pmin,
                Pmax = p.Pmax,
                CostPerMwh = costPerMwh,
                EffectivePmax = effectivePmax
            };
        }).ToList();
    }

    private Dictionary<string, double> AllocatePower(List<PowerPlantCost> sortedPlants, double load)
    {
        Dictionary<string, double> allocations = sortedPlants.ToDictionary(p => p.Name, _ => 0.0);
        double remainingLoad = load;

        foreach (var plant in sortedPlants)
        {
            if (remainingLoad <= 0) break;

            double effectivePmax = plant.EffectivePmax;
            double effectivePmin = plant.Pmin;

            if (effectivePmax <= 0) continue;

            if (remainingLoad < effectivePmin)
            {
                bool adjusted = TryAdjustPreviousAllocations(
                    sortedPlants, allocations, plant, remainingLoad);

                if (adjusted)
                {
                    allocations[plant.Name] = effectivePmin;
                    remainingLoad = 0;
                }
                continue;
            }
            
            var allocation = Math.Min(effectivePmax, remainingLoad);

            if (allocation < effectivePmin)
            {
                allocation = 0;
            }

            allocations[plant.Name] = allocation;
            remainingLoad -= allocation;
        }

        if (remainingLoad > 0.1)
        {
            throw new InvalidOperationException(
                $"Cannot satisfy load. Remaining: {remainingLoad} MWh");
        }

        return allocations;
    }

    private bool TryAdjustPreviousAllocations(
        List<PowerPlantCost> sortedPlants,
        Dictionary<string, double> allocations,
        PowerPlantCost currentPlant,
        double remainingLoad)
    {
        double deficit = currentPlant.Pmin - remainingLoad;

        for (int i = sortedPlants.Count - 1; i >= 0; i--)
        {
            var prevPlant = sortedPlants[i];
            if (prevPlant.Name == currentPlant.Name) continue;

            var currentAllocation = allocations[prevPlant.Name];
            if (currentAllocation <= 0) continue;

            double minAllocation = prevPlant.Pmin;
            double canReduce = currentAllocation - minAllocation;

            if (canReduce >= deficit)
            {
                allocations[prevPlant.Name] = currentAllocation - deficit;
                return true;
            }
            else if (canReduce > 0)
            {
                allocations[prevPlant.Name] = 0;
                deficit -= canReduce;
            }
        }

        return deficit <= 0.1;
    }

    private static double RoundToOneTenth(double value)
    {
        return Math.Round(value, 1, MidpointRounding.AwayFromZero);
    }
}

using Domain.Entities;
using Domain.Enums;
using Domain.Value_Objects;
using SharedKernel;

namespace Domain.Services
{
    public sealed class ProductionPlanCalculator
    {
        public Result<IReadOnlyCollection<PlantProduction>> Calculate(
       double load,
       IReadOnlyCollection<PowerPlant> powerPlants,
       Fuel fuels)
        {
            var productions = powerPlants.ToDictionary(
                plant => plant.Name,
                _ => 0.0);

            if (load == 0)
            {
                return Result<IReadOnlyCollection<PlantProduction>>.Success(
                    [.. powerPlants.Select(p => new PlantProduction(p.Name, 0))]);
            }

            var remainingLoad = load;

            // 1. Use available wind power first.
            foreach (var plant in powerPlants.Where(
                         p => p.Type == PlantType.WindTurbine))
            {
                var availablePower = plant.Pmax * fuels.Wind / 100.0;
                var power = Math.Min(availablePower, remainingLoad);

                productions[plant.Name] = power;
                remainingLoad -= power;
            }

            // 2. Dispatch thermal power plants by merit order.
            var orderedPlants = MeritOrder.Sort(
                powerPlants.Where(p => p.Type != PlantType.WindTurbine),
                fuels);

            foreach (var plant in orderedPlants)
            {
                if (remainingLoad <= 0)
                    break;

                // A plant must respect its minimum output when activated.
                if (remainingLoad < plant.Pmin)
                    continue;

                var power = Math.Min(plant.Pmax, remainingLoad);

                productions[plant.Name] = power;
                remainingLoad -= power;
            }

            // 3. Return a business failure if the demand cannot be met.
            if (remainingLoad > 0.000001)
            {
                return Result<IReadOnlyCollection<PlantProduction>>.Failure(
                    new Error(
                        "InsufficientCapacity",
                        $"Unable to satisfy the requested load. " +
                        $"{remainingLoad:F2} MW remain unserved."));
            }

            var result = powerPlants
                .Select(p => new PlantProduction(
                    p.Name,
                    productions[p.Name]))
                .ToList();

            return Result<IReadOnlyCollection<PlantProduction>>.Success(result);
        }
    }
}

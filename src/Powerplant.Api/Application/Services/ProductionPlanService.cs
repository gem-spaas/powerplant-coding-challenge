using Powerplant.Api.Application.Abstractions;
using Powerplant.Api.Domain;
using Powerplant.Api.Generated;

namespace Powerplant.Api.Application.Services
{
    public class ProductionPlanService : IProductionPlanService
    {
        public IReadOnlyCollection<ProductionPlanEntry> Calculate(
            decimal Load,
            FuelPrices fuelPrices,
            List<PowerPlant> powerPlants)
        {

            var result = new List<ProductionPlanEntry>();
            var remainingLoad = Load;

            var sortedPowerPlants = powerPlants
                .OrderBy(p => p.CostPerMWh(fuelPrices))
                .ToList();

            foreach (PowerPlant powerPlant in sortedPowerPlants)
            {
                var possibleLoad = powerPlant.GetPossibleLoad(remainingLoad, fuelPrices);
                result.Add(new ProductionPlanEntry
                {
                    Name = powerPlant.Name,
                    Power = possibleLoad
                });
                remainingLoad -= possibleLoad;

            }
            return result;

        }
        
    }
}

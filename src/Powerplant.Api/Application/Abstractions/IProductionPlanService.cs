using Powerplant.Api.Domain;
using Powerplant.Api.Generated;

namespace Powerplant.Api.Application.Abstractions
{
    public interface IProductionPlanService
    {
        IReadOnlyCollection<ProductionPlanEntry> Calculate(
            decimal Load,
            FuelPrices fuelPrices,
            List<PowerPlant> powerPlants);
    }
}

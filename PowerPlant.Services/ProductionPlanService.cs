using PowerPlant.Models;

namespace PowerPlant.Services;

public class ProductionPlanService : IProductionPlanService
{
    private readonly IPowerPlantService _powerPlantService;

    public ProductionPlanService(IPowerPlantService powerPlantService)
    {
        _powerPlantService = powerPlantService;
    }

    public  IEnumerable<ProductionPlan> GetProductionPlan(RequestedLoad requestedLoad)
    {
        var requestedLoadPowerplants = requestedLoad.Powerplants;

        var powerPlantOrderedByMerit = _powerPlantService.OrderByMerit(requestedLoadPowerplants, requestedLoad.Fuels);

        var productionPlan = CalculateProductionPlan(requestedLoad, powerPlantOrderedByMerit);

        return productionPlan;
    }

    private IEnumerable<ProductionPlan> CalculateProductionPlan(RequestedLoad requestedLoad, IEnumerable<Powerplant> powerPlantOrderedByMerit)
    {
        var productionPlan = Enumerable.Empty<ProductionPlan>();
        var remainingLoad = requestedLoad.Load;
        foreach (var powerplant in powerPlantOrderedByMerit)
        {    
            if (productionPlan.Any(pp => pp.Name == powerplant.Name))
            {
                continue;
            }
            
            if (remainingLoad <= 0)
            {
                productionPlan = productionPlan.Append(new ProductionPlan(powerplant.Name, 0));
                continue;
            }

            if (remainingLoad <= powerplant.Pmin)
            {
                productionPlan = productionPlan.Append(new ProductionPlan(powerplant.Name, 0));
                var closestPowerPlant= GetClosestPowerPlant(powerPlantOrderedByMerit ,remainingLoad);
                productionPlan = productionPlan.Append(new ProductionPlan(closestPowerPlant.Name, remainingLoad));
                remainingLoad = 0;
                continue;
            }

            var powerplantProduction = CalculatePowerplantProduction(powerplant, requestedLoad.Fuels, remainingLoad);

            productionPlan = productionPlan.Append(powerplantProduction);

            remainingLoad -= powerplantProduction.P;
        }
        return productionPlan;
    }

    private ProductionPlan CalculatePowerplantProduction(Powerplant powerplant, Fuels requestedLoadFuels, double remainingLoad)
    {
        var correctedPmax =
            powerplant.Type == PowerPlantType.windturbine.ToString()
                ? (powerplant.PMax * requestedLoadFuels.Wind) / 100
                : powerplant.PMax;
            
        if (remainingLoad <= correctedPmax)
        {
            return new ProductionPlan(powerplant.Name, remainingLoad);
        }

        var p = Math.Min(correctedPmax, Math.Max(powerplant.Pmin, remainingLoad));

        return new ProductionPlan(powerplant.Name, Math.Round(p, 1));
    }

    public Powerplant GetClosestPowerPlant(IEnumerable<Powerplant> powerplants, double remainingLoad)
    {
        var filteredPowerPlants = powerplants.Where(pp => pp.Pmin >= remainingLoad).ToList();

        var closestPowerplant = filteredPowerPlants.First();
    
        var minDifference = Math.Abs(closestPowerplant.Pmin - remainingLoad);

        foreach (var powerplant in filteredPowerPlants)
        {
            var difference = Math.Abs(powerplant.Pmin - remainingLoad);

            if (difference < minDifference)
            {
                minDifference = difference;
                closestPowerplant = powerplant;
            }
        }

        return closestPowerplant;
    }
}
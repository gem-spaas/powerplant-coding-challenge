using PowerPlant.Models;

namespace PowerPlant.Services;

public class ProductionPlanService : IProductionPlanService
{
    public async Task<IEnumerable<ProductionPlan>> GetProductionPlan(RequestedLoad requestedLoad)
    {
  
        var requestedLoadPowerplants = requestedLoad.Powerplants;

        var powerPlantOrderedByMerit =  OrderByMerit(requestedLoadPowerplants,requestedLoad.Fuels);
    
        var productionPlan = CalculateProductionPlan(requestedLoad, powerPlantOrderedByMerit);

        return productionPlan;
    }

    private IEnumerable<ProductionPlan> CalculateProductionPlan(RequestedLoad requestedLoad, IEnumerable<Powerplant> powerPlantOrderedByMerit)
    {
        var productionPlan =  Enumerable.Empty<ProductionPlan>();
        var remainingLoad = requestedLoad.Load;
        foreach (var powerplant in powerPlantOrderedByMerit)
        {
            if (remainingLoad <= 0)
            {
                productionPlan = productionPlan.Append(new ProductionPlan(powerplant.Name, 0));
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

        var p = Math.Min((correctedPmax), Math.Max((powerplant.Pmin), remainingLoad));
        return new ProductionPlan(powerplant.Name, Math.Round(p,1));
    }

    internal IEnumerable<Powerplant> OrderByMerit(IEnumerable<Powerplant> requestedLoadPowerplants, Fuels requestedFuels)
        => requestedLoadPowerplants
                .OrderByDescending(x => x.Efficiency);

}
using GlobalEnergyManagement.Application.Contracts;
using GlobalEnergyManagement.Application.DTOs;

namespace GlobalEnergyManagement.Application.Services;

public class PowerPlantService : IPowerPlantService
{
    public async Task<ICollection<PowerPlantPower>> CalculateProductionPlan(PowerPlantPayload payload, bool includeCarbonOxide = false)
    {

        var orderedPowerPlants = CalculateCostsForPowerPlants(payload, includeCarbonOxide);

        var productionPlan = CreateProductionPlanFromCosts(payload.Load, payload.Fuels.WindPercentage, orderedPowerPlants);

        return await Task.FromResult(productionPlan);
    }
    
    private List<PowerPlants> CalculateCostsForPowerPlants(PowerPlantPayload payload, bool includeCarbonOxide = false)
    {
        foreach (var plant in payload.PowerPlants)
        {
            switch (plant.Type)
            {
                case "gasfired":
                    var carbonOxideCostPerMegaWatt = 0.3 * payload.Fuels.CarbonDioxideEuroTon;
                    
                    plant.Cost = (payload.Fuels.GasEuroMegaWattHour / plant.Efficiency + (includeCarbonOxide ? carbonOxideCostPerMegaWatt : 0) );
                    break;
                
                case "turbojet":
                    plant.Cost = payload.Fuels.KerosineEuroMegaWattHour / plant.Efficiency;
                    break;
                
                case "windturbine":
                    plant.Cost = 0;
                    break;
            }
        }

        return payload.PowerPlants.OrderBy(pp => pp.Cost).ToList();
    }
    
    private List<PowerPlantPower> CreateProductionPlanFromCosts(int load, int windPercentage, List<PowerPlants> orderedPowerPlants)
    {
        var productionPlan = new List<PowerPlantPower>();
        
        double remainingLoad = load;
        
        // Calculate Power Max for wind turbines
        
        foreach (var windPlant in orderedPowerPlants.Where(d => d.Type == "windturbine"))
        {
            windPlant.PowerMax = windPlant.PowerMax * windPercentage / 100.0;
        }

        foreach (var powerPlant in orderedPowerPlants)
        {
            var powerToAllocate = Math.Min(powerPlant.PowerMax, remainingLoad);
            
            remainingLoad -= powerToAllocate;
            
            productionPlan.Add(new PowerPlantPower(powerPlant.Name, powerToAllocate, powerPlant.Cost));
        }
        
        return productionPlan;
    }

}
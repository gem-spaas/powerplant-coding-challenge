using PowerPlant.Domain.Constants;
using PowerPlant.Domain.Entities;
using PowerPlant.Domain.Enums;
using PowerPlant.Domain.Interfaces;

namespace PowerPlant.Domain.Services;

public class CostComputationService : ICostComputationService
{
    public ProductionPlan ComputeCost(ProductionPlan productionPlan)
    {
        foreach (var powerPlant in productionPlan.PowerPlants)
        {
            switch (powerPlant.Type)
            {
                case PowerPlantType.windturbine:
                    powerPlant.Cost = 0;
                    powerPlant.Pmax = (powerPlant.Pmax / 100) * productionPlan.Fuels.WindPercentage;
                    break;
                case PowerPlantType.gasfired:
                    var fuelCost = ComputeFuelCost(productionPlan.Fuels.GasPricePerMWh, powerPlant.Efficiency);
                    var co2Cost = ComputeCo2Cost(productionPlan.Fuels.Co2PricePerTon, powerPlant.Efficiency,  CO2Cost.GasFired);
                    powerPlant.Cost = fuelCost + co2Cost;
                    break;
                case PowerPlantType.turbojet:
                    powerPlant.Cost = ComputeFuelCost(productionPlan.Fuels.KerosinePricePerMWh, powerPlant.Efficiency);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        return productionPlan;
    }
    
    private static decimal ComputeFuelCost(
        decimal fuelPricePerMWh, 
        decimal efficiency)
    {
        return fuelPricePerMWh / efficiency;
    }
    
    private static decimal ComputeCo2Cost(
        decimal co2PricePerTon,
        decimal efficiency,
        decimal numberOfCo2TonsGeneratedPerTon)
    {
        return (co2PricePerTon / efficiency) * numberOfCo2TonsGeneratedPerTon;
    }
}
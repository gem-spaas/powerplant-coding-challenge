using PowerPlant.Domain.Entities;
using PowerPlant.Domain.Enums;

namespace PowerPlant.Domain.UnitTests.Common;

public static class ProductionPlanTests
{
    public static Entities.ProductionPlan GetDefaultPowerPlanInput()
    {
        return new Entities.ProductionPlan
        {
            Load = 910M,
            Fuels = new Fuels
            {
                GasPricePerMWh = 13.4M,
                KerosinePricePerMWh = 50.8M,
                Co2PricePerTon = 20M,
                WindPercentage = 60
            },
            PowerPlants = new List<Domain.Entities.PowerPlant>()
            {
                new()
                {
                    Name = "gasfiredbig1", 
                    Type = PowerPlantType.gasfired, 
                    Efficiency = 0.53M, 
                    Pmin = 100M, 
                    Pmax = 460M
                },
                new()
                {
                    Name = "gasfiredbig2", 
                    Type = PowerPlantType.gasfired, 
                    Efficiency = 0.53M, 
                    Pmin = 100M, 
                    Pmax = 460M
                },
                new()
                {
                    Name = "gasfiredsomewhatsmaller", 
                    Type = PowerPlantType.gasfired, 
                    Efficiency = 0.37M, 
                    Pmin = 40M, 
                    Pmax = 210M
                },
                new()
                {
                    Name = "tj1", 
                    Type = PowerPlantType.turbojet, 
                    Efficiency = 0.3M, 
                    Pmin = 0M, 
                    Pmax = 16M
                },
                new()
                {
                    Name = "windpark1", 
                    Type = PowerPlantType.windturbine, 
                    Efficiency = 1M, 
                    Pmin = 0M, 
                    Pmax = 150
                },
                new()
                {
                    Name = "windpark2", 
                    Type = PowerPlantType.windturbine, 
                    Efficiency = 1M, 
                    Pmin = 0M, 
                    Pmax = 36M
                }
            }
        };
    }
}
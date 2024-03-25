
using PowerPlantCC.Domain.Models;
using PowerPlantCC.Domain.Enums;
using FluentAssertions;

namespace PowerPlantCC.Domain.UnitTests.Models;

public class ProductionPlanServiceTests
{
    [Test]
    public void BuildProductionPlan_Should_ReturnCorrectOutput()
    {
        // Arrange

        ProductionPlan productionPlan = new()
        {
            Load = 910,
            Fuels = new Fuels
            {
                Gas = 13.4M,
                Kerosine = 50.8M,
                Co2 = 20,
                Wind = 60
            },
            PowerPlants =
            [
                new() { Name = "gasfiredbig1", Type = PowerPlantType.gasfired, Efficiency = 0.53M, Pmin = 100, Pmax = 460 } ,
                new() { Name = "gasfiredbig2", Type = PowerPlantType.gasfired, Efficiency = 0.53M, Pmin = 100, Pmax = 460 },
                new() { Name = "gasfiredsomewhatsmaller", Type = PowerPlantType.gasfired, Efficiency = 0.37M, Pmin = 40, Pmax = 210 },
                new() { Name = "tj1", Type = PowerPlantType.turbojet, Efficiency = 0.3M, Pmin = 0, Pmax = 16 },
                new() { Name = "windpark1", Type = PowerPlantType.windturbine, Efficiency = 1, Pmin = 0, Pmax = 150 },
                new() { Name = "windpark2", Type = PowerPlantType.windturbine, Efficiency = 1, Pmin = 0, Pmax = 36 }
            ]
        };

        PowerPlant[] expectedOutput =
            [
                new() { Name = "windpark1", Type = PowerPlantType.windturbine, Efficiency = 1, Pmin = 0, Pmax = 150, Power = 90.0M },
                new() { Name = "windpark2", Type = PowerPlantType.windturbine, Efficiency = 1, Pmin = 0, Pmax = 36, Power = 21.6M },
                new() { Name = "gasfiredbig1", Type = PowerPlantType.gasfired, Efficiency = 0.53M, Pmin = 100, Pmax = 460, Power = 460.0M } ,
                new() { Name = "gasfiredbig2", Type = PowerPlantType.gasfired, Efficiency = 0.53M, Pmin = 100, Pmax = 460, Power = 338.4M },
                new() { Name = "gasfiredsomewhatsmaller", Type = PowerPlantType.gasfired, Efficiency = 0.37M, Pmin = 40, Pmax = 210, Power = 0.0M },
                new() { Name = "tj1", Type = PowerPlantType.turbojet, Efficiency = 0.3M, Pmin = 0, Pmax = 16, Power = 0.0M },
            ];

        // Act

        productionPlan.Build();

        // Assert

        for (int i = 0; i < productionPlan.PowerPlants.Length; i++)
        {
            PowerPlant powerPlant = productionPlan.PowerPlants[i];
            PowerPlant expectedPowerPlant = expectedOutput[i];

            powerPlant.Power.Should().Be(expectedPowerPlant.Power);
        }
    }

    [Test]
    public void BuildProductionPlanWithPmin_Should_ReturnCorrectOutput()
    {
        // Arrange

        ProductionPlan productionPlan = new()
        {
            Load = 6,
            Fuels = new Fuels
            {
                Gas = 13.4M,
                Kerosine = 50.8M,
                Co2 = 20,
                Wind = 60
            },
            PowerPlants =
            [
                new() { Name = "gasfiredbig1", Type = PowerPlantType.gasfired, Efficiency = 1, Pmin = 2, Pmax = 4} ,
                new() { Name = "gasfiredbig2", Type = PowerPlantType.gasfired, Efficiency = 1, Pmin = 4, Pmax = 8 },
            ]
        };

        PowerPlant[] expectedOutput =
        [
            new() { Name = "gasfiredbig1", Type = PowerPlantType.gasfired, Efficiency = 1, Pmin = 2, Pmax = 4, Power = 2 } ,
            new() { Name = "gasfiredbig2", Type = PowerPlantType.gasfired, Efficiency = 1, Pmin = 4, Pmax = 8, Power = 4 },
        ];

        // Act

        productionPlan.Build();

        // Assert

        for (int i = 0; i < productionPlan.PowerPlants.Length; i++)
        {
            PowerPlant powerPlant = productionPlan.PowerPlants[i];
            PowerPlant expectedPowerPlant = expectedOutput[i];

            powerPlant.Power.Should().Be(expectedPowerPlant.Power);
        }
    }
}
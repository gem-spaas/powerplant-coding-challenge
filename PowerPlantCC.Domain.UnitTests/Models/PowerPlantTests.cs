
using PowerPlantCC.Domain.Models;
using PowerPlantCC.Domain.Enums;
using FluentAssertions;

namespace PowerPlantCC.Domain.UnitTests.Models;

public class PowerPlantExtensionsTests
{
    [Test]
    public void UseDeliverablePower_WherePowerRequiredBiggerThanPmax_ShouldReturnPmax()
    {
        // Arrange
        const decimal powerRequired = 500;
        const int expectedResponse = 460;

        PowerPlant powerPlant = new()
        {
            Name = "gasfiredbig1",
            Type = PowerPlantType.gasfired,
            Efficiency = 0.53M,
            Pmin = 100,
            Pmax = expectedResponse
        };

        // Act
        powerPlant.UseDeliverablePower(powerRequired);

        // Assert
        var result = powerPlant.Power;
        result.Should().Be(expectedResponse);
    }

    [Test]
    public void UseDeliverablePower_WherePowerRequiredLowerThanPmax_ShouldReturnPowerRequired()
    {
        // Arrange
        const decimal powerRequired = 400;

        PowerPlant powerPlant = new()
        {
            Name = "gasfiredbig1",
            Type = PowerPlantType.gasfired,
            Efficiency = 0.53M,
            Pmin = 100,
            Pmax = 460
        };

        // Act
        powerPlant.UseDeliverablePower(powerRequired);

        // Assert
        var result = powerPlant.Power;
        result.Should().Be(powerRequired);
    }

    [Test]
    public void UseDeliverablePower_WherePowerRequiredLowerThanPmin_ShouldReturnPmin()
    {
        // Arrange
        const decimal powerRequired = 50;
        const int expectedResponse = 100;

        PowerPlant powerPlant = new()
        {
            Name = "gasfiredbig1",
            Type = PowerPlantType.gasfired,
            Efficiency = 0.53M,
            Pmin = expectedResponse,
            Pmax = 460
        };

        // Act
        powerPlant.UseDeliverablePower(powerRequired);

        // Assert
        var result = powerPlant.Power;
        result.Should().Be(expectedResponse);
    }

    [Test]
    public void ReducePossiblePower_WherePowerToRemoveIsBiggerThanPossibleReduce_ShouldReturnPossibleReduce()
    {
        // Arrange
        const decimal powerToRemove = 250;
        const decimal expectedResponse = 200;
        const decimal basePower = 300;

        PowerPlant powerPlant = new()
        {
            Name = "gasfiredbig1",
            Type = PowerPlantType.gasfired,
            Efficiency = 0.53M,
            Pmin = 100,
            Pmax = 460,
            Power = basePower
        };

        // Act
        powerPlant.ReducePossiblePower(powerToRemove);

        // Assert
        var result = basePower - powerPlant.Power;
        result.Should().Be(expectedResponse);
    }

    [Test]
    public void ReducePossiblePower_WherePowerToRemoveIsLowerThanPossibleReduce_ShouldReturnPowerToRemove()
    {
        // Arrange
        const decimal powerToRemove = 100;
        const decimal expectedResponse = 100;
        const decimal basePower = 300;

        PowerPlant powerPlant = new()
        {
            Name = "gasfiredbig1",
            Type = PowerPlantType.gasfired,
            Efficiency = 0.53M,
            Pmin = 100,
            Pmax = 460,
            Power = basePower
        };

        // Act
        powerPlant.ReducePossiblePower(powerToRemove);

        // Assert
        var result = basePower - powerPlant.Power;
        result.Should().Be(expectedResponse);
    }

    [Test]
    public void UseGetPowerPlantCost_WherePowerPlantIsWind_ShouldReturnCorrectCost()
    {
        // Arrange 
        PowerPlant powerPlant = new()
        {
            Name = "windpark2",
            Type = PowerPlantType.windturbine,
            Efficiency = 1,
            Pmin = 0,
            Pmax = 36
        };

        Fuels fuels = new()
        {
            Gas = 13.4M,
            Kerosine = 50.8M,
            Co2 = 20,
            Wind = 60
        };

        decimal expectedResponse = 0;

        // Act

        var result = powerPlant.GetPowerPlantCost(fuels);

        // Assert 

        result.Should().Be(expectedResponse);
    }

    [Test]
    public void UseGetPowerPlantCost_WherePowerPlantIsGas_ShouldReturnCorrectCost()
    {
        // Arrange 
        PowerPlant powerPlant = new()
        {
            Name = "gasfiredbig1",
            Type = PowerPlantType.gasfired,
            Efficiency = 0.53M,
            Pmin = 100,
            Pmax = 460
        };

        Fuels fuels = new()
        {
            Gas = 13.4M,
            Kerosine = 50.8M,
            Co2 = 20,
            Wind = 60
        };

        decimal expectedResponse = (13.4M / 0.53M) + (20 * 0.3M);

        // Act

        var result = powerPlant.GetPowerPlantCost(fuels);

        // Assert 

        result.Should().Be(expectedResponse);
    }

    [Test]
    public void UseGetPowerPlantCost_WherePowerPlantIsTurboJet_ShouldReturnCorrectCost()
    {
        // Arrange 
        PowerPlant powerPlant = new()
        {
            Name = "tj1",
            Type = PowerPlantType.turbojet,
            Efficiency = 0.3M,
            Pmin = 0,
            Pmax = 16
        };

        Fuels fuels = new()
        {
            Gas = 13.4M,
            Kerosine = 50.8M,
            Co2 = 20,
            Wind = 60
        };

        decimal expectedResponse = 50.8M / 0.3M;

        // Act

        var result = powerPlant.GetPowerPlantCost(fuels);

        // Assert 

        result.Should().Be(expectedResponse);
    }

    [Test]
    public void UseGetPowerPlantCostToOrderPowerPlants_ShouldReturnOrderedPowerPlants()
    {
        // Arrange
        PowerPlant[] powerPlants = [
            new() { Name = "gasfiredbig1", Type = PowerPlantType.gasfired, Efficiency = 0.53M, Pmin = 100, Pmax = 460 } ,
            new() { Name = "gasfiredbig2", Type = PowerPlantType.gasfired, Efficiency = 0.53M, Pmin = 100, Pmax = 460 },
            new() { Name = "gasfiredsomewhatsmaller", Type = PowerPlantType.gasfired, Efficiency = 0.37M, Pmin = 40, Pmax = 210 },
            new() { Name = "tj1", Type = PowerPlantType.turbojet, Efficiency = 0.3M, Pmin = 0, Pmax = 16 },
            new() { Name = "windpark1", Type = PowerPlantType.windturbine, Efficiency = 1, Pmin = 0, Pmax = 150 },
            new() { Name = "windpark2", Type = PowerPlantType.windturbine, Efficiency = 1, Pmin = 0, Pmax = 36 }
        ];

        Fuels fuels = new()
        {
            Gas = 13.4M,
            Kerosine = 50.8M,
            Co2 = 20,
            Wind = 60
        };

        PowerPlant[] expectedOrderedPowerPlants = [
                new() { Name = "windpark1", Type = PowerPlantType.windturbine, Efficiency = 1, Pmin = 0, Pmax = 150 },
                new() { Name = "windpark2", Type = PowerPlantType.windturbine, Efficiency = 1, Pmin = 0, Pmax = 36 },
                new() { Name = "gasfiredbig1", Type = PowerPlantType.gasfired, Efficiency = 0.53M, Pmin = 100, Pmax = 460} ,
                new() { Name = "gasfiredbig2", Type = PowerPlantType.gasfired, Efficiency = 0.53M, Pmin = 100, Pmax = 460 },
                new() { Name = "gasfiredsomewhatsmaller", Type = PowerPlantType.gasfired, Efficiency = 0.37M, Pmin = 40, Pmax = 210 },
                new() { Name = "tj1", Type = PowerPlantType.turbojet, Efficiency = 0.3M, Pmin = 0, Pmax = 16 }
        ];

        // Act

        IEnumerable<PowerPlant> result = powerPlants.OrderBy(p => p.GetPowerPlantCost(fuels));

        // Assert

        result.Count().Should().Be(expectedOrderedPowerPlants.Length);

        for (int i = 0; i < expectedOrderedPowerPlants.Length; i++)
        {
            result.ElementAt(i).Name.Should().Be(expectedOrderedPowerPlants[i].Name);
        }
    }
}
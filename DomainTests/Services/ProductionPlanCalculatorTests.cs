
using Domain.Entities;
using Domain.Enums;
using Domain.Services;
using Domain.Value_Objects;

namespace Domain.Tests.Services;

public sealed class ProductionPlanCalculatorTests
{
    private readonly ProductionPlanCalculator _calculator = new();

    [Fact]
    public void Calculate_ShouldReturnZeroProduction_WhenLoadIsZero()
    {
        // Arrange
        var fuels = CreateFuels(wind: 50);
        var plants = new[]
        {
            CreatePlant("wind1", PlantType.WindTurbine, 0.5, 0, 100),
            CreatePlant("gas1", PlantType.GasFired, 0.5, 0, 100)
        };

        // Act
        var result = _calculator.Calculate(0, plants, fuels);

        // Assert
        Assert.True(result.IsSuccess);

        Assert.All(result.Value!, production =>
            Assert.Equal(0, production.Power));
    }

    [Fact]
    public void Calculate_ShouldUseWindFirst_WhenWindCanCoverLoad()
    {
        // Arrange
        var fuels = CreateFuels(wind: 50);
        var plants = new[]
        {
            CreatePlant("wind1", PlantType.WindTurbine, 0.5, 0, 100),
            CreatePlant("gas1", PlantType.GasFired, 0.5, 0, 100)
        };

        // Act
        var result = _calculator.Calculate(30, plants, fuels);

        // Assert
        Assert.True(result.IsSuccess);

        Assert.Equal(30, GetPower(result.Value!, "wind1"));
        Assert.Equal(0, GetPower(result.Value!, "gas1"));
    }

    [Fact]
    public void Calculate_ShouldUseGasAfterWind_WhenWindIsInsufficient()
    {
        // Arrange
        var fuels = CreateFuels(gas: 50, wind: 50);
        var plants = new[]
        {
            CreatePlant("wind1", PlantType.WindTurbine, 0.5, 0, 100),
            CreatePlant("gas1", PlantType.GasFired, 0.5, 0, 100)
        };

        // Act
        var result = _calculator.Calculate(80, plants, fuels);

        // Assert
        Assert.True(result.IsSuccess);

        Assert.Equal(50, GetPower(result.Value!, "wind1"));
        Assert.Equal(30, GetPower(result.Value!, "gas1"));

        Assert.Equal(
            80,
            result.Value!.Sum(p => p.Power));
    }

    [Fact]
    public void Calculate_ShouldDispatchThermalPlantsByMeritOrder()
    {
        // Arrange
        var fuels = CreateFuels(gas: 50, kerosine: 100, wind: 0);

        var gasPlant = CreatePlant(
            "gas1", PlantType.GasFired, 0.5, 0, 100);

        var turbojet = CreatePlant(
            "turbojet1", PlantType.Turbojet, 0.5, 0, 100);

        // Gas merit = 50 / 0.5 = 100
        // Turbojet merit = 100 / 0.5 = 200

        var plants = new[] { turbojet, gasPlant };

        // Act
        var result = _calculator.Calculate(150, plants, fuels);

        // Assert
        Assert.True(result.IsSuccess);

        Assert.Equal(100, GetPower(result.Value!, "gas1"));
        Assert.Equal(50, GetPower(result.Value!, "turbojet1"));
    }

    [Fact]
    public void Calculate_ShouldReturnFailure_WhenCapacityIsInsufficient()
    {
        // Arrange
        var fuels = CreateFuels(wind: 0);

        var plants = new[]
        {
            CreatePlant("gas1", PlantType.GasFired, 0.5, 0, 100),
            CreatePlant("turbojet1", PlantType.Turbojet, 0.5, 0, 100)
        };

        // Act
        var result = _calculator.Calculate(250, plants, fuels);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("InsufficientCapacity", result.Error.Code);
    }

    [Fact]
    public void Calculate_ShouldNotActivatePlant_WhenRemainingLoadIsBelowPmin()
    {
        // Arrange
        var fuels = CreateFuels(wind: 0);

        var plants = new[]
        {
            CreatePlant("gas1", PlantType.GasFired, 0.5, 30, 100)
        };

        // Act
        var result = _calculator.Calculate(20, plants, fuels);

        // Assert
        Assert.True(result.IsFailure);
    }

    private static PowerPlant CreatePlant(
        string name,
        PlantType type,
        double efficiency,
        double pmin,
        double pmax)
    {
        return PowerPlant.Create(
            name,
            efficiency,
            type.ToString(),
            pmin,
            pmax);
    }

    private static Fuel CreateFuels(
        double gas = 50,
        double kerosine = 100,
        double co2 = 0,
        double wind = 0)
    {
        return Fuel.Create(
            gas: gas,
            kerosine: kerosine,
            co2: co2,
            wind: wind);
    }

    private static double GetPower(
        IReadOnlyCollection<PlantProduction> productions,
        string plantName)
    {
        return productions.Single(p => p.Name == plantName).Power;
    }
}

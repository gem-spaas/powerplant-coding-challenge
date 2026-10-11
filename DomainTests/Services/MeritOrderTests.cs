
using Domain.Entities;
using Domain.Enums;
using Domain.Services;
using Domain.Value_Objects;

namespace Domain.Tests.Services;

public sealed class MeritOrderTests
{
    private static PowerPlant CreatePlant(
        string name,
        double efficiency,
        PlantType type)
    {
        return PowerPlant.Create(
            name,
            efficiency,
            type.ToString(),
            pmin: 0,
            pmax: 100);
    }

    [Fact]
    public void Sort_ShouldOrderPowerPlantsByMeritAscending()
    {
        // Arrange
        var fuels = CreateFuels(gas: 50, kerosine: 100);

        var gasPlant = CreatePlant(
            "gas1", 0.5, PlantType.GasFired);

        var turbojet = CreatePlant(
            "turbojet1", 0.5, PlantType.Turbojet);

        var windTurbine = CreatePlant(
            "wind1", 0.5, PlantType.WindTurbine);

        var plants = new[]
        {
            turbojet,
            gasPlant,
            windTurbine
        };

        // Act
        var result = MeritOrder.Sort(plants, fuels);

        // Assert
        Assert.Equal(
            new[] { "wind1", "gas1", "turbojet1" },
            result.Select(p => p.Name));
    }

    [Fact]
    public void Sort_ShouldConsiderEfficiencyWhenOrderingGasPlants()
    {
        // Arrange
        var fuels = CreateFuels(gas: 50, kerosine: 100);

        var efficientPlant = CreatePlant(
            "efficient", 0.8, PlantType.GasFired);

        var inefficientPlant = CreatePlant(
            "inefficient", 0.4, PlantType.GasFired);

        // Act
        var result = MeritOrder.Sort(
            new[] { inefficientPlant, efficientPlant },
            fuels);

        // Assert
        Assert.Equal(
            new[] { "efficient", "inefficient" },
            result.Select(p => p.Name));

        Assert.Equal(62.5, efficientPlant.CalculateMerit(fuels));
        Assert.Equal(125, inefficientPlant.CalculateMerit(fuels));
    }

    [Fact]
    public void Sort_ShouldOrderGasBeforeTurbojetWhenGasMeritIsLower()
    {
        // Arrange
        var fuels = CreateFuels(gas: 30, kerosine: 80);

        var gasPlant = CreatePlant(
            "gas1", 0.5, PlantType.GasFired);

        var turbojet = CreatePlant(
            "turbojet1", 0.5, PlantType.Turbojet);

        // Act
        var result = MeritOrder.Sort(
            new[] { turbojet, gasPlant },
            fuels);

        // Assert
        Assert.Equal(
            new[] { "gas1", "turbojet1" },
            result.Select(p => p.Name));
    }

    [Fact]
    public void Sort_ShouldReturnEmptyCollection_WhenNoPlantsProvided()
    {
        // Arrange
        var fuels = CreateFuels(gas: 50, kerosine: 100);

        // Act
        var result = MeritOrder.Sort(
            Array.Empty<PowerPlant>(),
            fuels);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void Sort_ShouldNotModifyOriginalCollection()
    {
        // Arrange
        var fuels = CreateFuels(gas: 50, kerosine: 100);

        var turbojet = CreatePlant(
            "turbojet1", 0.5, PlantType.Turbojet);

        var windTurbine = CreatePlant(
            "wind1", 0.5, PlantType.WindTurbine);

        var plants = new[] { turbojet, windTurbine };

        // Act
        var result = MeritOrder.Sort(plants, fuels);

        // Assert
        Assert.Equal(
            new[] { "turbojet1", "wind1" },
            plants.Select(p => p.Name));

        Assert.Equal(
            new[] { "wind1", "turbojet1" },
            result.Select(p => p.Name));
    }

    private static Fuel CreateFuels(
        double gas,
        double kerosine)
    {
        return Fuel.Create(
            gas: gas,
            kerosine: kerosine,
            co2: 0,
            wind: 0);
    }
}

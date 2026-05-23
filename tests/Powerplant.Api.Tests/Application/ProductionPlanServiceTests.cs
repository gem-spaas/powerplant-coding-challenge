using Powerplant.Api.Application.Services;
using Powerplant.Api.Domain;
using Xunit;

namespace Powerplant.Api.Tests.Application;

public sealed class ProductionPlanServiceTests
{
    [Fact]
    public void Calculate_UsesMeritOrderAndAllocatesRemainingLoad()
    {
        var service = new ProductionPlanService();

        var fuelPrices = CreateFuelPrices();

        var powerPlants = new List<PowerPlant>
        {
            CreatePowerPlant("tj1", PowerPlantType.TurboJet, 0.3m, 0m, 16m),
            CreatePowerPlant("gasfiredsomewhatsmaller", PowerPlantType.GasFired, 0.37m, 40m, 210m),
            CreatePowerPlant("windpark2", PowerPlantType.WindTurbine, 1m, 0m, 36m),
            CreatePowerPlant("gasfiredbig2", PowerPlantType.GasFired, 0.53m, 100m, 460m),
            CreatePowerPlant("windpark1", PowerPlantType.WindTurbine, 1m, 0m, 150m),
            CreatePowerPlant("gasfiredbig1", PowerPlantType.GasFired, 0.53m, 100m, 460m)
        };

        var result = service.Calculate(480m, fuelPrices, powerPlants).ToList();

        Assert.Collection(
            result,
            item =>
            {
                Assert.Equal("windpark2", item.Name);
                Assert.Equal(21.6m, item.Power);
            },
            item =>
            {
                Assert.Equal("windpark1", item.Name);
                Assert.Equal(90m, item.Power);
            },
            item =>
            {
                Assert.Equal("gasfiredbig2", item.Name);
                Assert.Equal(368.4m, item.Power);
            },
            item =>
            {
                Assert.Equal("gasfiredbig1", item.Name);
                Assert.Equal(0m, item.Power);
            },
            item =>
            {
                Assert.Equal("gasfiredsomewhatsmaller", item.Name);
                Assert.Equal(0m, item.Power);
            },
            item =>
            {
                Assert.Equal("tj1", item.Name);
                Assert.Equal(0m, item.Power);
            });
    }

    [Fact]
    public void Calculate_WhenFirstPlantCanSatisfyLoad_SetsRemainingPlantsToZero()
    {
        var service = new ProductionPlanService();

        var fuelPrices = CreateFuelPrices();

        var powerPlants = new List<PowerPlant>
        {
            CreatePowerPlant("gasfiredbig1", PowerPlantType.GasFired, 0.53m, 100m, 460m),
            CreatePowerPlant("tj1", PowerPlantType.TurboJet, 0.3m, 0m, 16m)
        };

        var result = service.Calculate(200m, fuelPrices, powerPlants).ToList();

        Assert.Equal("gasfiredbig1", result[0].Name);
        Assert.Equal(200m, result[0].Power);

        Assert.Equal("tj1", result[1].Name);
        Assert.Equal(0m, result[1].Power);
    }

    [Fact]
    public void Calculate_WhenRemainingLoadIsBelowPMin_DoesNotStartPlant()
    {
        var service = new ProductionPlanService();

        var fuelPrices = CreateFuelPrices();

        var powerPlants = new List<PowerPlant>
        {
            CreatePowerPlant("gasfiredbig1", PowerPlantType.GasFired, 0.53m, 100m, 460m),
            CreatePowerPlant("gasfiredsmall", PowerPlantType.GasFired, 0.37m, 40m, 210m)
        };

        var result = service.Calculate(480m, fuelPrices, powerPlants).ToList();

        Assert.Equal("gasfiredbig1", result[0].Name);
        Assert.Equal(460m, result[0].Power);

        Assert.Equal("gasfiredsmall", result[1].Name);
        Assert.Equal(0m, result[1].Power);
    }

    [Fact]
    public void Calculate_ReturnsOneEntryForEveryPowerPlant()
    {
        var service = new ProductionPlanService();

        var fuelPrices = CreateFuelPrices();

        var powerPlants = new List<PowerPlant>
        {
            CreatePowerPlant("windpark1", PowerPlantType.WindTurbine, 1m, 0m, 150m),
            CreatePowerPlant("gasfiredbig1", PowerPlantType.GasFired, 0.53m, 100m, 460m),
            CreatePowerPlant("tj1", PowerPlantType.TurboJet, 0.3m, 0m, 16m)
        };

        var result = service.Calculate(120m, fuelPrices, powerPlants);

        Assert.Equal(3, result.Count);
    }

    private static FuelPrices CreateFuelPrices()
    {
        return new FuelPrices
        {
            GasEuroPerMWh = 13.4m,
            KerosineEuroPerMWh = 50.8m,
            Co2EuroPerTon = 20m,
            WindPercentage = 60m
        };
    }

    private static PowerPlant CreatePowerPlant(
        string name,
        PowerPlantType type,
        decimal efficiency,
        decimal pMin,
        decimal pMax)
    {
        return new PowerPlant
        {
            Name = name,
            Type = type,
            Efficiency = efficiency,
            PMin = pMin,
            PMax = pMax
        };
    }
}

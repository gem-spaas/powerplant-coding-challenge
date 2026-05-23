using Powerplant.Api.Domain;
using Xunit;

namespace Powerplant.Api.Tests.Domain;

public sealed class PowerPlantTests
{
    [Fact]
    public void CostPerMWh_ForGasFiredPlant_UsesGasPriceEfficiencyAndCo2()
    {
        var fuelPrices = CreateFuelPrices();

        var powerPlant = new PowerPlant
        {
            Name = "gasfiredbig1",
            Type = PowerPlantType.GasFired,
            Efficiency = 0.53m,
            PMin = 100m,
            PMax = 460m
        };

        var result = powerPlant.CostPerMWh(fuelPrices);

        var expected = fuelPrices.GasEuroPerMWh / 0.53m + fuelPrices.Co2EuroPerTon * 0.3m;

        Assert.Equal(expected, result);
    }

    [Fact]
    public void CostPerMWh_ForTurboJet_UsesKerosinePriceEfficiencyAndCo2()
    {
        var fuelPrices = CreateFuelPrices();

        var powerPlant = new PowerPlant
        {
            Name = "tj1",
            Type = PowerPlantType.TurboJet,
            Efficiency = 0.3m,
            PMin = 0m,
            PMax = 16m
        };

        var result = powerPlant.CostPerMWh(fuelPrices);

        var expected = fuelPrices.KerosineEuroPerMWh / 0.3m + fuelPrices.Co2EuroPerTon * 0.3m;

        Assert.Equal(expected, result);
    }

    [Fact]
    public void CostPerMWh_ForWindTurbine_ReturnsZero()
    {
        var fuelPrices = CreateFuelPrices();

        var powerPlant = new PowerPlant
        {
            Name = "windpark1",
            Type = PowerPlantType.WindTurbine,
            Efficiency = 1m,
            PMin = 0m,
            PMax = 150m
        };

        var result = powerPlant.CostPerMWh(fuelPrices);

        Assert.Equal(0m, result);
    }

    [Fact]
    public void GetPossibleLoad_WhenLoadIsBelowPMin_ReturnsZero()
    {
        var fuelPrices = CreateFuelPrices();

        var powerPlant = new PowerPlant
        {
            Name = "gasfiredbig1",
            Type = PowerPlantType.GasFired,
            Efficiency = 0.53m,
            PMin = 100m,
            PMax = 460m
        };

        var result = powerPlant.GetPossibleLoad(50m, fuelPrices);

        Assert.Equal(0m, result);
    }

    [Fact]
    public void GetPossibleLoad_ForGasFiredPlant_WhenLoadIsAbovePMax_ReturnsPMax()
    {
        var fuelPrices = CreateFuelPrices();

        var powerPlant = new PowerPlant
        {
            Name = "gasfiredbig1",
            Type = PowerPlantType.GasFired,
            Efficiency = 0.53m,
            PMin = 100m,
            PMax = 460m
        };

        var result = powerPlant.GetPossibleLoad(500m, fuelPrices);

        Assert.Equal(460m, result);
    }

    [Fact]
    public void GetPossibleLoad_ForGasFiredPlant_WhenLoadIsBetweenPMinAndPMax_ReturnsLoad()
    {
        var fuelPrices = CreateFuelPrices();

        var powerPlant = new PowerPlant
        {
            Name = "gasfiredbig1",
            Type = PowerPlantType.GasFired,
            Efficiency = 0.53m,
            PMin = 100m,
            PMax = 460m
        };

        var result = powerPlant.GetPossibleLoad(250m, fuelPrices);

        Assert.Equal(250m, result);
    }

    [Fact]
    public void GetPossibleLoad_ForWindTurbine_UsesWindPercentage()
    {
        var fuelPrices = CreateFuelPrices();

        var powerPlant = new PowerPlant
        {
            Name = "windpark1",
            Type = PowerPlantType.WindTurbine,
            Efficiency = 1m,
            PMin = 0m,
            PMax = 150m
        };

        var result = powerPlant.GetPossibleLoad(500m, fuelPrices);

        Assert.Equal(90m, result);
    }

    [Fact]
    public void GetPossibleLoad_ForWindTurbine_WhenLoadIsLowerThanAvailableWindPower_ReturnsLoad()
    {
        var fuelPrices = CreateFuelPrices();

        var powerPlant = new PowerPlant
        {
            Name = "windpark1",
            Type = PowerPlantType.WindTurbine,
            Efficiency = 1m,
            PMin = 0m,
            PMax = 150m
        };

        var result = powerPlant.GetPossibleLoad(50m, fuelPrices);

        Assert.Equal(50m, result);
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
}

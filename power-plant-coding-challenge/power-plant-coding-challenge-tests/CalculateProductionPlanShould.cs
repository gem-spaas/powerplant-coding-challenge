using Microsoft.Extensions.Options;
using power_plant_coding_challenge_core.Features.CalculateProductionPlan;
using power_plant_coding_challenge_core.Options;
using power_plant_coding_challenge_domain.Enums;
using power_plant_coding_challenge_domain.Models;

namespace power_plant_coding_challenge_tests;

public class CalculateProductionPlanShould
{
    private readonly CalculateProductionPlanHandler _sut =
    new(Options.Create(new ProductionPlanOptions { IncludeCo2Costs = false }));

    // Powerplants coming from payload1/2/3.json
    private static List<Powerplant> DefaultPowerplants =>
    [
        new() { Name = "gasfiredbig1", Type = PowerplantType.Gasfired, Efficiency = 0.53m, Pmin = 100, Pmax = 460 },
        new() { Name = "gasfiredbig2", Type = PowerplantType.Gasfired, Efficiency = 0.53m, Pmin = 100, Pmax = 460 },
        new() { Name = "gasfiredsomewhatsmaller", Type = PowerplantType.Gasfired, Efficiency = 0.37m, Pmin = 40, Pmax = 210 },
        new() { Name = "tj1", Type = PowerplantType.Turbojet, Efficiency = 0.3m, Pmin = 0, Pmax = 16 },
        new() { Name = "windpark1", Type = PowerplantType.Windturbine, Efficiency = 1m, Pmin = 0, Pmax = 150 },
        new() { Name = "windpark2", Type = PowerplantType.Windturbine, Efficiency = 1m, Pmin = 0, Pmax = 36 },
    ];

    /// <summary>
    /// Test that the production plan returned by the handler matches the expected values for payload1.json.
    /// </summary>
    [Fact]
    public async Task ReturnCorrectPlan_ForPayload1()
    {
        //Arrange
        var command = new CalculateProductionPlanCommand(
            Load: 480,
            Fuels: new Fuel { Gas = 13.4m, Kerosine = 50.8m, Co2 = 20m, Wind = 60m },
            Powerplants: DefaultPowerplants
        );

        //Act
        var response = await _sut.Handle(command, CancellationToken.None);
        var results = response.Results.ToDictionary(result => result.Name, result => result.P);

        //Assert
        Assert.Equal(90.0m, results["windpark1"]);
        Assert.Equal(21.6m, results["windpark2"]);
        Assert.Equal(368.4m, results["gasfiredbig1"]);
        Assert.Equal(0m, results["gasfiredbig2"]);
        Assert.Equal(0m, results["gasfiredsomewhatsmaller"]);
        Assert.Equal(0m, results["tj1"]);
        Assert.Equal(480m, results.Values.Sum());
    }

    /// <summary>
    /// Test that the production plan returned by the handler matches the expected values for payload2.json.
    /// </summary>
    [Fact]
    public async Task ReturnCorrectPlan_ForPayload2()
    {
        //Arrange
        var command = new CalculateProductionPlanCommand(
            Load: 480,
            Fuels: new Fuel { Gas = 13.4m, Kerosine = 50.8m, Co2 = 20m, Wind = 0m },
            Powerplants: DefaultPowerplants
        );

        //Act
        var response = await _sut.Handle(command, CancellationToken.None);
        var results = response.Results.ToDictionary(result => result.Name, result => result.P);

        //Assert
        Assert.Equal(0m, results["windpark1"]);
        Assert.Equal(0m, results["windpark2"]);
        Assert.Equal(380m, results["gasfiredbig1"]);
        Assert.Equal(100m, results["gasfiredbig2"]);
        Assert.Equal(0m, results["gasfiredsomewhatsmaller"]);
        Assert.Equal(0m, results["tj1"]);
        Assert.Equal(480m, results.Values.Sum());
    }

    /// <summary>
    /// Test that the production plan returned by the handler matches the expected values for payload3.json.
    /// </summary>
    [Fact]
    public async Task ReturnCorrectPlan_ForPayload3()
    {
        //Arrange
        var command = new CalculateProductionPlanCommand(
            Load: 910,
            Fuels: new Fuel { Gas = 13.4m, Kerosine = 50.8m, Co2 = 20m, Wind = 60m },
            Powerplants: DefaultPowerplants
        );

        //Act
        var response = await _sut.Handle(command, CancellationToken.None);
        var results = response.Results.ToDictionary(result => result.Name, result => result.P);

        //Assert
        Assert.Equal(90.0m, results["windpark1"]);
        Assert.Equal(21.6m, results["windpark2"]);
        Assert.Equal(460.0m, results["gasfiredbig1"]);
        Assert.Equal(338.4m, results["gasfiredbig2"]);
        Assert.Equal(0m, results["gasfiredsomewhatsmaller"]);
        Assert.Equal(0m, results["tj1"]);
        Assert.Equal(910m, results.Values.Sum());
    }

    /// <summary>
    /// Test that the sum of the production plan returned by the handler equals the requested load, for various load and wind percentage combinations.
    /// </summary>
    [Theory]
    [InlineData(480, 60)]
    [InlineData(480, 0)]
    [InlineData(910, 60)]
    public async Task ReturnAResultWithSumEqualsToLoad(int load, decimal windPercent)
    {
        //Arrange
        var command = new CalculateProductionPlanCommand(
            Load: load,
            Fuels: new Fuel { Gas = 13.4m, Kerosine = 50.8m, Co2 = 20m, Wind = windPercent },
            Powerplants: DefaultPowerplants
        );

        //Act
        var response = await _sut.Handle(command, CancellationToken.None);

        //Assert
        Assert.Equal(load, response.Results.Sum(result => result.P));
    }

    /// <summary>
    /// Test that when the load is zero, the production plan returned by the handler contains only zero values for all powerplants.
    /// </summary>
    [Fact]
    public async Task ReturnAResultWithOnlyZeroPWhenLoadIsZero()
    {
        //Arrange
        var command = new CalculateProductionPlanCommand(
            Load: 0,
            Fuels: new Fuel { Gas = 13.4m, Kerosine = 50.8m, Co2 = 20m, Wind = 60m },
            Powerplants: DefaultPowerplants
        );

        //Act
        var response = await _sut.Handle(command, CancellationToken.None);

        //Assert
        Assert.All(response.Results, result => Assert.Equal(0m, result.P));
    }

    /// <summary>
    /// Test that when the wind percentage is zero, the production plan returned by the handler contains zero values for all wind turbines.
    /// </summary>
    [Fact]
    public async Task ReturnAResultWithWindTurbineAtZeroWhenWindIsZero()
    {
        //Arrange
        var command = new CalculateProductionPlanCommand(
            Load: 480,
            Fuels: new Fuel { Gas = 13.4m, Kerosine = 50.8m, Co2 = 20m, Wind = 0m },
            Powerplants: DefaultPowerplants
        );

        //Act
        var response = await _sut.Handle(command, CancellationToken.None);
        var results = response.Results.ToDictionary(result => result.Name, result => result.P);

        //Assert
        Assert.Equal(0m, results["windpark1"]);
        Assert.Equal(0m, results["windpark2"]);
    }

    /// <summary>
    /// Test that no powerplant in the production plan returned by the handler exceeds its Pmax, taking into account the effective Pmax for wind turbines based on the wind percentage.
    /// </summary>
    [Fact]
    public async Task NotReturnAPowerplantWithPExceedingItsPMax()
    {
        //Arrange
        var command = new CalculateProductionPlanCommand(
            Load: 910,
            Fuels: new Fuel { Gas = 13.4m, Kerosine = 50.8m, Co2 = 20m, Wind = 60m },
            Powerplants: DefaultPowerplants
        );

        //Act
        var response = await _sut.Handle(command, CancellationToken.None);

        //Assert
        foreach (var result in response.Results)
        {
            var plant = DefaultPowerplants.Single(p => p.Name == result.Name);
            var effectivePmax = plant.Type == PowerplantType.Windturbine
                ? plant.Pmax * (60m / 100m)
                : plant.Pmax;

            Assert.True(result.P <= effectivePmax,
                $"{result.Name} produces {result.P} MW but its Pmax is {effectivePmax} MW");
        }
    }

    /// <summary>
    /// Test that no powerplant in the production plan returned by the handler produces less than its Pmin if it is active (producing more than 0 MW).
    /// </summary>
    [Fact]
    public async Task NotReturnAnyPowerplantWithPUnderItsPMinIfPowerplantIsActive()
    {
        //Arrange
        var command = new CalculateProductionPlanCommand(
            Load: 910,
            Fuels: new Fuel { Gas = 13.4m, Kerosine = 50.8m, Co2 = 20m, Wind = 60m },
            Powerplants: DefaultPowerplants
        );

        //Act
        var response = await _sut.Handle(command, CancellationToken.None);

        //Assert
        foreach (var result in response.Results)
        {
            if (result.P == 0m) continue;

            var plant = DefaultPowerplants.Single(p => p.Name == result.Name);
            Assert.True(result.P >= plant.Pmin,
                $"{result.Name} produces {result.P} MW but its Pmin is {plant.Pmin} MW");
        }
    }

    /// <summary>
    /// Test that an exception is thrown when the load exceeds the total capacity of all powerplants.
    /// </summary>
    [Fact]
    public async Task ThrowExceptionWhenLoadExceedsTotalCapacity()
    {
        //Arrange
        var command = new CalculateProductionPlanCommand(
            Load: 800,
            Fuels: new Fuel { Gas = 15m, Kerosine = 50m, Co2 = 20m, Wind = 0m },
            Powerplants: [
                new() { Name = "StationA", Type = PowerplantType.Gasfired, Efficiency = 0.5m, Pmin = 50, Pmax = 300 },
                new() { Name = "StationB", Type = PowerplantType.Turbojet, Efficiency = 0.3m, Pmin = 0, Pmax = 100 },
            ]
        );

        //Act & Assert
        await Assert.ThrowsAnyAsync<Exception>(() => _sut.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// Test that when the effective Pmax of a wind farm equals the load, the wind farm covers the entire load and the gas backup remains off.
    /// </summary>
    [Fact]
    public async Task ReturnOnlyWindPowerplantResultIfWindPowerplantMatchTheExactLoad()
    {
        //Arrange
        var command = new CalculateProductionPlanCommand(
            Load: 40,
            Fuels: new Fuel { Gas = 15m, Kerosine = 50m, Co2 = 20m, Wind = 100m },
            Powerplants: [
                new() { Name = "WindFarm",  Type = PowerplantType.Windturbine, Efficiency = 1m, Pmin = 0, Pmax = 40 },
                new() { Name = "GasBackup", Type = PowerplantType.Gasfired, Efficiency = 0.6m, Pmin = 10, Pmax = 150 },
            ]
        );

        //Act
        var response = await _sut.Handle(command, CancellationToken.None);
        var results = response.Results.ToDictionary(result => result.Name, result => result.P);

        //Assert
        Assert.Equal(40m, results["WindFarm"]);
        Assert.Equal(0m, results["GasBackup"]);
        Assert.Equal(40m, results.Values.Sum());
    }

    /// <summary>
    /// Test that when the load is less than the effective Pmax of a wind farm, the wind farm covers part of the load and the gas backup covers the remainder.
    /// </summary>
    [Fact]
    public async Task ReturnAResultWithAMixOfWindAndGas()
    {
        //Arrange
        var command = new CalculateProductionPlanCommand(
            Load: 50,
            Fuels: new Fuel { Gas = 15m, Kerosine = 50m, Co2 = 20m, Wind = 75m },
            Powerplants: [
                new() { Name = "WindFarm", Type = PowerplantType.Windturbine, Efficiency = 1m,   Pmin = 0, Pmax = 40 },
                new() { Name = "GasPlant", Type = PowerplantType.Gasfired, Efficiency = 0.6m, Pmin = 5, Pmax = 100 },
            ]
        );

        //Act
        var response = await _sut.Handle(command, CancellationToken.None);
        var results = response.Results.ToDictionary(result => result.Name, result => result.P);

        //Assert
        Assert.Equal(30m, results["WindFarm"]);
        Assert.Equal(20m, results["GasPlant"]);
        Assert.Equal(50m, results.Values.Sum());
    }

    /// <summary>
    /// Test that when the effective Pmax of a wind farm exceeds the load, the wind farm is turned off and the gas backup covers the load.
    /// </summary>
    [Fact]
    public async Task NotReturnAnyWindInResultIfWindPowerplantsExceedsLoad()
    {
        //Arrange
        var command = new CalculateProductionPlanCommand(
            Load: 30,
            Fuels: new Fuel { Gas = 15m, Kerosine = 50m, Co2 = 20m, Wind = 80m },
            Powerplants: [
                new() { Name = "WindFarm", Type = PowerplantType.Windturbine, Efficiency = 1m, Pmin = 0, Pmax = 50 },
                new() { Name = "GasPlant", Type = PowerplantType.Gasfired, Efficiency = 0.6m, Pmin = 10, Pmax = 100 },
            ]
        );

        //Act
        var response = await _sut.Handle(command, CancellationToken.None);
        var results = response.Results.ToDictionary(result => result.Name, result => result.P);

        //Assert
        Assert.Equal(0m, results["WindFarm"]);
        Assert.Equal(30m, results["GasPlant"]);
        Assert.Equal(30m, results.Values.Sum());
    }

    /// <summary>
    /// Test that when the effective Pmax of a wind farm plus the Pmin of a gas plant exceeds the load, the wind farm is turned off and the gas plant covers the load.
    /// </summary>
    [Fact]
    public async Task NotReturnAnyWindPowerplantIfItsPMaxAndPMinOfNextPowerplantExceedsLoad()
    {
        //Arrange
        var command = new CalculateProductionPlanCommand(
            Load: 60,
            Fuels: new Fuel { Gas = 20m, Kerosine = 50m, Co2 = 0m, Wind = 100m },
            Powerplants: [
                new() { Name = "WindFarm", Type = PowerplantType.Windturbine, Efficiency = 1m, Pmin = 0, Pmax = 30 },
                new() { Name = "EfficientGas", Type = PowerplantType.Gasfired, Efficiency = 0.8m, Pmin = 40, Pmax = 100 },
                new() { Name = "InefficientGas", Type = PowerplantType.Gasfired, Efficiency = 0.1m, Pmin = 0, Pmax = 100 },
            ]
        );

        //Act
        var response = await _sut.Handle(command, CancellationToken.None);
        var results = response.Results.ToDictionary(result => result.Name, result => result.P);

        //Assert
        Assert.Equal(0m,  results["WindFarm"]);
        Assert.Equal(60m, results["EfficientGas"]);
        Assert.Equal(0m,  results["InefficientGas"]);
        Assert.Equal(60m, results.Values.Sum());
    }

    /// <summary>
    /// Test that when the effective Pmax of a wind farm plus the Pmin of a gas plant exceeds the load, and the gas plant is cheaper than the wind farm, the wind farm is turned off and the gas plant covers the load.
    /// </summary>
    [Fact]
    public async Task ReturnTheCheapestResultWithhoutWindIfPossible()
    {
        //Arrange
        var command = new CalculateProductionPlanCommand(
            Load: 80,
            Fuels: new Fuel { Gas = 20m, Kerosine = 50m, Co2 = 0m, Wind = 100m },
            Powerplants: [
                new() { Name = "WindFarm", Type = PowerplantType.Windturbine, Efficiency = 1m, Pmin = 0,  Pmax = 50 },
                new() { Name = "EfficientGas", Type = PowerplantType.Gasfired, Efficiency = 0.8m, Pmin = 40, Pmax = 120 },
                new() { Name = "InefficientGas", Type = PowerplantType.Gasfired, Efficiency = 0.1m, Pmin = 0,  Pmax = 200 },
            ]
        );

        //Act
        var response = await _sut.Handle(command, CancellationToken.None);
        var results = response.Results.ToDictionary(result => result.Name, result => result.P);

        //Assert
        Assert.Equal(0m, results["WindFarm"]);
        Assert.Equal(80m, results["EfficientGas"]);
        Assert.Equal(0m, results["InefficientGas"]);
        Assert.Equal(80m, results.Values.Sum());
    }

    /// <summary>
    /// Test that when multiple gas plants are available, the most efficient one is selected first in the merit order, and the others are not activated if the load can be met by the most efficient plant alone.
    /// </summary>
    [Fact]
    public async Task OrderThePowerplantBasedOnTheirActualEfficiency()
    {
        //Arrange
        var command = new CalculateProductionPlanCommand(
            Load: 50,
            Fuels: new Fuel { Gas = 15m, Kerosine = 50m, Co2 = 20m, Wind = 0m },
            Powerplants: [
                new() { Name = "OldPlant", Type = PowerplantType.Gasfired, Efficiency = 0.4m, Pmin = 10, Pmax = 150 },
                new() { Name = "EfficientPlant", Type = PowerplantType.Gasfired, Efficiency = 0.7m,  Pmin = 10, Pmax = 150 },
                new() { Name = "MidPlant", Type = PowerplantType.Gasfired, Efficiency = 0.55m, Pmin = 10, Pmax = 150 },
            ]
        );

        //Act
        var response = await _sut.Handle(command, CancellationToken.None);
        var results = response.Results.ToDictionary(result => result.Name, result => result.P);

        //Assert
        Assert.Equal(50m, results["EfficientPlant"]);
        Assert.Equal(0m,  results["MidPlant"]);
        Assert.Equal(0m,  results["OldPlant"]);
        Assert.Equal(50m, results.Values.Sum());
    }

    /// <summary>
    /// Test that when the load requires all available plants to be activated, they are activated in merit order based on their efficiency and cost.
    /// </summary>
    [Fact]
    public async Task ReturnAllPowerplantIfNeededWithTheResultOrderedInMeritOrder()
    {
        //Arrange
        var command = new CalculateProductionPlanCommand(
            Load: 490,
            Fuels: new Fuel { Gas = 15m, Kerosine = 50m, Co2 = 20m, Wind = 0m },
            Powerplants: [
                new() { Name = "Gas1", Type = PowerplantType.Gasfired, Efficiency = 0.5m, Pmin = 10, Pmax = 100 },
                new() { Name = "Gas2", Type = PowerplantType.Gasfired, Efficiency = 0.6m, Pmin = 10, Pmax = 100 },
                new() { Name = "Gas3", Type = PowerplantType.Gasfired, Efficiency = 0.8m, Pmin = 10, Pmax = 100 },
                new() { Name = "Gas4", Type = PowerplantType.Gasfired, Efficiency = 0.3m, Pmin = 10, Pmax = 100 },
                new() { Name = "Gas5", Type = PowerplantType.Gasfired, Efficiency = 0.45m, Pmin = 10, Pmax = 100 },
            ]
        );

        //Act
        var response = await _sut.Handle(command, CancellationToken.None);
        var results = response.Results.ToDictionary(result => result.Name, result => result.P);

        //Assert
        Assert.Equal(100m, results["Gas3"]);
        Assert.Equal(100m, results["Gas2"]);
        Assert.Equal(100m, results["Gas1"]);
        Assert.Equal(100m, results["Gas5"]);
        Assert.Equal(90m,  results["Gas4"]);
        Assert.Equal(490m, results.Values.Sum());
    }

    /// <summary>
    /// Test that when a powerplant's Pmin exceeds the remaining load after other plants have been activated, it is skipped and not activated in the production plan.
    /// </summary>
    [Fact]
    public async Task NotReturnAPowerplantWithPminExceedingRemainingLoadInTheProcess()
    {
        //Arrange
        var command = new CalculateProductionPlanCommand(
            Load: 120,
            Fuels: new Fuel { Gas = 15m, Kerosine = 50m, Co2 = 20m, Wind = 100m },
            Powerplants: [
                new() { Name = "WindFarm", Type = PowerplantType.Windturbine, Efficiency = 1m,   Pmin = 0,   Pmax = 20  },
                new() { Name = "HighPminPlant", Type = PowerplantType.Gasfired, Efficiency = 0.5m, Pmin = 120, Pmax = 250 },
                new() { Name = "EfficientPlant",Type = PowerplantType.Gasfired, Efficiency = 0.8m, Pmin = 70,  Pmax = 200 },
            ]
        );

        //Act
        var response = await _sut.Handle(command, CancellationToken.None);
        var results = response.Results.ToDictionary(result => result.Name, result => result.P);

        //Assert
        Assert.Equal(20m, results["WindFarm"]);
        Assert.Equal(0m, results["HighPminPlant"]);
        Assert.Equal(100m, results["EfficientPlant"]);
        Assert.Equal(120m, results.Values.Sum());
    }

    /// <summary>
    /// Test that when a turbojet's Pmin exceeds the remaining load after other plants have been activated, it is activated over a gas plant with a lower Pmin, as long as the turbojet is more efficient or cheaper.
    /// </summary>
    [Fact]
    public async Task ReturnResultWithTurbojetOverGasWhenGasPminExceedsRemainingLoad()
    {
        //Arrange
        var command = new CalculateProductionPlanCommand(
            Load: 100,
            Fuels: new Fuel { Gas = 15m, Kerosine = 50m, Co2 = 20m, Wind = 50m },
            Powerplants: [
                new() { Name = "WindFarm", Type = PowerplantType.Windturbine, Efficiency = 1m, Pmin = 0, Pmax = 150 },
                new() { Name = "GasPlant", Type = PowerplantType.Gasfired, Efficiency = 0.5m, Pmin = 100, Pmax = 200 },
                new() { Name = "JetTurbine", Type = PowerplantType.Turbojet, Efficiency = 0.5m, Pmin = 0, Pmax = 200 },
            ]
        );

        //Act
        var response = await _sut.Handle(command, CancellationToken.None);
        var results = response.Results.ToDictionary(result => result.Name, result => result.P);

        //Assert
        Assert.Equal(75m, results["WindFarm"]);
        Assert.Equal(0m, results["GasPlant"]);
        Assert.Equal(25m, results["JetTurbine"]);
        Assert.Equal(100m, results.Values.Sum());
    }

    /// <summary>
    /// Test that when CO2 costs are enabled, the CO2 cost is factored into the merit order of gas-fired plants.
    /// </summary>
    [Fact]
    public async Task ReturnTurboJetHigherInMeritOrderTakingToAccountTheCo2Cost()
    {
        //Arrange
        var sutWithCo2 = new CalculateProductionPlanHandler(
            Options.Create(new ProductionPlanOptions { IncludeCo2Costs = true }));

        var command = new CalculateProductionPlanCommand(
            Load: 50,
            Fuels: new Fuel { Gas = 10m, Kerosine = 20m, Co2 = 100m, Wind = 0m },
            Powerplants: [
                new() { Name = "GasPlant", Type = PowerplantType.Gasfired, Efficiency = 0.9m, Pmin = 0, Pmax = 100 },
                new() { Name = "JetPlant", Type = PowerplantType.Turbojet, Efficiency = 0.5m, Pmin = 0, Pmax = 100 },
            ]
        );

        //Act
        var response = await sutWithCo2.Handle(command, CancellationToken.None);
        var results = response.Results.ToDictionary(result => result.Name, result => result.P);

        //Assert
        Assert.Equal(50m, results["JetPlant"]);
        Assert.Equal(0m,  results["GasPlant"]);
        Assert.Equal(50m, results.Values.Sum());
    }

    /// <summary>
    /// Test that CO2 costs actively change the merit order between a gas plant and a turbojet.
    /// </summary>
    [Fact]
    public async Task ReturnDifferentResultRegardingTheUseOfCo2Cost()
    {
        //Arrange
        var sutNoCo2   = new CalculateProductionPlanHandler(Options.Create(new ProductionPlanOptions { IncludeCo2Costs = false }));
        var sutWithCo2 = new CalculateProductionPlanHandler(Options.Create(new ProductionPlanOptions { IncludeCo2Costs = true }));

        var command = new CalculateProductionPlanCommand(
            Load: 100,
            Fuels: new Fuel { Gas = 10m, Kerosine = 15m, Co2 = 30m, Wind = 0m },
            Powerplants: [
                new() { Name = "GasStation", Type = PowerplantType.Gasfired, Efficiency = 0.4m, Pmin = 0, Pmax = 200 },
                new() { Name = "JetStation", Type = PowerplantType.Turbojet, Efficiency = 0.5m, Pmin = 0, Pmax = 200 },
            ]
        );

        //Act
        var responseNoCo2  = await sutNoCo2.Handle(command, CancellationToken.None);
        var responseWithCo2 = await sutWithCo2.Handle(command, CancellationToken.None);

        var resultsNoCo2   = responseNoCo2.Results.ToDictionary(result => result.Name, result => result.P);
        var resultsWithCo2 = responseWithCo2.Results.ToDictionary(result => result.Name, result => result.P);

        //Assert — without CO2
        Assert.Equal(100m, resultsNoCo2["GasStation"]);
        Assert.Equal(0m, resultsNoCo2["JetStation"]);

        //Assert — with CO2
        Assert.Equal(0m, resultsWithCo2["GasStation"]);
        Assert.Equal(100m, resultsWithCo2["JetStation"]);
    }
}

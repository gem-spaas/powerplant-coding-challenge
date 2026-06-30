using PowerPlant.Domain;
using PowerPlant.Domain.Exceptions;
using PowerPlant.Domain.PowerPlants;
using PowerPlant.Domain.Planning;
using Xunit;
using Plant = PowerPlant.Domain.PowerPlants.PowerPlant;

namespace PowerPlant.Domain.UnitTests;

public class ProductionPlannerTests
{
    private readonly ProductionPlanner _planner = new();

    private static Plant[] ChallengeExamplePlants() =>
    [
        new GasFiredPlant("gasfiredbig1", 100m, 460m, 0.53m),
        new GasFiredPlant("gasfiredbig2", 100m, 460m, 0.53m),
        new GasFiredPlant("gasfiredsomewhatsmaller", 40m, 210m, 0.37m),
        new TurbojetPlant("tj1", 0m, 16m, 0.3m),
        new WindTurbinePlant("windpark1", 150m),
        new WindTurbinePlant("windpark2", 36m)
    ];

    private static Fuels ChallengeExampleFuels() => new(13.4m, 50.8m, 20m, 60m);

    [Fact]
    public void Diagnostic_ConstructorMapsArgumentsCorrectly()
    {
        var plant = new GasFiredPlant("x", 100m, 460m, 0.5m);
        var fuels = new Fuels(13.4m, 50.8m, 20m, 60m);

        Assert.Equal(100m, plant.EffectiveMinimum(fuels));
        Assert.Equal(460m, plant.EffectiveMaximum(fuels));
    }

    [Fact]
    public void Plan_WithChallengeExamplePayload_ReturnsExpectedProductionPlan()
    {
        var plan = _planner.Plan(910m, ChallengeExamplePlants(), ChallengeExampleFuels());
        var producedBy = plan.ToDictionary(output => output.Name, output => output.Power);

        Assert.Equal(460m, producedBy["gasfiredbig1"]);
        Assert.Equal(338.4m, producedBy["gasfiredbig2"]);
        Assert.Equal(0m, producedBy["gasfiredsomewhatsmaller"]);
        Assert.Equal(0m, producedBy["tj1"]);
        Assert.Equal(90m, producedBy["windpark1"]);
        Assert.Equal(21.6m, producedBy["windpark2"]);
    }

    [Fact]
    public void Plan_WithChallengeExamplePayload_ReturnsOutputsSummingExactlyToLoad()
    {
        var plan = _planner.Plan(910m, ChallengeExamplePlants(), ChallengeExampleFuels());

        Assert.Equal(910m, plan.Sum(output => output.Power));
    }

    [Fact]
    public void Plan_WithChallengeExamplePayload_ReturnsOutputsInTenthOfMegawattIncrements()
    {
        var plan = _planner.Plan(910m, ChallengeExamplePlants(), ChallengeExampleFuels());

        Assert.All(plan, output => Assert.Equal(0m, output.Power % 0.1m));
    }

    [Fact]
    public void Plan_WhenSwitchingOnACheaperPlantWouldOvershootThroughItsMinimum_ThrottlesTheCheaperPlant()
    {
        Plant[] plants =
        [
            new GasFiredPlant("cheaper", 0m, 30m, 1m),
            new GasFiredPlant("pricierWithHighMinimum", 40m, 60m, 0.9m)
        ];
        var fuels = new Fuels(10m, 50m, 0m, 0m);

        var plan = _planner.Plan(50m, plants, fuels);
        var producedBy = plan.ToDictionary(output => output.Name, output => output.Power);

        Assert.Equal(10m, producedBy["cheaper"]);
        Assert.Equal(40m, producedBy["pricierWithHighMinimum"]);
    }

    [Fact]
    public void Plan_WhenWindAloneWouldExceedLoad_SwitchesWindParksOff()
    {
        Plant[] plants =
        [
            new WindTurbinePlant("windpark1", 100m),
            new WindTurbinePlant("windpark2", 100m),
            new GasFiredPlant("gas", 0m, 100m, 1m)
        ];
        var fuels = new Fuels(10m, 50m, 0m, 50m);

        var plan = _planner.Plan(30m, plants, fuels);
        var producedBy = plan.ToDictionary(output => output.Name, output => output.Power);

        Assert.Equal(0m, producedBy["windpark1"]);
        Assert.Equal(0m, producedBy["windpark2"]);
        Assert.Equal(30m, producedBy["gas"]);
    }

    [Fact]
    public void Plan_WhenWindProducesFractionalPower_StillSumsToLoadInTenthIncrements()
    {
        Plant[] plants =
        [
            new WindTurbinePlant("windpark", 150m),
            new GasFiredPlant("gas", 0m, 500m, 1m)
        ];
        var fuels = new Fuels(10m, 50m, 0m, 37.7m);

        var plan = _planner.Plan(200m, plants, fuels);

        Assert.Equal(200m, plan.Sum(output => output.Power));
        Assert.All(plan, output => Assert.Equal(0m, output.Power % 0.1m));
    }

    [Fact]
    public void Plan_WhenLoadExceedsTotalCapacity_ThrowsInfeasibleLoadException()
    {
        Plant[] plants = [new GasFiredPlant("gas", 0m, 100m, 1m)];
        var fuels = new Fuels(10m, 50m, 0m, 0m);

        Assert.Throws<InfeasibleLoadException>(() => _planner.Plan(1000m, plants, fuels));
    }
}
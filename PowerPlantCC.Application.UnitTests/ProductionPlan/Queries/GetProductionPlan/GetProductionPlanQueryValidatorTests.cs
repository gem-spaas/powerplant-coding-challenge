
using PowerPlantCC.Domain.Models;
using PowerPlantCC.Domain.Enums;
using PowerPlantCC.Application.ProductionPlan.Queries.GetProductionPlan;
using FluentAssertions;

namespace PowerPlantCC.Application.UnitTests.ProductionPlan.Queries.GetProductionPlan;

public class GetProductionPlanQueryValidatorTests
{
    private GetProductionPlanQueryValidator _validator;
    private const string NEGATIVE_VALUE_ERROR_MESSAGE = "must be greater than or equal to 0";

    [SetUp]
    public void Setup()
    {
        _validator = new GetProductionPlanQueryValidator();
    }

    [Test]
    public void ValidateQuery_WithoutError_ShouldNotHaveAnyError()
    {
        GetProductionPlanQuery query = new()
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
                new() { Name = "gasfiredbig1", Type = PowerPlantType.gasfired, Efficiency = 0.53M, Pmin = 100, Pmax = 460} ,
                new() { Name = "gasfiredbig2", Type = PowerPlantType.gasfired, Efficiency = 0.53M, Pmin = 100, Pmax = 460 },
                new() { Name = "gasfiredsomewhatsmaller", Type = PowerPlantType.gasfired, Efficiency = 0.37M, Pmin = 40, Pmax = 210 },
                new() { Name = "tj1", Type = PowerPlantType.turbojet, Efficiency = 0.3M, Pmin = 0, Pmax = 16 },
                new() { Name = "windpark1", Type = PowerPlantType.windturbine, Efficiency = 1, Pmin = 0, Pmax = 150 },
                new() { Name = "windpark2", Type = PowerPlantType.windturbine, Efficiency = 1, Pmin = 0, Pmax = 36 }
            ]
        };

        var result = _validator.Validate(query);

        result.Errors.Count.Should().Be(0);
    }

    [Test]
    public void ValidateQuery_LoadIsNegative_ShouldHaveError()
    {
        GetProductionPlanQuery query = new()
        {
            Load = -10,
            Fuels = new Fuels
            {
                Gas = 13.4M,
                Kerosine = 50.8M,
                Co2 = 20,
                Wind = 60
            },
            PowerPlants = []
        };

        var result = _validator.Validate(query);

        result.Errors.Count.Should().Be(1);
        result.Errors[0].ErrorMessage.Should().Contain(NEGATIVE_VALUE_ERROR_MESSAGE);
    }

    [Test]
    public void ValidateQuery_WhereFuelsGasHaveNegativeValue_ShouldHaveError()
    {
        GetProductionPlanQuery query = new()
        {
            Load = 910,
            Fuels = new Fuels
            {
                Gas = -13.4M,
                Kerosine = 50.8M,
                Co2 = 20,
                Wind = 60
            },
            PowerPlants = []
        };

        var result = _validator.Validate(query);

        result.Errors.Count.Should().Be(1);
        result.Errors[0].ErrorMessage.Should().Contain(NEGATIVE_VALUE_ERROR_MESSAGE);
    }

    [Test]
    public void ValidateQuery_WhereFuelsKerosineHaveNegativeValue_ShouldHaveError()
    {
        GetProductionPlanQuery query = new()
        {
            Load = 910,
            Fuels = new Fuels
            {
                Gas = 13.4M,
                Kerosine = -50.8M,
                Co2 = 20,
                Wind = 60
            },
            PowerPlants = []
        };

        var result = _validator.Validate(query);

        result.Errors.Count.Should().Be(1);
        result.Errors[0].ErrorMessage.Should().Contain(NEGATIVE_VALUE_ERROR_MESSAGE);
    }

    [Test]
    public void ValidateQuery_WhereFuelsCo2HaveNegativeValue_ShouldHaveError()
    {
        GetProductionPlanQuery query = new()
        {
            Load = 910,
            Fuels = new Fuels
            {
                Gas = 13.4M,
                Kerosine = 50.8M,
                Co2 = -20,
                Wind = 60
            },
            PowerPlants = []
        };

        var result = _validator.Validate(query);

        result.Errors.Count.Should().Be(1);
        result.Errors[0].ErrorMessage.Should().Contain(NEGATIVE_VALUE_ERROR_MESSAGE);
    }

    [Test]
    public void ValidateQuery_WhereFuelsWindHaveNegativeValue_ShouldHaveError()
    {
        GetProductionPlanQuery query = new()
        {
            Load = 910,
            Fuels = new Fuels
            {
                Gas = 13.4M,
                Kerosine = 50.8M,
                Co2 = 20,
                Wind = -60
            },
            PowerPlants = []
        };

        var result = _validator.Validate(query);

        result.Errors.Count.Should().Be(1);
        result.Errors[0].ErrorMessage.Should().Contain(NEGATIVE_VALUE_ERROR_MESSAGE);
    }

    [Test]
    public void ValidateQuery_WherePowerPlantEfficiencyIsNegative_ShouldHaveError()
    {
        GetProductionPlanQuery query = new()
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
                new(){ Name = "gasfiredbig1", Type = PowerPlantType.gasfired, Efficiency = -0.53M, Pmin = 100, Pmax = 460 }
            ]
        };

        var result = _validator.Validate(query);

        result.Errors.Count.Should().Be(1);
        result.Errors[0].ErrorMessage.Should().Contain(NEGATIVE_VALUE_ERROR_MESSAGE);
    }

    [Test]
    public void ValidateQuery_WherePowerPlantPminIsNegative_ShouldHaveError()
    {
        GetProductionPlanQuery query = new()
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
                new(){ Name = "gasfiredbig1", Type = PowerPlantType.gasfired, Efficiency = 0.53M, Pmin = -100, Pmax = 460 }
            ]
        };

        var result = _validator.Validate(query);

        result.Errors.Count.Should().Be(1);
        result.Errors[0].ErrorMessage.Should().Contain(NEGATIVE_VALUE_ERROR_MESSAGE);
    }

    [Test]
    public void ValidateQuery_WherePowerPlantPmaxIsNegative_ShouldHaveError()
    {
        GetProductionPlanQuery query = new()
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
                new(){ Name = "gasfiredbig1", Type = PowerPlantType.gasfired, Efficiency = 0.53M, Pmin = 100, Pmax = -460 }
            ]
        };

        var result = _validator.Validate(query);

        result.Errors.Count.Should().Be(1);
        result.Errors[0].ErrorMessage.Should().Contain(NEGATIVE_VALUE_ERROR_MESSAGE);
    }
}

using PowerPlantCC.Domain.Models;
using PowerPlantCC.Domain.Enums;
using PowerPlantCC.Application.ProductionPlan.Services;
using FluentAssertions;
using PowerPlantCC.Application.Common.Exceptions;

namespace PowerPlantCC.Application.UnitTests.ProductionPlan.Services;

public class ProductionPlanServiceTests
{
    private IProductionPlanService _productionPlanService;

    [SetUp]
    public void Setup()
    {
        _productionPlanService = new ProductionPlanService();
    }

    [Test]
    public void BuildProductionPlan_WhereLoadIsNotFullyProvided_ShouldThrowException()
    {
        // Arrange

        Domain.Models.ProductionPlan buildRequest = new()
        {
            Load = 9100,
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

        Action act = () => _productionPlanService.BuildProductionPlan(buildRequest);

        act.Should().Throw<BusinessException>()
            .WithMessage("A business failure has occured.")
            .Where(e => e.ErrorMessage == "The given powerplants cannot fulfil the given load.");
    }
}
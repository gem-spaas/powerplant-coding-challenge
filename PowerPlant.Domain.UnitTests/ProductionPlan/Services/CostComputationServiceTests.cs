using FluentAssertions;
using PowerPlant.Domain.Enums;
using PowerPlant.Domain.Services;
using PowerPlant.Domain.UnitTests.Common;

namespace PowerPlant.Domain.UnitTests.ProductionPlan.Services;

public class CostComputationServiceTests
{
    private CostComputationService _costComputationService;
    
    [SetUp]
    public void Setup()
    {
        _costComputationService = new CostComputationService();
    }

    [Test]
    public void Valid_Default_Input_Should_Return_Expected_Cost()
    {
        var input = ProductionPlanTests.GetDefaultPowerPlanInput();

        var result = _costComputationService.ComputeCost(input);

        result.PowerPlants[0].Cost.Should().Be(36.603773584905660377358490566M);
        result.PowerPlants[1].Cost.Should().Be(36.603773584905660377358490566M);
        result.PowerPlants[2].Cost.Should().Be(52.432432432432432432432432432M);
        result.PowerPlants[3].Cost.Should().Be(169.33333333333333333333333333M);
        result.PowerPlants[4].Cost.Should().Be(0);
        result.PowerPlants[5].Cost.Should().Be(0);
    }
}
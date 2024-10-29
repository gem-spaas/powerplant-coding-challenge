using FluentAssertions;
using PowerPlant.Domain.Services;
using PowerPlant.Domain.UnitTests.Common;

namespace PowerPlant.Domain.UnitTests.ProductionPlan.Services;

public class UnitCommitmentStrategyServiceTests
{
    private UnitCommitmentStrategyService _unitCommitmentStrategyService;
    
    [SetUp]
    public void Setup()
    {
        _unitCommitmentStrategyService = new UnitCommitmentStrategyService();
    }

    [Test]
    public void Valid_Default_Input_Should_Return_Expected_UnitCommitment()
    {
        var input = ProductionPlanTests.GetDefaultPowerPlanInput();
        input.PowerPlants[0].Cost = 36.603773584905660377358490566M;
        input.PowerPlants[1].Cost = 36.603773584905660377358490566M;
        input.PowerPlants[2].Cost = 52.432432432432432432432432432M;
        input.PowerPlants[3].Cost = 169.33333333333333333333333333M;
        input.PowerPlants[4].Cost = 0;
        input.PowerPlants[4].Pmax = 90M;
        input.PowerPlants[5].Cost = 0;
        input.PowerPlants[5].Pmax = 21.6M;

        var result = _unitCommitmentStrategyService.Resolve(input);
        result.Count.Should().Be(6);
        
        result[0].Name.Should().Be("windpark1");
        result[0].Power.Should().Be(90.0M);
        result[1].Name.Should().Be("windpark2");
        result[1].Power.Should().Be(21.6M);
        result[2].Name.Should().Be("gasfiredbig1");
        result[2].Power.Should().Be(460.0M);
        result[3].Name.Should().Be("gasfiredbig2");
        result[3].Power.Should().Be(338.4M);
        result[4].Name.Should().Be("gasfiredsomewhatsmaller");
        result[4].Power.Should().Be(0);
        result[5].Name.Should().Be("tj1");
        result[5].Power.Should().Be(0);
    }
    
    [Test]
    public void Valid_Input_But_Need_To_Adapt_Previous_Added_PowerPlant_Should_Return_Expected_UnitCommitment()
    {
        var input = ProductionPlanTests.GetDefaultPowerPlanInput();
        input.Load = 210M;
        input.PowerPlants[0].Cost = 36.603773584905660377358490566M;
        input.PowerPlants[1].Cost = 36.603773584905660377358490566M;
        input.PowerPlants[2].Cost = 52.432432432432432432432432432M;
        input.PowerPlants[3].Cost = 169.33333333333333333333333333M;
        input.PowerPlants[4].Cost = 0;
        input.PowerPlants[4].Pmax = 90M;
        input.PowerPlants[5].Cost = 0;
        input.PowerPlants[5].Pmax = 21.6M;

        var result = _unitCommitmentStrategyService.Resolve(input);
        result.Count.Should().Be(6);
        
        result[0].Name.Should().Be("windpark1");
        result[0].Power.Should().Be(90.0M);
        result[1].Name.Should().Be("windpark2");
        result[1].Power.Should().Be(20M);
        result[2].Name.Should().Be("gasfiredbig1");
        result[2].Power.Should().Be(100.0M);
        result[3].Name.Should().Be("gasfiredbig2");
        result[3].Power.Should().Be(0);
        result[4].Name.Should().Be("gasfiredsomewhatsmaller");
        result[4].Power.Should().Be(0);
        result[5].Name.Should().Be("tj1");
        result[5].Power.Should().Be(0);
    }
}
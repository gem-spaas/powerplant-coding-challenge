using FluentAssertions;
using MapsterMapper;
using Moq;
using PowerPlant.Application.Exceptions;
using PowerPlant.Application.Services;
using PowerPlant.Application.UniTests.Common;
using PowerPlant.Domain.Entities;
using PowerPlant.Domain.Interfaces;
using PowerPlant.Domain.UnitTests.Common;

namespace PowerPlant.Application.UniTests.ProductionPlan.Services;

public class ProductionPlanServiceTests
{
    private ProductionPlanService _productionPlanService;
    
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ICostComputationService> _costComputationServiceMock;
    private readonly Mock<IUnitCommitmentStrategyService> _unitCommitmentStrategyServiceMock;

    public ProductionPlanServiceTests()
    {
        _mapperMock = new Mock<IMapper>();
        _costComputationServiceMock = new Mock<ICostComputationService>();
        _unitCommitmentStrategyServiceMock = new Mock<IUnitCommitmentStrategyService>();
    }
    
    [SetUp]
    public void Setup()
    {
        _productionPlanService = 
            new ProductionPlanService(
                _mapperMock.Object, 
                _costComputationServiceMock.Object, 
                _unitCommitmentStrategyServiceMock.Object);
    }

    [Test]
    public void ValidInputQuery_ShouldNotBreakDuringValidation()
    {
        var input = ProductionPlanTests.GetDefaultPowerPlanInput();
        
        var computationServiceOutput = input;
        computationServiceOutput.PowerPlants[0].Cost = 36.603773584905660377358490566M;
        computationServiceOutput.PowerPlants[1].Cost = 36.603773584905660377358490566M;
        computationServiceOutput.PowerPlants[2].Cost = 52.432432432432432432432432432M;
        computationServiceOutput.PowerPlants[3].Cost = 169.33333333333333333333333333M;
        computationServiceOutput.PowerPlants[4].Cost = 0;
        computationServiceOutput.PowerPlants[5].Cost = 0;

        var unitCommitmentServiceOutput = new List<UnitCommitment>
        {
            new ("windpark1", 90.0M),
            new ("windpark2", 21.6M),
            new ("gasfiredbig1", 460.0M),
            new ("gasfiredbig2", 338.4M),
            new ("gasfiredsomewhatsmaller", 0),
            new ("tj1", 0)
        };
        
        _costComputationServiceMock.Setup(x => x.ComputeCost(input))
            .Returns(computationServiceOutput);
        _unitCommitmentStrategyServiceMock.Setup(x => x.Resolve(It.IsAny<Domain.Entities.ProductionPlan>()))
            .Returns(unitCommitmentServiceOutput);
        _mapperMock.Setup(x => x.Map<Domain.Entities.ProductionPlan>(It.IsAny<Domain.Entities.ProductionPlan>()))
            .Returns(new Domain.Entities.ProductionPlan());
        
        Action result = () => _productionPlanService.GenerateProductionPlan(input);
        result.Should().NotThrow<BusinessException>();
    }
    
    [Test]
    public void Invalid_Load_Throw_Business_Exception()
    {
        var input = ProductionPlanTests.GetDefaultPowerPlanInput();
        input.Load = 2000m;
        
        var computationServiceOutput = input;
        computationServiceOutput.PowerPlants[0].Cost = 36.603773584905660377358490566M;
        computationServiceOutput.PowerPlants[1].Cost = 36.603773584905660377358490566M;
        computationServiceOutput.PowerPlants[2].Cost = 52.432432432432432432432432432M;
        computationServiceOutput.PowerPlants[3].Cost = 169.33333333333333333333333333M;
        computationServiceOutput.PowerPlants[4].Cost = 0;
        computationServiceOutput.PowerPlants[5].Cost = 0;

        var unitCommitmentServiceOutput = new List<UnitCommitment>
        {
            new ("windpark1", 90.0M),
            new ("windpark2", 21.6M),
            new ("gasfiredbig1", 460.0M),
            new ("gasfiredbig2", 338.4M),
            new ("gasfiredsomewhatsmaller", 0),
            new ("tj1", 0)
        };
        
        _costComputationServiceMock.Setup(x => x.ComputeCost(input))
            .Returns(computationServiceOutput);
        _unitCommitmentStrategyServiceMock.Setup(x => x.Resolve(It.IsAny<Domain.Entities.ProductionPlan>()))
            .Returns(unitCommitmentServiceOutput);
        _mapperMock.Setup(x => x.Map<Domain.Entities.ProductionPlan>(It.IsAny<Domain.Entities.ProductionPlan>()))
            .Returns(new Domain.Entities.ProductionPlan());
        
        
        Action result = () => _productionPlanService.GenerateProductionPlan(input);
        result.Should().Throw<BusinessException>()
            .WithMessage("Business failure occured.")
            .Where(exception => exception.ErrorMessage == "Powerplants cannot generated enough power for the load requested.");
    }
}
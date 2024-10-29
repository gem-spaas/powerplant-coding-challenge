using FluentAssertions;
using PowerPlant.Application.Queries.ProductionPlan;
using PowerPlant.Application.UniTests.Common;
using PowerPlant.Domain.Enums;

namespace PowerPlant.Application.UniTests.ProductionPlan.Queries;

public class ProductionPlanQueryValidatorTest
{
    private ProductionPlanQueryValidator _productionPlanQueryValidator;
    
    [SetUp]
    public void Setup()
    {
        _productionPlanQueryValidator = new ProductionPlanQueryValidator();
    }

    [Test]
    public void ValidInputQuery_ShouldNotBreakDuringValidation()
    {
        var input = ProductionPlanQueryTests.GetDefaultPowerPlanInput();
        
        var result = _productionPlanQueryValidator.Validate(input);
        result.Errors.Count.Should().Be(0);
    }
    
    [Test]
    public void InvalidInputQuery_LoadCantBeEmpty()
    {
        var input = ProductionPlanQueryTests.GetDefaultPowerPlanInput();
        input.Load = 0;
        
        var result = _productionPlanQueryValidator.Validate(input);
        result.Errors.Count.Should().Be(2);
        result.Errors[0].ErrorMessage.Should().Be("Load is required.");
        result.Errors[1].ErrorMessage.Should().Be("Load must be greater than 0.");
    }
    
    [Test]
    public void InvalidInputQuery_GasPriceCantBeNegative()
    {
        var input = ProductionPlanQueryTests.GetDefaultPowerPlanInput();
        input.Fuels.GasPricePerMWh = 0;
        
        var result = _productionPlanQueryValidator.Validate(input);
        result.Errors.Count.Should().Be(2);
        result.Errors[0].ErrorMessage.Should().Be("Gas Price is required.");
        result.Errors[1].ErrorMessage.Should().Be("Gas Price must be greater than 0.");
    }
    
    [Test]
    public void InvalidInputQuery_KerosinePriceCantBeNegative()
    {
        var input = ProductionPlanQueryTests.GetDefaultPowerPlanInput();
        input.Fuels.KerosinePricePerMWh = 0;
        
        var result = _productionPlanQueryValidator.Validate(input);
        result.Errors.Count.Should().Be(2);
        result.Errors[0].ErrorMessage.Should().Be("Kerosine Price is required.");
        result.Errors[1].ErrorMessage.Should().Be("Kerosine Price must be greater than 0.");
    }
    
    [Test]
    public void InvalidInputQuery_CO2PriceCantBeNegative()
    {
        var input = ProductionPlanQueryTests.GetDefaultPowerPlanInput();
        input.Fuels.Co2PricePerTon = 0;
        
        var result = _productionPlanQueryValidator.Validate(input);
        result.Errors.Count.Should().Be(2);
        result.Errors[0].ErrorMessage.Should().Be("CO2 Price is required.");
        result.Errors[1].ErrorMessage.Should().Be("CO2 Price must be greater than 0.");
    }
    
    [Test]
    public void InvalidInputQuery_WindPercentageCantBeNegative()
    {
        var input = ProductionPlanQueryTests.GetDefaultPowerPlanInput();
        input.Fuels.WindPercentage = -1;
        
        var result = _productionPlanQueryValidator.Validate(input);
        result.Errors.Count.Should().Be(1);
        result.Errors[0].ErrorMessage.Should().Be("Wind percentage must be greater than or equal to 0.");
    }
    
    [Test]
    public void InvalidInputQuery_PowerplantMustHaveAName()
    {
        var input = ProductionPlanQueryTests.GetDefaultPowerPlanInput();
        input.PowerPlants = new List<Domain.Entities.PowerPlant>
        {
            new()
            {
                Name = "",
                Type = PowerPlantType.windturbine,
                Efficiency = 1M,
                Pmin = 0,
                Pmax = 150
            }
        };
        
        var result = _productionPlanQueryValidator.Validate(input);
        result.Errors.Count.Should().Be(1);
        result.Errors[0].ErrorMessage.Should().Be("Power plant name is required.");
    }
    
    [Test]
    public void InvalidInputQuery_PowerplantCantHaveNegativeEfficiency()
    {
        var input = ProductionPlanQueryTests.GetDefaultPowerPlanInput();
        input.PowerPlants = new List<Domain.Entities.PowerPlant>
        {
            new()
            {
                Name = "windpark1",
                Type = PowerPlantType.windturbine,
                Efficiency = -1M,
                Pmin = 0,
                Pmax = 150
            }
        };
        
        var result = _productionPlanQueryValidator.Validate(input);
        result.Errors.Count.Should().Be(1);
        result.Errors[0].ErrorMessage.Should().Be("Power plant efficiency must be between 0 and 1.");
    }
    
    [Test]
    public void InvalidInputQuery_PowerplantCantEfficiencyBiggerThen1()
    {
        var input = ProductionPlanQueryTests.GetDefaultPowerPlanInput();
        input.PowerPlants = new List<Domain.Entities.PowerPlant>
        {
            new()
            {
                Name = "windpark1",
                Type = PowerPlantType.windturbine,
                Efficiency = 1.1M,
                Pmin = 0,
                Pmax = 150
            }
        };
        
        var result = _productionPlanQueryValidator.Validate(input);
        result.Errors.Count.Should().Be(1);
        result.Errors[0].ErrorMessage.Should().Be("Power plant efficiency must be between 0 and 1.");
    }
    
    [Test]
    public void InvalidInputQuery_PowerplantCantHavePMinNegative()
    {
        var input = ProductionPlanQueryTests.GetDefaultPowerPlanInput();
        input.PowerPlants = new List<Domain.Entities.PowerPlant>
        {
            new()
            {
                Name = "windpark1",
                Type = PowerPlantType.windturbine,
                Efficiency = 1M,
                Pmin = -1,
                Pmax = 150
            }
        };
        
        var result = _productionPlanQueryValidator.Validate(input);
        result.Errors.Count.Should().Be(1);
        result.Errors[0].ErrorMessage.Should().Be("Power plant minimum power must be greater than or equal to 0.");
    }
    
    [Test]
    public void InvalidInputQuery_PowerplantCantHavePMaxAt0()
    {
        var input = ProductionPlanQueryTests.GetDefaultPowerPlanInput();
        input.PowerPlants = new List<Domain.Entities.PowerPlant>
        {
            new()
            {
                Name = "windpark1",
                Type = PowerPlantType.windturbine,
                Efficiency = 1M,
                Pmin = 0,
                Pmax = 0
            }
        };
        
        var result = _productionPlanQueryValidator.Validate(input);
        result.Errors.Count.Should().Be(1);
        result.Errors[0].ErrorMessage.Should().Be("Power plant maximum power must be greater than 0.");
    }
    
    [Test]
    public void InvalidInputQuery_PowerplantPMaxMustBeGreaterThenPMin()
    {
        var input = ProductionPlanQueryTests.GetDefaultPowerPlanInput();
        input.PowerPlants = new List<Domain.Entities.PowerPlant>
        {
            new()
            {
                Name = "windpark1",
                Type = PowerPlantType.windturbine,
                Efficiency = 1M,
                Pmin = 20,
                Pmax = 10
            }
        };
        
        var result = _productionPlanQueryValidator.Validate(input);
        result.Errors.Count.Should().Be(1);
        result.Errors[0].ErrorMessage.Should().Be("Power plant maximum power must be greater than or equal to the minimum power.");
    }
}
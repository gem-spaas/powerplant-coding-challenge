using FluentValidation;
using PowerPlant.Domain.Enums;

namespace PowerPlant.Application.Queries.ProductionPlan;

public class ProductionPlanQueryValidator : AbstractValidator<ProductionPlanQuery>
{
    public ProductionPlanQueryValidator()
    {
        AddRuleForLoad();
        AddRuleForFuels();
        AddRuleForPowerPlants();
    }
    
    private void AddRuleForLoad()
    {
        RuleFor(query => query.Load)
            .NotEmpty().WithMessage("Load is required.")
            .GreaterThan(0).WithMessage("Load must be greater than 0.")
            .LessThanOrEqualTo(decimal.MaxValue).WithMessage("Load must be less than or equal to {MaxValue}.");
    }
    
    private void AddRuleForFuels()
    {
        RuleFor(query => query.Fuels.GasPricePerMWh)
            .NotEmpty().WithMessage("Gas Price is required.")
            .GreaterThan(0).WithMessage("Gas Price must be greater than 0.")
            .LessThanOrEqualTo(decimal.MaxValue).WithMessage("Gas Price must be less than or equal to {MaxValue}.");
        
        RuleFor(query => query.Fuels.KerosinePricePerMWh)
            .NotEmpty().WithMessage("Kerosine Price is required.")
            .GreaterThan(0).WithMessage("Kerosine Price must be greater than 0.")
            .LessThanOrEqualTo(decimal.MaxValue).WithMessage("Kerosine Price must be less than or equal to {MaxValue}.");
        
        RuleFor(query => query.Fuels.Co2PricePerTon)
            .NotEmpty().WithMessage("CO2 Price is required.")
            .GreaterThan(0).WithMessage("CO2 Price must be greater than 0.")
            .LessThanOrEqualTo(decimal.MaxValue).WithMessage("CO2 Price must be less than or equal to {MaxValue}.");
        
        RuleFor(query => query.Fuels.WindPercentage)
            .GreaterThanOrEqualTo(0).WithMessage("Wind percentage must be greater than or equal to 0.")
            .LessThanOrEqualTo(decimal.MaxValue).WithMessage("Wind percentage must be less than or equal to {MaxValue}.");
    }
    
    private void AddRuleForPowerPlants()
    {
        RuleForEach(query => query.PowerPlants)
            .ChildRules(powerPlant =>
            {
                powerPlant
                    .RuleFor(powerPlant => powerPlant.Name)
                    .NotEmpty().WithMessage("Power plant name is required.");

                powerPlant
                    .RuleFor(powerPlant => powerPlant.Type)
                    .Must(type => Enum.IsDefined(typeof(PowerPlantType), type)).WithMessage("Power plant type is invalid.");

                powerPlant
                    .RuleFor(powerPlant => powerPlant.Efficiency)
                    .InclusiveBetween(0M, 1M).WithMessage("Power plant efficiency must be between 0 and 1.");

                powerPlant
                    .RuleFor(powerPlant => powerPlant.Pmin)
                    .GreaterThanOrEqualTo(0).WithMessage("Power plant minimum power must be greater than or equal to 0.")
                    .LessThanOrEqualTo(decimal.MaxValue).WithMessage("Power plant minimum power must be less than or equal to {MaxValue}.");

                powerPlant
                    .RuleFor(powerPlant => powerPlant.Pmax)
                    .GreaterThan(0).WithMessage("Power plant maximum power must be greater than 0.")
                    .LessThanOrEqualTo(decimal.MaxValue).WithMessage("Power plant maximum power must be less than or equal to {MaxValue}.");

                powerPlant
                    .RuleFor(powerPlant => powerPlant.Pmax)
                    .GreaterThanOrEqualTo(powerPlant => powerPlant.Pmin)
                    .WithMessage("Power plant maximum power must be greater than or equal to the minimum power.");
            });
    }
}
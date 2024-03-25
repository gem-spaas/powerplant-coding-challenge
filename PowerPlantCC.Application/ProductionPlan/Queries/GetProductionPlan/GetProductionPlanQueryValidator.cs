using FluentValidation;
using PowerPlantCC.Application.Common.Extensions;

namespace PowerPlantCC.Application.ProductionPlan.Queries.GetProductionPlan;

public class GetProductionPlanQueryValidator : AbstractValidator<GetProductionPlanQuery>
{
    public GetProductionPlanQueryValidator()
    {
        RuleFor(x => x.Load)
            .GreaterThanOrEqualToWithMessage(0);

        RuleFor(x => x.Fuels.Wind)
            .GreaterThanOrEqualToWithMessage(0);

        RuleFor(x => x.Fuels.Kerosine)
            .GreaterThanOrEqualToWithMessage(0);

        RuleFor(x => x.Fuels.Gas)
            .GreaterThanOrEqualToWithMessage(0);

        RuleFor(x => x.Fuels.Co2)
            .GreaterThanOrEqualToWithMessage(0);

        RuleForEach(x => x.PowerPlants)
            .ChildRules(powerPlant =>
            {
                powerPlant
                    .RuleFor(x => x.Pmin)
                    .GreaterThanOrEqualToWithMessage(0);

                powerPlant
                    .RuleFor(x => x.Pmax)
                    .GreaterThanOrEqualToWithMessage(0);

                powerPlant
                    .RuleFor(x => x)
                    .Must(x => x.Pmax >= x.Pmin)
                    .When(x => x.Pmax >= 0 && x.Pmin >= 0)
                    .WithMessage("Pmax must be greater than or equal to Pmin.");

                powerPlant
                    .RuleFor(x => x.Efficiency)
                    .GreaterThanOrEqualToWithMessage(0);
            });
    }
}
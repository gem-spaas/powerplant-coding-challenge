using FluentValidation;

namespace Application.Use_Cases.ProductionPlan
{
    public sealed class CreateProductionPlanCommandValidator : AbstractValidator<CreateProductionPlanCommand>
    {
        public CreateProductionPlanCommandValidator()
        {
            RuleFor(x => x.Load)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Load must be a non-negative value.");

            RuleFor(x => x.Powerplants)
                .NotNull()
                .NotEmpty()
                .WithMessage("At least one powerplant is required.");

            RuleForEach(x => x.Powerplants)
                .ChildRules(powerplant =>
                {
                    powerplant.RuleFor(p => p.Name)
                        .NotEmpty()
                        .WithMessage("Powerplant name is required.");

                    powerplant.RuleFor(p => p.Efficiency)
                        .InclusiveBetween(0, 1)
                        .WithMessage(
                            "Efficiency must be a value between 0 and 1.");

                    powerplant.RuleFor(p => p.Type)
                    .NotEmpty()
                    .Must(type =>
                    type is "gasfired"
                    or "turbojet"
                    or "windturbine")
                    .WithMessage("Type must be a valid type.");

                    powerplant.RuleFor(p => p.Pmin)
                        .GreaterThanOrEqualTo(0)
                        .WithMessage(
                            "Pmin must be a non-negative value.");

                    powerplant.RuleFor(p => p.Pmax)
                        .GreaterThanOrEqualTo(0)
                        .WithMessage(
                            "Pmax must be a non-negative value.");

                    powerplant.RuleFor(p => p)
                        .Must(p => p.Pmin <= p.Pmax)
                        .WithMessage(
                            "Pmin must be less than or equal to Pmax.");
                });

            RuleFor(x => x.Fuels)
                .NotNull()
                .WithMessage("Fuel data is required.");

            RuleFor(x => x.Fuels.Gas)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Gas price must be a non-negative value.");

            RuleFor(x => x.Fuels.Kerosine)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Kerosine price must be a non-negative value.");

            RuleFor(x => x.Fuels.Co2)
                .GreaterThanOrEqualTo(0)
                .WithMessage("CO2 price must be a non-negative value.");

            RuleFor(x => x.Fuels.Wind)
                .InclusiveBetween(0, 100)
                .WithMessage("Wind must be a percentage between 0 and 100.");
        }
    }
}
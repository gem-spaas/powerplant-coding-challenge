using FluentValidation;
using MediatR;
using power_plant_coding_challenge_domain.Models;

namespace power_plant_coding_challenge_core.Features.CalculateProductionPlan
{
    public record CalculateProductionPlanResponse(List<CalculateProductionPlanResult> Results);
    public record CalculateProductionPlanResult(string Name, decimal P);
    public record CalculateProductionPlanCommand(
    int Load,
    Fuel Fuels,
    List<Powerplant> Powerplants) : IRequest<CalculateProductionPlanResponse>;
    public class CalculateProductionPlanCommandValidator : AbstractValidator<CalculateProductionPlanCommand>
    {
        public CalculateProductionPlanCommandValidator()
        {
            RuleFor(command => command.Load)
                .GreaterThanOrEqualTo(0).WithMessage("Load cannot be negative.");

            RuleFor(command => command.Fuels)
                .NotNull().WithMessage("Fuels are required.");

            RuleFor(command => command.Fuels.Gas)
                .GreaterThanOrEqualTo(0).WithMessage("Gas price cannot be negative.");

            RuleFor(command => command.Fuels.Kerosine)
                .GreaterThanOrEqualTo(0).WithMessage("Kerosine price cannot be negative.");

            RuleFor(command => command.Fuels.Wind)
                .InclusiveBetween(0, 100).WithMessage("Wind percentage must be between 0 and 100.");

            RuleFor(command => command.Powerplants)
                .NotNull().NotEmpty().WithMessage("At least one powerplant is required.");

            RuleForEach(command => command.Powerplants).ChildRules(powerplantValidator =>
            {
                powerplantValidator.RuleFor(powerplant => powerplant.Name)
                    .NotEmpty().WithMessage("Powerplant name is required.");

                powerplantValidator.RuleFor(powerplant => powerplant.Efficiency)
                    .InclusiveBetween(0, 1).WithMessage(powerplant => $"Efficiency of '{powerplant.Name}' must be between 0 and 1.");

                powerplantValidator.RuleFor(powerplant => powerplant.Pmin)
                    .GreaterThanOrEqualTo(0).WithMessage(powerplant => $"Pmin of '{powerplant.Name}' cannot be negative.");

                powerplantValidator.RuleFor(powerplant => powerplant.Pmax)
                    .GreaterThan(0).WithMessage(powerplant => $"Pmax of '{powerplant.Name}' must be greater than 0.")
                    .GreaterThanOrEqualTo(powerplant => powerplant.Pmin).WithMessage(powerplant => $"Pmax of '{powerplant.Name}' must be superior or equal to Pmin.");
            });
        }
    }
}

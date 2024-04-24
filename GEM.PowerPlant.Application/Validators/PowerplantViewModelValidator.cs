
using FluentValidation;
using GEM.PowerPlant.Application.ViewModels;

namespace GEM.PowerPlant.Application.Validators
{
	public class PowerplantViewModelValidator : AbstractValidator<PowerplantViewModel>
	{
		public PowerplantViewModelValidator()
		{
			RuleFor(p => p.Name)
				.NotEmpty()
				.WithMessage(p => $"{nameof(p.Name)} must not be empty");

			RuleFor(p => p)
				.Must(p => p.Pmax >= p.Pmin)
				.WithMessage(p => $"Powerplant {p.Name} Pmax must be greater than Pmin ");

			RuleFor(p => p.Efficiency)
				.InclusiveBetween(0, 1)
				.WithMessage(p => $"Powerplant {p.Name} Efficiency must be between 1 and 0");
		}
	}
}

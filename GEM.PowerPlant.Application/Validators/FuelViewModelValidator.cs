using FluentValidation;
using GEM.PowerPlant.Application.ViewModels;

namespace GEM.PowerPlant.Application.Validators
{
	public class FuelViewModelValidator : AbstractValidator<FuelViewModel>
	{
		public FuelViewModelValidator()
		{
			RuleFor(f => f.GasPrice)
				.GreaterThan(0)
				.WithMessage("gas(euro/MWh) must be positive");

			RuleFor(f => f.KerosinePrice)
				.GreaterThan(0)
				.WithMessage("kerosine(euro/MWh) must be positive");

			RuleFor(f => f.CarbonEmissionTax)
				.GreaterThan(0)
				.WithMessage("co2(euro/ton) must be positive");

			RuleFor(f => f.WindAvailaibilty)
				.GreaterThan(0)
				.WithMessage("wind(%) must be positive");
		}
	}
}

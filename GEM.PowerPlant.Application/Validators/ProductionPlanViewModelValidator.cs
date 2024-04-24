using FluentValidation;
using GEM.PowerPlant.Application.ViewModels;

namespace GEM.PowerPlant.Application.Validators
{
	public class ProductionPlanViewModelValidator : AbstractValidator<ProductionPlanViewModel>
	{
		public ProductionPlanViewModelValidator()
		{

			ClassLevelCascadeMode = CascadeMode.Continue;

			RuleFor(p => p.Load).GreaterThan(0)
				.WithMessage(p => $"{nameof(p.Load)} must be greater than 0");

			RuleFor(p => p.Fuels)
				.Cascade(CascadeMode.Stop)
				.NotNull()
				.WithMessage(p => $"{nameof(p.Fuels)} can not be null")
				.SetValidator(new FuelViewModelValidator());

			RuleFor(p => p.Powerplants)
				.NotNull()
				.WithMessage(p => $"{nameof(p.Powerplants)} can not be null");

			RuleForEach(p => p.Powerplants)
				.SetValidator(new PowerplantViewModelValidator());
		}
	}
}

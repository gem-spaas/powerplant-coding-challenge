using GEM.PowerPlant.Application.ViewModels;
using GEM.PowerPlant.Domain.Entities;


namespace Application.Mappers
{
	public static class PowerPlantMappers
	{
		private static Powerplant MapToDomain(this PowerplantViewModel viewModel) => Powerplant.Create(viewModel.Name,
			viewModel.Type,
			viewModel.Efficiency,
			viewModel.Pmax,
			viewModel.Pmin);

		public static List<Powerplant> MapToDomain(this List<PowerplantViewModel> list) => list.Select(MapToDomain).ToList();

		public static Fuel MapToDomain(this FuelViewModel viewModel) => Fuel.Create(viewModel.GasPrice,
			viewModel.KerosinePrice,
			viewModel.CarbonEmissionTax,
			viewModel.WindAvailaibilty);
	}
}

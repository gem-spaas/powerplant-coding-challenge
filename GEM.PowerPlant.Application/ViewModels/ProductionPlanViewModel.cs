namespace GEM.PowerPlant.Application.ViewModels
{
	public class ProductionPlanViewModel
	{
		public double Load { get; set; }
		public FuelViewModel Fuels { get; set; }
		public List<PowerplantViewModel> Powerplants { get; set; }
	}
}

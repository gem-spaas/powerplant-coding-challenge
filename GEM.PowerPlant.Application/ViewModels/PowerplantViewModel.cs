using GEM.PowerPlant.Domain.Enums;

namespace GEM.PowerPlant.Application.ViewModels
{
	public class PowerplantViewModel
	{
		public string Name { get; set; }

		public PowerplantType Type { get; set; }

		public double Efficiency { get; set; }

		public double Pmax { get; set; }

		public double Pmin { get; set; }
	}
}

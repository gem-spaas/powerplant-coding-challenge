
using GEM.PowerPlant.Application.Contracts;
using GEM.PowerPlant.Domain.Enums;

namespace GEM.PowerPlant.Application.Services
{
	public abstract class FossilPowerplantPlanner : IPoweplantPlanner
	{
		public abstract PowerplantType Type { get; }

		private double Generate(double requestedPower, double pmax, double pmin)
		{
			var generated = Math.Min(requestedPower, pmax);

			return generated < pmin ? pmin : generated;
		}

		public double Generate(double requestedPower, double pmax, double pmin, double windAvailability) => Generate(requestedPower, pmax, pmin);

	}
}

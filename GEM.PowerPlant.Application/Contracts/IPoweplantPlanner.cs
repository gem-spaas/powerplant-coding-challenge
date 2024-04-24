using GEM.PowerPlant.Domain.Enums;

namespace GEM.PowerPlant.Application.Contracts
{
	public interface IPoweplantPlanner
	{
		public PowerplantType Type { get; }
		double Generate(double requestedPower, double pmax, double pmin, double windAvailability = 0);
	}
}

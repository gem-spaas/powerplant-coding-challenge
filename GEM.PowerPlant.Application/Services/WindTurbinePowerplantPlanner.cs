using GEM.PowerPlant.Application.Contracts;
using GEM.PowerPlant.Domain.Enums;

namespace GEM.PowerPlant.Application.Services
{
	public class WindTurbinePowerplantPlanner : IPoweplantPlanner
	{
		public PowerplantType Type => PowerplantType.WindTurbine;

		public double Generate(double requestedPower, double pmax, double pmin, double windAvailability) => Math.Min(requestedPower, pmax) * windAvailability / 100;
	}
}

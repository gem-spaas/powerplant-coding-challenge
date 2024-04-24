using GEM.PowerPlant.Domain.Entities;

namespace GEM.PowerPlant.Application.Commands.Contracts
{
	public interface IPowerplantService
	{
		public IEnumerable<PowerplantProduction> GeneratePlan(List<Powerplant> availablePowerplants,
			Fuel fuel,
			double requestedPower);
	}
}

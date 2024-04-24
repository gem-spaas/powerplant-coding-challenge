using GEM.PowerPlant.Application.Commands.Contracts;
using GEM.PowerPlant.Application.Contracts;
using GEM.PowerPlant.Domain.Entities;
using GEM.PowerPlant.Domain.Enums;
using GEM.PowerPlant.Domain.Exceptions;

namespace GEM.PowerPlant.Application.Services
{
	public class PowerplantService : IPowerplantService
	{
		private readonly IPoweplantPlannerSelector poweplantPlannerSelector;

		public PowerplantService(IPoweplantPlannerSelector poweplantPlannerSelector)
		{
			this.poweplantPlannerSelector = poweplantPlannerSelector;
		}
		public IEnumerable<PowerplantProduction> GeneratePlan(List<Powerplant> availablePowerplants,
			Fuel fuelCost,
			double requestedPower)
		{

			var orderedPowerplants = availablePowerplants
				.Select(p =>
				{
					p.SetCost(type => SelectCost(type, fuelCost));
					return p;
				})
				.OrderBy(p => p.CostPerUnit)
				.ThenBy(p => p.Efficiency)
				.ToList();


			var remaingPower = requestedPower;

			List<PowerplantProduction> result = new List<PowerplantProduction>();

			int i = 0;

			while (i < orderedPowerplants.Count && remaingPower > 0)
			{
				var powerPlanner = poweplantPlannerSelector.Get(orderedPowerplants[i].Type);

				var generatedPower = powerPlanner.Generate(remaingPower,
					orderedPowerplants[i].Pmax,
					orderedPowerplants[i].Pmin,
					fuelCost.WindAvailaibilty);

				result.Add(PowerplantProduction.Create(orderedPowerplants[i].Name, generatedPower));


				remaingPower -= generatedPower;

				i++;
			}


			if (remaingPower > 0)
			{
				throw new NotReachedLoadException();
			}

			while (i < availablePowerplants.Count)
			{

				result.Add(PowerplantProduction.Create(orderedPowerplants[i].Name));
				i++;
			}

			return result;
		}


		private static double SelectCost(PowerplantType type, Fuel fuel) => type switch
		{
			PowerplantType.Turbojet => fuel.KerosinePrice,
			PowerplantType.Gasfired => fuel.GasPrice,
			PowerplantType.WindTurbine => 0d,
			_ => throw new NotImplementedException($"{nameof(SelectCost)} is not implemented for {type}")
		};

	}
}

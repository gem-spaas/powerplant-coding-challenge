using GEM.PowerPlant.Domain.Enums;

namespace GEM.PowerPlant.Domain.Entities
{
	public class Powerplant
	{

		public string Name { get; private set; }

		public PowerplantType Type { get; private set; }

		public double Efficiency { get; private set; }

		public double Pmax { get; private set; }

		public double Pmin { get; private set; }

		public double CostPerUnit { get; private set; }

		private Powerplant(string name,
			PowerplantType type,
			double efficiency,
			double pmax,
			double pmin)
		{
			Name = name;
			Type = type;
			Efficiency = efficiency;
			Pmax = pmax;
			Pmin = pmin;
		}

		public void SetCost(Func<PowerplantType, double> costSelector)
		{
			CostPerUnit = costSelector(Type) / Efficiency;
		}

		public static Powerplant Create(string name,
			PowerplantType type,
			double efficiency,
			double pmax,
			double pmin) => new Powerplant(name, type, efficiency, pmax, pmin);

	}
}

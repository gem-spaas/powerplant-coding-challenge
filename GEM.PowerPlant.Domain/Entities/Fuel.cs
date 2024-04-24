using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEM.PowerPlant.Domain.Entities
{
	public class Fuel
	{
		public double GasPrice { get; private set; }

		public double KerosinePrice { get; private set; }

		public double CarbonEmissionTax { get; private set; }

		public double WindAvailaibilty { get; private set; }

		private Fuel(double gasPrice,
			double kerosinePrice,
			double carbonEmissionTax,
			double windAvailaibilty)
		{
			GasPrice = gasPrice;
			KerosinePrice = kerosinePrice;
			CarbonEmissionTax = carbonEmissionTax;
			WindAvailaibilty = windAvailaibilty;
		}

		public static Fuel Create(double gasPrice,
			double kerosinePrice,
			double carbonEmissionTax,
			double windAvailaibilty) => new Fuel(gasPrice, kerosinePrice, carbonEmissionTax, windAvailaibilty);

	}
}

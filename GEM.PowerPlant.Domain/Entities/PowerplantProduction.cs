using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEM.PowerPlant.Domain.Entities
{
	public class PowerplantProduction
	{
		public string Name { get; private set; }

		protected PowerplantProduction(string name, double deliveredPower)
		{
			Name = name;
			DeliveredPower = deliveredPower;
		}

		public double DeliveredPower { get; private set; }

		public static PowerplantProduction Create(string name, double deliveredPower = 0) => new PowerplantProduction(name, deliveredPower);

	}
}

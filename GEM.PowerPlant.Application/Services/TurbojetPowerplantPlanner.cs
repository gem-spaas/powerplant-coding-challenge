using GEM.PowerPlant.Application.Contracts;
using GEM.PowerPlant.Domain.Enums;

namespace GEM.PowerPlant.Application.Services
{
	public class TurbojetPowerplantPlanner : FossilPowerplantPlanner, IPoweplantPlanner
	{
		public override PowerplantType Type => PowerplantType.Turbojet;
	}
}

using GEM.PowerPlant.Domain.Enums;

namespace GEM.PowerPlant.Application.Contracts
{
	public interface IPoweplantPlannerSelector
	{
		IPoweplantPlanner Get(PowerplantType type);
	}
}

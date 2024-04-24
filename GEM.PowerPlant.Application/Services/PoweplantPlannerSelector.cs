
using GEM.PowerPlant.Application.Contracts;
using GEM.PowerPlant.Domain.Enums;

namespace GEM.PowerPlant.Application.Services
{
	public class PoweplantPlannerSelector : IPoweplantPlannerSelector
	{
		private readonly IEnumerable<IPoweplantPlanner> planners;

		public PoweplantPlannerSelector(IEnumerable<IPoweplantPlanner> planners)
		{
			this.planners = planners;
		}
		public IPoweplantPlanner Get(PowerplantType type) => planners.FirstOrDefault(x => x.Type == type);
	}
}

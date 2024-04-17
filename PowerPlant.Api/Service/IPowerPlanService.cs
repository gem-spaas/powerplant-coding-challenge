using powerplant.Models;

namespace powerplant.Service
{
    public interface IPowerPlanService
    {
        public List<PowerPlantProuction> ActivatePowerPlants(PowerPlantRequest powerPlantRequest);
    }
}

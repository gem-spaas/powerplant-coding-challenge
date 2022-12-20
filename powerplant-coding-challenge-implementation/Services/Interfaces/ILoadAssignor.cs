using powerplant_coding_challenge_implementation.Models;

namespace powerplant_coding_challenge_implementation.Services.Interfaces
{
    public interface ILoadAssignor
    {
        List<ProductionPlanResponse> Assign(List<PowerPlant> meritOrderedPowerPlants,int load);
    }
}

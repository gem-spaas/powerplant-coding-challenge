using powerplant_coding_challenge_implementation.Models;

namespace powerplant_coding_challenge_implementation.Services.Interfaces
{
    public interface IMeritOrderCalculator
    {
        List<PowerPlant> Compute(ProductionPlanPayload productionPlanPayload);
    }
}

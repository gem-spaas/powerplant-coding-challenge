using PowerPlantCodingChallenge.ProductionPlan;

namespace PowerPlantCodingChallenge.Services
{
    public interface IProductionPlanService
    {
        IEnumerable<UnitCommitment> GenerateProductionPlan(PowerLoadForecast forecast);
    }
}

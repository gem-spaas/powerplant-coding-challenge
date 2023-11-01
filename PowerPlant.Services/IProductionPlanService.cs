using PowerPlant.Models;

namespace PowerPlant.Services;

public interface IProductionPlanService
{
    public IEnumerable<ProductionPlan> GetProductionPlan(RequestedLoad requestedLoad);
}
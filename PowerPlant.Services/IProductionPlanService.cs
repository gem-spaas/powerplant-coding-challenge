using PowerPlant.Models;

namespace PowerPlant.Services;

public interface IProductionPlanService
{
    public Task<IEnumerable<ProductionPlan>> GetProductionPlan(RequestedLoad requestedLoad);
}
using PowerPlant.Application.Queries.ProductionPlan;
using PowerPlant.Domain.Entities;

namespace PowerPlant.Application.Interfaces;

public interface IProductionPlanService
{
    List<ProductionPlanResponse> GenerateProductionPlan(ProductionPlan request);
}
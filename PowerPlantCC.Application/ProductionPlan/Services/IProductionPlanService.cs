using PowerPlantCC.Domain.Models;

namespace PowerPlantCC.Application.ProductionPlan.Services;

public interface IProductionPlanService
{
    Domain.Models.ProductionPlan BuildProductionPlan(Domain.Models.ProductionPlan productionPlan);
}
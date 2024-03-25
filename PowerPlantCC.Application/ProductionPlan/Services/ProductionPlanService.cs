
using PowerPlantCC.Application.Common.Exceptions;

namespace PowerPlantCC.Application.ProductionPlan.Services;

public class ProductionPlanService : IProductionPlanService
{
    public Domain.Models.ProductionPlan BuildProductionPlan(Domain.Models.ProductionPlan productionPlan)
    {
        productionPlan.Build();

        if (productionPlan.Load != productionPlan.CurrentPower)
        {
            throw new BusinessException("The given powerplants cannot fulfil the given load.");
        }

        return productionPlan;
    }
}
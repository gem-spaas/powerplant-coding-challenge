using PowerPlant.Domain.Entities;

namespace PowerPlant.Domain.Interfaces;

public interface ICostComputationService
{
    ProductionPlan ComputeCost(ProductionPlan productionPlan);
}
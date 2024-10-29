using PowerPlant.Domain.Entities;

namespace PowerPlant.Domain.Interfaces;

public interface IUnitCommitmentStrategyService
{
    List<UnitCommitment> Resolve(ProductionPlan productionPlan);
}
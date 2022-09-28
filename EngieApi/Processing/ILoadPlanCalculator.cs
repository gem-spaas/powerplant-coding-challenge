namespace EngieApi.Processing;

public interface ILoadPlanCalculator
{
    public ProductionPlanResponse GetLoadPlan(ProductionPlanRequest request);
}

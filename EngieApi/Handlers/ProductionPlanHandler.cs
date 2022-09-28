using EngieApi.Processing;

namespace EngieApi.Handlers;

public class ProductionPlanHandler
{
    public ProductionPlanHandler(ILoadPlanCalculator loadPlanCalculator)
    {
        LoadPlanCalculator = loadPlanCalculator;
    }

    public ILoadPlanCalculator LoadPlanCalculator { get; }

    public ProductionPlanResponse Handle(ProductionPlanRequest request)
    {
        return LoadPlanCalculator.GetLoadPlan(request);
    }
}

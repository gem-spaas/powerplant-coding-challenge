using PowerplantCodingChallenge.Domain.Request;
using PowerplantCodingChallenge.Domain.Response;

namespace PowerplantCodingChallenge.IBusiness
{
    public interface IProductionPlanService
    {
        IEnumerable<ProductionPlanItem> GetProductionPlan(ProductionPlanRequest request);
    }
}

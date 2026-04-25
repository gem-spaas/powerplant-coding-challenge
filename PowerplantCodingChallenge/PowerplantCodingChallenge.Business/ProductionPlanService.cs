using Microsoft.Extensions.Logging;
using PowerplantCodingChallenge.Domain.Request;
using PowerplantCodingChallenge.Domain.Response;
using PowerplantCodingChallenge.IBusiness;

namespace PowerplantCodingChallenge.Business
{
    public sealed class ProductionPlanService : IProductionPlanService
    {
        private readonly ILogger<ProductionPlanService> _logger;

        public ProductionPlanService(ILogger<ProductionPlanService> logger)
        {
            _logger = logger;
        }

        public IEnumerable<ProductionPlanItem> GetProductionPlan(ProductionPlanRequest request)
        {
            throw new NotImplementedException();
        }
    }
}

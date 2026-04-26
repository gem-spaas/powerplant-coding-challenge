using Microsoft.Extensions.Logging;
using PowerplantCodingChallenge.Business.Extensions;
using PowerplantCodingChallenge.Business.Factories;
using PowerplantCodingChallenge.Business.Models.Exceptions;
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
            var candidates = AllocationCandidateFactory.CreateCandidates(request);
            var sorted = candidates.SortByMeritOrder();
            var loadTenths = request.Load.ToTenths();
            var totalMin = 0;

            foreach (var candidate in sorted)
            {
                candidate.AllocatedPowerTenths = candidate.PowerMinTenths;
                totalMin += candidate.PowerMinTenths;
            }

            if (totalMin > loadTenths)
            {
                throw new BusinessException("Load cannot be satisfied due to minimum production constraints.");
            }

            int remaining = loadTenths - totalMin;

            foreach (var candidate in sorted)
            {
                if (remaining <= 0)
                    break;

                int available = candidate.PowerMaxTenths - candidate.AllocatedPowerTenths;
                int toAdd = Math.Min(available, remaining);

                candidate.AllocatedPowerTenths += toAdd;
                remaining -= toAdd;
            }

            if (remaining > 0)
            {
                throw new BusinessException("Unable to meet required load.");
            }

            return candidates
                .Select(c => new ProductionPlanItem
                {
                    Name = c.Name,
                    Power = (decimal)c.AllocatedPowerTenths / 10
                });
        }
    }
}

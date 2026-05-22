using PowerplantCodingChallenge.Application.DTOs;

namespace PowerplantCodingChallenge.Application.Interfaces;

public interface IProductionPlanService
{
    List<ProductionPlanResponse> CalculateProductionPlan(ProductionPlanRequest request);
}

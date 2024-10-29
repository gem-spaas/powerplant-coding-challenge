using MapsterMapper;
using PowerPlant.Application.Exceptions;
using PowerPlant.Application.Interfaces;
using PowerPlant.Application.Queries.ProductionPlan;
using PowerPlant.Domain.Entities;
using PowerPlant.Domain.Interfaces;

namespace PowerPlant.Application.Services;

public class ProductionPlanService : IProductionPlanService
{
    private readonly IMapper _mapper;
    private readonly ICostComputationService _costComputationService;
    private readonly IUnitCommitmentStrategyService _unitCommitmentStrategyService;
    public ProductionPlanService(
        IMapper mapper,
        ICostComputationService costComputationService,
        IUnitCommitmentStrategyService unitCommitmentStrategyService) 
    {
        _mapper = mapper;
        _costComputationService = costComputationService;
        _unitCommitmentStrategyService = unitCommitmentStrategyService;
    }
    
    public List<ProductionPlanResponse> GenerateProductionPlan(ProductionPlan request)
    {
        var productionPlan = _costComputationService.ComputeCost(request);
        
        var isAbleToManageLoad = productionPlan.PowerPlants.Sum(x => x.Pmax) >= productionPlan.Load;
        if(!isAbleToManageLoad)
        {
            throw new BusinessException("Powerplants cannot generated enough power for the load requested.");
        }
        var unitCommitmentForecast = _unitCommitmentStrategyService.Resolve(productionPlan);
        var result = _mapper.Map<List<ProductionPlanResponse>>(unitCommitmentForecast);
        
        return result;
    }
}
using MapsterMapper;
using MediatR;
using PowerPlant.Application.Interfaces;

namespace PowerPlant.Application.Queries.ProductionPlan;

public class ProductionPlanQueryHandler : IRequestHandler<ProductionPlanQuery, List<ProductionPlanResponse>>
{
    private readonly IMapper _mapper;
    private readonly IProductionPlanService _productionPlanService;

    public ProductionPlanQueryHandler(
        IMapper mapper, 
        IProductionPlanService productionPlanService)
    {
        _mapper = mapper;
        _productionPlanService = productionPlanService;
    }

    public Task<List<ProductionPlanResponse>> Handle(ProductionPlanQuery request, CancellationToken cancellationToken)
    {
        var serviceRequest = _mapper.Map<Domain.Entities.ProductionPlan>(request);
        var generatedProductionPlan = _productionPlanService.GenerateProductionPlan(serviceRequest);
        var result = _mapper.Map<List<ProductionPlanResponse>>(generatedProductionPlan);
        
        return Task.FromResult(result);
    }
}
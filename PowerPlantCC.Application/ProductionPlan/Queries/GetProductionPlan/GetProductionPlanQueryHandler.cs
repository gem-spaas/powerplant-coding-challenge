using MapsterMapper;
using MediatR;
using PowerPlantCC.Application.ProductionPlan.Services;

namespace PowerPlantCC.Application.ProductionPlan.Queries.GetProductionPlan;

public class GetProductionPlanQueryHandler : IRequestHandler<GetProductionPlanQuery, GetProductionPlanResponse[]>
{
    private readonly IProductionPlanService _productionPlanService;
    private readonly IMapper _mapper;

    public GetProductionPlanQueryHandler(IProductionPlanService productionPlanService, IMapper mapper)
    {
        _productionPlanService = productionPlanService;
        _mapper = mapper;
    }

    public Task<GetProductionPlanResponse[]> Handle(GetProductionPlanQuery request, CancellationToken cancellationToken)
    {
        var serviceRequest = _mapper.Map<Domain.Models.ProductionPlan>(request);
        var productionPlan = _productionPlanService.BuildProductionPlan(serviceRequest);

        var result = _mapper.Map<GetProductionPlanResponse[]>(productionPlan.PowerPlants);

        return Task.FromResult(result);
    }
}
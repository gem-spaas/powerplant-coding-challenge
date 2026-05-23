using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Powerplant.Api.Application.Abstractions;
using Powerplant.Api.Domain;
using Powerplant.Api.Generated;
using ProblemDetails = Powerplant.Api.Generated.ProblemDetails;

namespace Powerplant.Api.Controllers;

public sealed class ProductionPlanController : ProductionPlanControllerBaseControllerBase
{
    private readonly IProductionPlanService _productionPlanService;
    private readonly IMapper _mapper;

    public ProductionPlanController(IProductionPlanService productionPlanService, IMapper mapper)
    {
        _productionPlanService = productionPlanService;
        _mapper = mapper;
    }

    public override Task<ActionResult<ICollection<ProductionPlanResponseItem>>> CalculateProductionPlan([BindRequired, FromBody] ProductionPlanRequest body, CancellationToken cancellationToken = default)
    {
        if (body is null)
        {
            return Result(BadRequest(new ProblemDetails
            {
                Title = "Invalid request payload",
                Detail = "The request body is required.",
                Status = StatusCodes.Status400BadRequest
            }));
        }

        var plan = _productionPlanService.Calculate(body.Load, _mapper.Map<FuelPrices>(body.Fuels), _mapper.Map<List<PowerPlant>>(body.Powerplants));

        var response = _mapper.Map<ICollection<ProductionPlanResponseItem>>(plan);
        return Result(Ok(response));
    }

    private Task<ActionResult<ICollection<ProductionPlanResponseItem>>> Result(
    ActionResult<ICollection<ProductionPlanResponseItem>> result)
    {
        return Task.FromResult(result);
    }
}

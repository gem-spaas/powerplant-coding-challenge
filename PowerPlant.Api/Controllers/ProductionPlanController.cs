using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PowerPlant.Api.Models;
using PowerPlant.Application.Queries.ProductionPlan;

namespace PowerPlant.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductionPlanController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ISender _mediator;

    public ProductionPlanController(
        IMapper mapper, 
        ISender mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }
    
    [HttpPost(Name = "productionplan")]
    [Produces("application/json")]
    public async Task<IActionResult> GenerateProductionPlan([FromBody] ProductionPlanRequest request)
    {
        var query = _mapper.Map<ProductionPlanQuery>(request);
        var response = await _mediator.Send(query);
        return Ok(_mapper.Map<List<ProductionPlanResponseDTO>>(response));
    }
}
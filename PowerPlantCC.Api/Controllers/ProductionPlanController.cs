using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PowerPlantCC.Application.ProductionPlan.Queries.GetProductionPlan;
using PowerPlantCC.Contracts.ProductionPlan.Models;

namespace PowerPlantCC.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductionPlanController : ControllerBase
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public ProductionPlanController(IMapper mapper, ISender mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    [HttpPost]
    [Route("")]
    [Produces("application/json")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(PowerPlantOutputDTO[]))]
    public async Task<IActionResult> BuildProductionPlan([FromBody] GetProductionPlanRequestDTO payload)
    {
        var request = _mapper.Map<GetProductionPlanQuery>(payload);

        GetProductionPlanResponse[] response = await _mediator.Send(request);

        return Ok(_mapper.Map<PowerPlantOutputDTO[]>(response));
    }
}
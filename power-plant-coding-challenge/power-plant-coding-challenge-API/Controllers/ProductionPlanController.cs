using MediatR;
using Microsoft.AspNetCore.Mvc;
using power_plant_coding_challenge_core.Features.CalculateProductionPlan;

namespace power_plant_coding_challenge_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductionPlanController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<CalculateProductionPlanResult>), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<IActionResult> CalculateProductionPlan([FromBody] CalculateProductionPlanCommand command, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(command, cancellationToken);

            return Ok(response.Results);
        }
    }
}

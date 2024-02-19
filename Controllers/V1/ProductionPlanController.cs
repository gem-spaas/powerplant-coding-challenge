using Microsoft.AspNetCore.Mvc;
using powerplant_coding_challenge.Domain;
using powerplant_coding_challenge.Infrastructure.Models;
using System.Linq.Expressions;
using System.Text.Json;

namespace powerplant_coding_challenge.Controllers.V1
{
    [ApiController]
    [Route("[controller]")]
    public class ProductionPlanController : ControllerBase
    {
        private readonly ILogger<ProductionPlanController> _logger;
        private readonly IPowerClient _power;

        public ProductionPlanController(
            ILogger<ProductionPlanController> logger,
            IPowerClient power)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._power = power ?? throw new ArgumentNullException(nameof(power));
        }

        [HttpPost(Name = "ProductionPlan")]
        [ProducesResponseType(typeof(ProductionLoad), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostAction
        (
            [FromBody] ProductionLoad productionLoad
        )
        {
            //Validate the input
            if (productionLoad == null )
            {
                return await Task.FromResult(StatusCode(StatusCodes.Status400BadRequest, "Production Plan Request is missing"));
            }

            //Proceed with PowerPlant request
            try
            {
                var productionPlan = await _power.GetPowerPlan(productionLoad);

                if (productionPlan == null)
                {
                    return await Task.FromResult(StatusCode(StatusCodes.Status404NotFound, "Production Plan Not Found"));
                }

                //Return the production plan in array format
                return await Task.FromResult(StatusCode(StatusCodes.Status200OK, productionPlan.Plan));

            }
            catch (JsonException ex)
            {
                return await Task.FromResult(StatusCode(StatusCodes.Status400BadRequest, "Invalid JSON format: " + ex.Message));

            } catch (UnauthorizedAccessException ) {
                return await Task.FromResult(StatusCode(StatusCodes.Status401Unauthorized, "Unauthorized"));

            } catch (Exception ex) {
                return await Task.FromResult(StatusCode(StatusCodes.Status500InternalServerError, ex));
            }

        }
    }
}

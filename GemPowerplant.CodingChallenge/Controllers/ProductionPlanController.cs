using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GemPowerplant.CodingChallenge.Models;
using GemPowerplant.CodingChallenge.Services;

namespace GemPowerplant.CodingChallenge.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductionPlanController : ControllerBase
    {
        private readonly ILogger<ProductionPlanController> _logger;
        private readonly IProductionPlanService _productionPlanService;

        public ProductionPlanController(
            ILogger<ProductionPlanController> logger,
            IProductionPlanService productionPlanService)
        {
            _logger = logger;
            _productionPlanService = productionPlanService;
        }

        [HttpPost]
        public ActionResult<IEnumerable<ProductionPlanResponse>> CalculateProductionPlan([FromBody] ProductionPlanRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var productionPlan = _productionPlanService.CalculateProductionPlan(request.Load, request.Fuels, request.Powerplants);
                return Ok(productionPlan);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating production plan");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}

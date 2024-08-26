using Microsoft.AspNetCore.Mvc;
using PowerPlantCodingChallenge.ProductionPlan;
using PowerPlantCodingChallenge.Services;

namespace PowerPlantCodingChallenge.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductionPlanController : ControllerBase
    {
        private readonly ILogger<ProductionPlanController> _logger;
        private readonly IProductionPlanService _service;

        public ProductionPlanController(ILogger<ProductionPlanController> logger, IProductionPlanService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpPost(Name = "productionplan")]
        public IActionResult GenerateProductionPlan([FromBody] PowerLoadForecast forecast)
        {
            try
            {
                var productionPlan = _service.GenerateProductionPlan(forecast);
                return Ok(productionPlan);
            }
            catch (Exception e)
            {
                _logger.LogError($"{e.Message}\nStack Trace: {e.StackTrace?.ToString()}.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error during production plan generation.");
            }
        }
    }
}

using KiloWattNavigator.Domain;
using KiloWattNavigator.Service;
using Microsoft.AspNetCore.Mvc;

namespace KiloWattNavigator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PowerPlantController : ControllerBase
    {
        private readonly ILogger<PowerPlantController> _logger;
        private readonly IProductionPlanService _productionPlanService;

        public PowerPlantController(ILogger<PowerPlantController> logger, IProductionPlanService productionPlanService)
        {
            _logger = logger;
            _productionPlanService = productionPlanService;
        }

        [HttpPost("productionplan") ]
        public IActionResult CalculateProductionPlan([FromBody] ProductionPlanRequest request)
        {
            try
            {
                // Validate request
                if (request == null || request.Load <= 0 || request.Fuels == null || request.Powerplants == null || !request.Powerplants.Any())
                {
                    return BadRequest("Invalid request payload.");
                }

                // Calculate production plan
                var productionPlan = _productionPlanService.CalculateProduction(request.Load, request.Fuels, request.Powerplants);

                // Log production plan
                //LogProductionPlan(productionPlan);

                return Ok(productionPlan.Powerplants);
            }
            catch (Exception ex)
            {
                // Log and handle runtime errors
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

      
    }
}

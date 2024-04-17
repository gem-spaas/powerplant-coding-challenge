using Microsoft.AspNetCore.Mvc;
using powerplant.Models;
using powerplant.Service;

namespace powerplant.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductionPlanController : ControllerBase
    {


        private readonly ILogger<ProductionPlanController> _logger;
        private IPowerPlanService _powerPlanService; 

        public ProductionPlanController(ILogger<ProductionPlanController> logger, IPowerPlanService powerPlanService)
        {
            _logger = logger;
            _powerPlanService = powerPlanService;
        }

        // post method to activate powerPlant
        [HttpPost]
        public IActionResult PickPowerPlants(PowerPlantRequest powerPlantRequest)
        {
            try
            {
                var listPowerPlants = _powerPlanService.ActivatePowerPlants(powerPlantRequest);
                return Ok(listPowerPlants);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
               return new ObjectResult(new { error = ex.Message })
                {
                    StatusCode = 500
                };
            }
            
        }

    }
}

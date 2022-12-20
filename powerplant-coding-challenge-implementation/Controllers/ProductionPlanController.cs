using Microsoft.AspNetCore.Mvc;
using powerplant_coding_challenge_implementation.Models;
using powerplant_coding_challenge_implementation.Services.Interfaces;
using System.Net;
using System.Text.Json;

namespace powerplant_coding_challenge_implementation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductionPlanController : ControllerBase
    {
        private readonly ILogger<ProductionPlanController> _logger;
        private IMeritOrderCalculator _meritOrderCalculator;
        private ILoadAssignor _loadAssignor;

        public ProductionPlanController(ILogger<ProductionPlanController> logger,IMeritOrderCalculator meritOrderCalculator, ILoadAssignor loadAssignor)
        {
            _logger = logger;
            _meritOrderCalculator = meritOrderCalculator;
            _loadAssignor = loadAssignor;
        }

        [HttpPost(Name = "productionplan")]
        public IActionResult PostProductionPlan([FromBody] ProductionPlanPayload productionPlanPayload)
        {
            _logger.LogTrace($"Enter PostProductionPlan");

            List<PowerPlant> meritOrderedPowerPlants = _meritOrderCalculator.Compute(productionPlanPayload);

            List<ProductionPlanResponse> response = _loadAssignor.Assign(meritOrderedPowerPlants,productionPlanPayload.Load);

            return Ok(response);
        }
    }
}
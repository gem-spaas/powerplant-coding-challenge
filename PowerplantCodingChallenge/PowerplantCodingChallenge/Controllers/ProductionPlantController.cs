using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PowerplantCodingChallenge.Models;
using PowerplantCodingChallenge.Services;

namespace PowerplantCodingChallenge.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductionPlantController : ControllerBase
    {
        private readonly IPowerProductionService _powerProductionService;
        private readonly ILogger<ProductionPlantController> _logger;
        public ProductionPlantController(IPowerProductionService powerProductionService, ILogger<ProductionPlantController> logger)
        {
            this._powerProductionService = powerProductionService;
            this._logger = logger;
        }

        [HttpPost(Name = "/productionplan")]
        public async Task<IActionResult> Post(Payload payload)
        {          
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var powerSupply = await _powerProductionService.GetPowerPlantsActivatedByUnitCost(payload);

            return Ok(powerSupply);
        }
    }
}

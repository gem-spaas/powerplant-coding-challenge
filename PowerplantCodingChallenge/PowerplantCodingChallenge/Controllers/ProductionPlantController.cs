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
        public ProductionPlantController(IPowerProductionService powerProductionService)
        {
            this._powerProductionService = powerProductionService;
        }

        [HttpPost(Name = "/productionplan")]
        public async Task<IActionResult> Post(Payload payload)
        {
            var powerSupply = await _powerProductionService.GetPowerSupply(payload);
            
            return Ok(powerSupply);
        }
    }
}

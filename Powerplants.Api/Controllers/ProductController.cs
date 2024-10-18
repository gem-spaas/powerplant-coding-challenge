using BusinessLogic.Models;
using BusinessLogic.Services.Calculation;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Powerplant.Controllers
{
    [ApiController]
    [Route("productionplan")]
    public class ProductController : ControllerBase
    {
        private readonly ICalculationPowerPlanService _calculationService;

        public ProductController(ICalculationPowerPlanService calculationService)
        {
            _calculationService = calculationService;
        }
        [HttpPost]
        [ProducesResponseType(typeof(List<ProductResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult CalculatePowerPlants([FromBody] ProductPlanInput productPlanDto)
        {
            List<ProductResponse> result = _calculationService.CalculatePowerPlants(productPlanDto);
            return Ok(result);
        }
    }
}

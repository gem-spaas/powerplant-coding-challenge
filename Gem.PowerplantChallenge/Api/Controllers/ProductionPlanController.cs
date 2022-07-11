using Gem.PowerplantChallenge.Service.DTOs;
using Gem.PowerplantChallenge.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Gem.PowerplantChallenge.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductionPlanController : ControllerBase
{
    public ProductionPlanController(IProductionCalculatorService productionCalculatorService)
    {
        ProductionCalculatorService = productionCalculatorService;
    }

    private IProductionCalculatorService ProductionCalculatorService { get; }

    [HttpPost]
    public IActionResult Post([FromBody] PowerplantsWithLoadDTO payload)
    {
        return Ok(ProductionCalculatorService.GeneratePowerplantUsage(payload));
    }
}
using EngieApi.Handlers;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EngieApi.Controllers;

[ApiController]
[Route("/")]
public class ProductionPlanController : ControllerBase
{
    private readonly ILogger<ProductionPlanController> _logger;
    private readonly ProductionPlanHandler _productionPlanHandler;

    public ProductionPlanController(ILogger<ProductionPlanController> logger, ProductionPlanHandler productionPlanHandler)
    {
        _logger = logger;
        _productionPlanHandler = productionPlanHandler ?? throw new ArgumentNullException(nameof(productionPlanHandler));
    }

    [HttpPost(Name = "PostProduction")]
    [Consumes("application/json")]
    [Route("productionplan")]
    public JsonResult Post([FromBody] JsonElement payload)
    {
        return _productionPlanHandler.Handle(payload);
    }
}

using EngieApi.Handlers;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
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

    [HttpPost("productionplan")]
    public ProductionPlanResponse Post([FromBody][Required] ProductionPlanRequest request)
    {
        return _productionPlanHandler.Handle(request);
    }
}

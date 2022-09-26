using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EngieApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductionPlanController : ControllerBase
{
    private readonly ILogger<ProductionPlanController> _logger;

    public ProductionPlanController(ILogger<ProductionPlanController> logger)
    {
        _logger = logger;
    }

    [HttpPost(Name = "PostProduction")]
    [Consumes("application/json")]
    [Route("productionplan")]
    public JsonResult Post([FromBody] JsonElement request)
    {
        _logger.LogInformation("POST PostProduction called.");
        try
        {
            _logger.LogInformation("Start JSON Deserialize");
            ProductionPlanRequest? productionPlanRequest =
                    JsonSerializer.Deserialize<ProductionPlanRequest>(request);
            _logger.LogInformation("JSON Deserialized");

            _logger.LogInformation("Start JSON Serialize");
            List<ProductionPlanResponse> response = Processing.GetLoadPlan(productionPlanRequest, _logger);
            var jsonResult = JsonSerializer.Serialize(response);
            _logger.LogInformation("JSON Serialized");

            return new JsonResult(jsonResult);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw new Exception(ex.Message);
        };
    }
}

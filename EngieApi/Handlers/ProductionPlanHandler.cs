using EngieApi.Processing;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EngieApi.Handlers;

public class ProductionPlanHandler
{
    public JsonResult Handle(JsonElement payload)
    {
        try
        {
            ProductionPlanRequest? productionPlanRequest =
                    JsonSerializer.Deserialize<ProductionPlanRequest>(payload);

            List<ProductionPlanResponse> response = Calculator.GetLoadPlan(productionPlanRequest);
            var jsonResult = JsonSerializer.Serialize(response);

            return new JsonResult(jsonResult);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        };
    }

}

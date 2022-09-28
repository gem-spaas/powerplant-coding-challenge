using EngieApi.Processing;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EngieApi.Handlers;

public class ProductionPlanHandler
{
    public ProductionPlanResponse Handle(ProductionPlanRequest request)
    {
        return Calculator.GetLoadPlan(request);
    }
}

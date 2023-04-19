using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ProductionPlan.Models;

namespace ProductionPlan.Controllers;

[ApiController]
public class ProductionPlanController : ControllerBase
{
    private readonly IProductionPlan _productionPlan;
    public ProductionPlanController(IProductionPlan productionPlan)
    {
        _productionPlan = productionPlan;
    }

    [HttpPost("productionplan")]
    public IReadOnlyCollection<ProductionResponseDto> GetProductionPlan([FromBody] ProductionRequestDto requestDto)
    {
        return _productionPlan.GetProductionPlan(requestDto);
    }
}
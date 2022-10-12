using GemSpaasPowerplant.Model;
using Microsoft.AspNetCore.Mvc;

namespace GemSpaasPowerplant.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductionPlanController : ControllerBase
    {
       

        private readonly ILogger<ProductionPlanController> _logger;
        private readonly ICalculation _calculation;

        public ProductionPlanController(ILogger<ProductionPlanController> logger, ICalculation  calculation)
        {
            _logger = logger;
            _calculation = calculation;
        }

        [HttpPost(Name = "productionplan")]
        public IEnumerable<PowerLoad> Post(payload myPayload)
        {   
            return _calculation.GetProductionPlan(myPayload);
        }

        
    }
}
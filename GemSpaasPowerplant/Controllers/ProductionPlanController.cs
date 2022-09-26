using GemSpaasPowerplant.Model;
using Microsoft.AspNetCore.Mvc;

namespace GemSpaasPowerplant.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductionPlanController : ControllerBase
    {
       

        private readonly ILogger<ProductionPlanController> _logger;

        public ProductionPlanController(ILogger<ProductionPlanController> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "productionplan")]
        public IEnumerable<PowerLoad> Post(payload myPayload)
        {   
            Calculation calc = new Calculation(myPayload);
            return calc.GetProductionPlan();
        }

        
    }
}
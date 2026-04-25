using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PowerplantCodingChallenge.Domain.Request;
using PowerplantCodingChallenge.IBusiness;

namespace PowerplantCodingChallenge.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductionPlanController : ControllerBase
    {

        private readonly ILogger<ProductionPlanController> _logger;
        private readonly IProductionPlanService _pps;

        public ProductionPlanController(ILogger<ProductionPlanController> logger, IProductionPlanService pps)
        {
            _logger = logger;
            _pps = pps;
        }

        [HttpPost(Name = "GetProductionPlan")]
        public IActionResult GetProductionPlan(ProductionPlanRequest request)
        {
            try
            {
                var response = _pps.GetProductionPlan(request);
                return Ok(response);
            }
            catch (NotImplementedException ex)
            {
                _logger.LogError(ex, "Call attempted on an unimplemented method.");
                return StatusCode(501, new { message = "This method is not yet supported. Please try again later." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while computing production plan.");
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }
    }
}

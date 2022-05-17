using CalculatePowerGenerationByPowerPlants.Helpers;
using CalculatePowerGenerationByPowerPlants.Models;
using CalculatePowerGenerationByPowerPlants.Validators;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace CalculatePowerGenerationByPowerPlants.Controllers.V1
{

    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ProductionPlanController : ControllerBase
    {
        private readonly ILogger<ProductionPlanController> _logger;
        private readonly IPostRequestValidator<ProductionPlanRequest> _requestValidator;

        /// <summary>
        /// creates an instance of <see cref="ProductionPlanController"/>
        /// </summary>
        /// <param name="logger"><see cref="ILogger"/></param>
        /// <param name="requestValidator"><see cref="IRequestValidator<ProductionPlanRequest>"/></param>
        public ProductionPlanController(ILogger<ProductionPlanController> logger, IPostRequestValidator<ProductionPlanRequest> requestValidator)
        {
            _logger = logger;
            _requestValidator = requestValidator;
        }


        [HttpPost]
        [ProducesResponseType(typeof(ProductionPlanResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ObjectResult), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ObjectResult), (int)HttpStatusCode.InternalServerError)]
        public IActionResult Post(ProductionPlanRequest request)
        {
            try
            {
                _requestValidator.Validate(request);
                var response = MeritOrderHelper.CalculateProductionPlanByMeritOrder(request);
                return Ok(response);
            }
            catch (ArgumentException argEx)
            {
                _logger.LogError(argEx, $"Request :{JsonConvert.SerializeObject(request)}");
                return Problem(argEx.Message, statusCode: (int)HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Request :{JsonConvert.SerializeObject(request)}");
                return Problem();
            }
        }
    }
}

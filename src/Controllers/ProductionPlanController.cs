using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using PowerplantChallenge.Model;
using PowerplantChallenge.Services;

namespace PowerplantChallenge.Controllers {

    [ApiController]
    [Route("[controller]")]
    public class ProductionPlanController: ControllerBase
    {
        private readonly ILogger<ProductionPlanController> _logger;
        private readonly PowerPlanService _powerPlanService;

        public ProductionPlanController(ILogger<ProductionPlanController> logger, PowerPlanService powerPlanService){
            _powerPlanService = powerPlanService ?? throw new ArgumentNullException(nameof(powerPlanService));
            _logger = logger;
        }

        [HttpPost]
        public IEnumerable<ProductionPlanResponseElement> Post([FromBody][Required]ProductionPlanRequest request){
            List<(FuelType, decimal)> fuelCosts = new List<(FuelType, decimal)>{
                (FuelType.Gaz, request.Fuels.Gas),
                (FuelType.Kerosine, request.Fuels.Kerosine),
                (FuelType.Co2, request.Fuels.Co2),
                (FuelType.Wind, request.Fuels.Wind)
            };

            IEnumerable<Powerplant> powerplants = request.PowerPlants.Select(pp => new Powerplant(pp, fuelCosts));

            var result = _powerPlanService.GetPowerPlan(powerplants, request.Load);

            return result.Select(pp => new ProductionPlanResponseElement{Name = pp.Name, P = pp.ProposedLoad});
        }
    }
}
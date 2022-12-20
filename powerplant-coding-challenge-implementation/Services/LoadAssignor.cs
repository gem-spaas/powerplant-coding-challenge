using powerplant_coding_challenge_implementation.Controllers;
using powerplant_coding_challenge_implementation.Models;
using powerplant_coding_challenge_implementation.Services.Interfaces;

namespace powerplant_coding_challenge_implementation.Services
{
    public class LoadAssignor : ILoadAssignor
    {
        private readonly ILogger<ProductionPlanController> _logger;
        public LoadAssignor(ILogger<ProductionPlanController> logger)
        {
            _logger=logger;
        }

        public List<ProductionPlanResponse> Assign(List<PowerPlant> meritOrderedPowerPlants,int load)
        {
            List<ProductionPlanResponse> response = new();

            foreach(PowerPlant powerPlant in meritOrderedPowerPlants)
            {
             

                if (load == 0)
                {
                    ProductionPlanResponse productionPlanResponse = new ProductionPlanResponse(powerPlant.Name, 0);
                    response.Add(productionPlanResponse);
                }
                else
                {
                    int loadToAssign = (load - powerPlant.PActual > 0) ? powerPlant.PActual : load;
                    load -= loadToAssign;
                    ProductionPlanResponse productionPlanResponse = new ProductionPlanResponse(powerPlant.Name, loadToAssign);
                    response.Add(productionPlanResponse);
                }
                
            }
            return response;
        }
    }
}

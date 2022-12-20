using powerplant_coding_challenge_implementation.Models;
using powerplant_coding_challenge_implementation.Services.Interfaces;

namespace powerplant_coding_challenge_implementation.Services
{
    public class LoadAssignor : ILoadAssignor
    {
        public List<ProductionPlanResponse> Assign(List<PowerPlant> meritOrderedPowerPlants,int load)
        {
            List<ProductionPlanResponse> response = new();

            foreach(PowerPlant powerPlant in meritOrderedPowerPlants)
            {
             

                if (load == 0)
                {
                    ProductionPlanResponse productionPlanResponse = new()
                    {
                        Name = powerPlant.Name,
                        Production = 0
                    };
                    response.Add(productionPlanResponse);
                }
                else
                {
                    int loadToAssign = (load - powerPlant.PActual > 0) ? powerPlant.PActual : load;
                    load -= loadToAssign;
                    ProductionPlanResponse productionPlanResponse = new()
                    {
                        Name = powerPlant.Name,
                        Production = loadToAssign
                    };
                    response.Add(productionPlanResponse);
                }
                
            }
            return response;
        }
    }
}

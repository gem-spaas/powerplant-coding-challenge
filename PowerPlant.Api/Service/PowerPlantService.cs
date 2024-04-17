using powerplant.Models;
using powerplant.Helper;
using powerplant.PowerPlantActivationStrategy;

namespace powerplant.Service
{
    public class PowerPlantService :IPowerPlanService
    {
        public List<PowerPlantProuction> ActivatePowerPlants(PowerPlantRequest powerPlantRequest)
        {
            MeritOrderHelper.CalculatePowerPlantsCost(powerPlantRequest.PowerPlants, powerPlantRequest.Fuels);
            var ordredPowerPlants = MeritOrderHelper.OrderPowerPlantsByMerit(powerPlantRequest.PowerPlants);
            var powerPlantsActivationPlan = BuildtPlantsActivationPlan(ordredPowerPlants);
            var plantsFinaActivationPlan = BuildtPlantsFinaActivationPlan(powerPlantsActivationPlan, powerPlantRequest.Load, powerPlantRequest.Fuels.Wind) ;
            var activationPlanResponse = BuildtActivationPlanResponse(plantsFinaActivationPlan);
            return activationPlanResponse;
        }
        private List<PowerPlantProuction> BuildtActivationPlanResponse(Dictionary<string, double> finalActivationPlan)
        {
            return  finalActivationPlan.Select(p => new PowerPlantProuction
            {
                PowerPlantName = p.Key,
                Production = p.Value
            }).ToList() ;
        }
        private  Dictionary<PowerPlantProuction, Powerplant>  BuildtPlantsActivationPlan(List<Powerplant> ordredPowerPlants)
        {
            return ordredPowerPlants.ToDictionary(p => new PowerPlantProuction {
                PowerPlantName = p.Name,
                Production = 0
            },p=>p ) ;
        }

        private Dictionary<string, double> BuildtPlantsFinaActivationPlan(Dictionary<PowerPlantProuction, Powerplant> powerPlantsActivationPlan, double load, double windPercent)
        {
            var finalPlan = new Dictionary<string, double>();
            double currentPlantProduction = 0;
            foreach ( var powerPlantActivationPlan in powerPlantsActivationPlan )
            {
                var powerplant = powerPlantActivationPlan.Value;
                if (load <= 0)
                    finalPlan.Add(powerplant.Name, 0);
                else
                {
                    if (StrategyMapping.strategies.TryGetValue(powerplant.Type, out var activationStrategy))
                    {
                        currentPlantProduction = activationStrategy.GetProduction(powerplant.Pmin, powerplant.Pmax, load, windPercent);
                        finalPlan.Add (powerplant.Name, currentPlantProduction);
                        load = load - currentPlantProduction;
                    }
                }
            }
            return finalPlan;
        }
    }
}

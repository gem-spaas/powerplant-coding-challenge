using powerplant_coding_challenge_implementation.Models;
using powerplant_coding_challenge_implementation.Services.Interfaces;
using System;

namespace powerplant_coding_challenge_implementation.Services
{
    public class MeritOrderCalculator : IMeritOrderCalculator
    {
        public List<PowerPlant> Compute(ProductionPlanPayload productionPlanPayload)
        {
            List<ProductionPlanResponse> productionPlanResponses = new List<ProductionPlanResponse>();
            float productionRate;
            int comsuptionRate;
            Dictionary<PowerPlant, float> powerPlantByProfit = new();
            foreach (PowerPlant powerplant in productionPlanPayload.Powerplants)
            {
                switch (powerplant.Type)
                {
                    case "windturbine":
                        if(powerplant.Efficiency == 1)
                        {
                            // Using full capacity since it's either on or off
                            productionRate = (((productionPlanPayload.Fuels.Wind) * powerplant.PMax) / 100);
                            comsuptionRate = 0;
                            powerplant.PActual = Convert.ToInt32(productionRate);
                            powerPlantByProfit.Add(powerplant, productionRate - comsuptionRate);
                        }
                        else
                        {
                            productionRate = 0;
                            comsuptionRate = 0;
                            powerPlantByProfit.Add(powerplant, productionRate - comsuptionRate);

                        }
                        break;
                    case "turbojet":
                        productionRate = ((productionPlanPayload.Fuels.Kerozine * powerplant.PMax) / 100);
                        comsuptionRate = 100 - (powerplant.PMin/100);
                        powerplant.PActual = powerplant.PMax;
                        powerPlantByProfit.Add(powerplant, productionRate - comsuptionRate);
                        break;
                    case "gasfired":
                        productionRate = ((productionPlanPayload.Fuels.Gas * powerplant.PMax) / 100);
                        comsuptionRate = 100 - (powerplant.PMin/100);
                        powerplant.PActual = powerplant.PMax;
                        powerPlantByProfit.Add(powerplant, productionRate - comsuptionRate);
                        break;
                    default:
                        break;
                }
            }
            return powerPlantByProfit.OrderByDescending(e => e.Value).Select(e => e.Key).ToList<PowerPlant>();
        }
    }
}

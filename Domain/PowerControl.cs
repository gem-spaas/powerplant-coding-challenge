using powerplant_coding_challenge.Infrastructure.Models;

namespace powerplant_coding_challenge.Domain
{
    public class PowerControl : IPowerClient
    {
        public Task<ProductionPlan?> GetPowerPlan(ProductionLoad productionLoad)
        {
            // Get the total load/commitment that needs to be fulfilled
            float remainingLoad = productionLoad.Load * 1.00f;

            // Calculate effective price of producing power using gas
            float gasCost = productionLoad.Fuels.GasEuroMWh * productionLoad.PowerPlants
                .Where(pp => pp.Type == "gasfired")
                .Select(pp => pp.Efficiency)
                .Max();

            // Calculate effective price of producing power using kerosine
            float kerosineCost = productionLoad.Fuels.KerosineEuroMWh * productionLoad.PowerPlants
                .Where(pp => pp.Type == "turbojet")
                .Select(pp => pp.Efficiency)
                .Max();

            // Calculate the effective price of producing power using wind (wind power is considered free)
            float windCost = 0.00f;

            // Sort powerplants by their merit order (lowest cost first)
            var orderedPowerplants = productionLoad.PowerPlants.OrderBy(pp =>
            {
                if (pp.Type == "gasfired")
                    return gasCost;
                else if (pp.Type == "turbojet")
                    return kerosineCost;
                else
                    return windCost;
            }).ToArray(); // Convert to array

            // Allocate load to powerplants based on their Pmin and Pmax constraints
            var productionPlan = new ProductionPlan
            {
                Plan = new List<Plan>()
            };

            foreach (var powerplant in orderedPowerplants)            
            {
                var production = new Plan
                {
                    Name = powerplant.Name
                };

                if (powerplant.Type == "windturbine")
                {
                    // Calculate wind power production based on wind percentage                    
                    float effectivePowerMax = productionLoad.Fuels.Wind * powerplant.Efficiency * powerplant.PMax / 100;
                    production.P = Math.Min(effectivePowerMax, remainingLoad);
                }
                else
                {
                    // Calculate gas or kerosine power production
                    float effectivePowerMax = powerplant.PMax * powerplant.Efficiency;
                    production.P = Math.Min(effectivePowerMax, remainingLoad);
                }

                // Track commitment
                remainingLoad -= (float)production.P;
                if (remainingLoad <= 0)
                    remainingLoad = 0.00f;

                // Add powerplant calculations to structure
                productionPlan.Plan.Add(production);
            }

            return Task.FromResult<ProductionPlan?>(productionPlan);
        }
    }
}

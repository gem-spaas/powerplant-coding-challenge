using GemPowerplant.CodingChallenge.Models;

namespace GemPowerplant.CodingChallenge.Services
{
    public class ProductionPlanService : IProductionPlanService
    {
        public List<ProductionPlanResponse> CalculateProductionPlan(double load, Fuels fuels, List<Powerplant> powerPlants)
        {
            var sortedPlants = CalculateCost(fuels, powerPlants);
            var productionPlan = AllocatePowerToPlants(sortedPlants, load);

            ValidateLoad(load, productionPlan);

            return productionPlan;
        }

        private List<Powerplant> CalculateCost(Fuels fuels, List<Powerplant> powerPlants)
        {
            double windFactor = fuels.Wind / 100.0;
            double CO2_EMISSIONS_PER_MWH = 0.3;

            foreach (var plant in powerPlants)
            {
                switch (plant.Type)
                {
                    case PowerplantType.Gasfired:
                        // Include both gas cost and CO2 emissions cost
                        double gasCost = fuels.Gas / plant.Efficiency;
                        double co2Cost = (fuels.Co2 * CO2_EMISSIONS_PER_MWH) / plant.Efficiency;
                        plant.Cost = gasCost + co2Cost;
                        break;
                    case PowerplantType.Turbojet:
                        plant.Cost = fuels.Kerosine / plant.Efficiency;
                        break;
                    case PowerplantType.Windturbine:
                        plant.Cost = 0;
                        plant.Pmax *= windFactor;
                        plant.Pmin = 0;
                        break;
                }
            }

            return powerPlants.OrderBy(p => p.Cost).ToList();
        }

        private List<ProductionPlanResponse> AllocatePowerToPlants(List<Powerplant> sortedPlants, double load)
        {
            var productionPlan = new List<ProductionPlanResponse>();
            double remainingLoad = load;

            foreach (var plant in sortedPlants)
            {
                double power = 0;

                if (remainingLoad > 0)
                {
                    power = Math.Min(remainingLoad, plant.Pmax);

                    if (plant.Pmax > 0)
                    {
                        if (power >= plant.Pmin || remainingLoad <= plant.Pmax)
                        {
                            power = Math.Max(power, plant.Pmin);

                            remainingLoad -= power;
                        }
                    }
                }


                productionPlan.Add(new ProductionPlanResponse
                {
                    Name = plant.Name,
                    P = Math.Round(power, 1)
                });
            }

            return productionPlan;
        }

        private void ValidateLoad(double initialLoad, List<ProductionPlanResponse> productionPlan)
        {
            double totalGeneratedPower = productionPlan.Sum(p => p.P);

            if (totalGeneratedPower < initialLoad)
            {
                throw new InvalidOperationException("Not enough capacity to meet the load.");
            }
        }
    }
}

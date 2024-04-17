using KiloWattNavigator.Domain;

namespace KiloWattNavigator.Service
{
    public class ProductionPlanService : IProductionPlanService
    {
        public ProductionPlanResponse CalculateProduction(double load, Fuels fuels, List<Powerplant> powerplants)
        {
            var productionPlan = new ProductionPlanResponse();
            var remainingLoad = load;

            // Sort powerplants by merit-order
            var sortedPowerplants = powerplants.OrderBy(p => p.GetCost(fuels)).ToList();

            // Loop through powerplants and allocate power
            foreach (var powerplant in sortedPowerplants)
            {
                var powerToGenerate = 0.0d;
                if (remainingLoad > 0)
                {
                    //Take the min amount of power we still need
                    //Can be the full PMAX of the powerplant or the remaining load but it produces anyway the PMin if powerplan is on
                    powerToGenerate = Math.Min(powerplant.GetPowerMax(fuels.Wind), Math.Max(powerplant.Pmin, remainingLoad));
                    remainingLoad -= powerToGenerate;
                }
                productionPlan.AddPowerplant(powerplant.Name, powerToGenerate);
            }
            return productionPlan;
        }

    }
}

using GemPowerplant.CodingChallenge.Models;

namespace GemPowerplant.CodingChallenge.Services
{
    public interface IProductionPlanService
    {
        List<ProductionPlanResponse> CalculateProductionPlan(double load, Fuels fuels, List<Powerplant> powerPlants);
    }
}

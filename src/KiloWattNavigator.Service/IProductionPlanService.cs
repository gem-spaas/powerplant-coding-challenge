using KiloWattNavigator.Domain;

namespace KiloWattNavigator.Service
{
    public interface IProductionPlanService
    {
        ProductionPlanResponse CalculateProduction(double load, Fuels fuels, List<Powerplant> powerplants);
        

    }
}

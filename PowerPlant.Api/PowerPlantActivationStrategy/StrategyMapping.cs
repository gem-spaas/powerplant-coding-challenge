using powerplant.Helper;

namespace powerplant.PowerPlantActivationStrategy
{
    public class StrategyMapping
    {
        public static Dictionary<string, IPowerPlantActivationStrategy> strategies =
           new Dictionary<string, IPowerPlantActivationStrategy>(){
                {PowerPlantType.gasfired.ToString(), new UncleanEnergyStrategy ()},
             {PowerPlantType.turbojet.ToString(), new UncleanEnergyStrategy()},
             {PowerPlantType.windturbine.ToString(), new RenewableEnergyStrategy()},
             };
    }
}

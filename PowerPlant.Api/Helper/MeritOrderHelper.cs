using powerplant.Models;
using System.Linq.Expressions;
using System.Linq;
namespace powerplant.Helper
{
    public static class MeritOrderHelper
    {
        private static Dictionary<string, string> FuelTypes = new Dictionary<string, string>()
        {
            { "gasfired","gas(euro/MWh)" },
            { "turbojet","kerosine(euro/MWh)" }
        };
        public static List<Powerplant> OrderPowerPlantsByMerit(List<Powerplant> powerplants)
        {
            return powerplants.OrderBy(p => p.Cost).ToList();

        }
        public static void CalculatePowerPlantsCost(List<Powerplant> powerplants, Fuels fuels )
        {
           foreach(Powerplant powerPlant in powerplants)
            {
                if (powerPlant.Type == PowerPlantType.windturbine.ToString()) powerPlant.Cost = 0;
                else powerPlant.Cost=  GetCost(powerPlant.Efficiency, GetPrice(fuels, powerPlant.Type));
            }
        }
        private static double GetCost (double effcient, double price)
        {
            return Math.Round( price / effcient , 2);
        }
        private static double GetPrice(Fuels fuels, string powerPlantType)
        {
            FuelTypes.TryGetValue(powerPlantType, out var typePrice);
            switch (typePrice)
            {
                case "gas(euro/MWh)":
                    return fuels.GasPrice;
                case "kerosine(euro/MWh)":
                  return fuels.KerosinePrice;
                default : return 0;
            }
        }
    }
}

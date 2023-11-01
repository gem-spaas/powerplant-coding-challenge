using PowerPlant.Models;

namespace PowerPlant.Services;

public class PowerPlantService : IPowerPlantService
{
    public IEnumerable<Powerplant> OrderByMerit(IEnumerable<Powerplant> requestedLoadPowerplants, Fuels requestedFuels)
    {

        List<Powerplant> powerplants = new();
        foreach (var powerplant in requestedLoadPowerplants)
        {
            var costOfRunning = powerplant.Type switch
            {
                "gasfired" => requestedFuels.GaseuroMWh / powerplant.Efficiency + requestedFuels.Co2Euroton * 0.3,
                "turbojet" => requestedFuels.KerosineeuroMWh / powerplant.Efficiency,
                "wind" => 0,
                _ => powerplant.Efficiency
            };

            powerplants.Add(powerplant with { CostOfRunning = costOfRunning });

        }
        return powerplants
            .OrderBy(x => x.CostOfRunning);
    }
}
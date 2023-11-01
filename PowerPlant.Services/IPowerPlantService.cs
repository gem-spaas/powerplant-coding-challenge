using PowerPlant.Models;

namespace PowerPlant.Services;

public interface IPowerPlantService
{
    public IEnumerable<Powerplant> OrderByMerit(IEnumerable<Powerplant> requestedLoadPowerplants, Fuels requestedFuels);
}
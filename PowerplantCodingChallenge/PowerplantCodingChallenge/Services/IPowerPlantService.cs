using PowerplantCodingChallenge.Models;

namespace PowerplantCodingChallenge.Services
{
    public interface IPowerPlantService
    {
       public double GetPowerPlantMaxPower(List<PowerPlant> powerPlants, UnitCommitment powerPlantToActivate);
        List<UnitCommitment> GetPowerPlantsOrderByUnitCost(Payload powerPlants);
    }
}

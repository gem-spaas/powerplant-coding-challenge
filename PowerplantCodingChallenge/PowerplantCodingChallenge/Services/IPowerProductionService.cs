using PowerplantCodingChallenge.Models;


namespace PowerplantCodingChallenge.Services
{
    public interface IPowerProductionService
    {
        public Task<List<UnitCommitment>> GetPowerPlantsActivatedByUnitCost(Payload payload);
    }
}
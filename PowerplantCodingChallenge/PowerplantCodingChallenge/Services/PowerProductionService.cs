using PowerplantCodingChallenge.Models;

namespace PowerplantCodingChallenge.Services
{
    public class PowerProductionService : IPowerProductionService
    {
        private readonly IPowerPlantService _powerPlantService;

        private List<UnitCommitment> listPowerPlantsUnitCost;
        public PowerProductionService(IPowerPlantService powerPlantService)
        {
            this._powerPlantService = powerPlantService;
        }
        public Task<List<UnitCommitment>> GetPowerPlantsActivatedByUnitCost(Payload payload)
        {
            List<UnitCommitment> listResult = this.GetPowerPlantsActived(payload);
            return Task.FromResult(listResult);
        }

        private List<UnitCommitment> GetPowerPlantsActived(Payload payload)
        {
            double load = payload.Load;
            listPowerPlantsUnitCost = _powerPlantService.GetPowerPlantsOrderByUnitCost(payload);

            List<UnitCommitment> listPowerSupplyResponseActivated = new List<UnitCommitment>();

            while (load >= 0 && listPowerPlantsUnitCost.Count > 0)
            {

                UnitCommitment powerPlantToActivate = listPowerPlantsUnitCost.First();
                var powerPlantPowerMax = _powerPlantService.GetPowerPlantMaxPower(payload.PowerPlants, powerPlantToActivate);

                if (powerPlantToActivate.Activable == true)
                {
                    if ((load = load - powerPlantToActivate.Power) >= 0)
                    {

                        if (powerPlantPowerMax > load)
                        {
                            powerPlantToActivate.Power = load;
                            load = 0;
                        }
                        else
                        {
                            powerPlantToActivate.Power = powerPlantPowerMax;
                            load = load - powerPlantToActivate.Power;
                        }
                    }
                    else
                    {
                        powerPlantToActivate.Power = 0;
                        load = 0;
                    }
                }
                listPowerPlantsUnitCost.Remove(powerPlantToActivate);
                listPowerSupplyResponseActivated.Add(powerPlantToActivate);
            }

            return listPowerSupplyResponseActivated;
        }
    }
}

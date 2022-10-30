using PowerplantCodingChallenge.Models;

namespace PowerplantCodingChallenge.Services
{
    public class PowerPlantService : IPowerPlantService
    {
        public PowerPlantService()
        {
        }
        public double GetPowerPlantMaxPower(List<PowerPlant> powerPlants, UnitCommitment powerPlantToActivate)
        {
            return powerPlants.Find(powerPlant => powerPlant.Name == powerPlantToActivate.Name).PowerMax;
        }
        private List<UnitCommitment> OrderCommintmentsByUnitCost(List<UnitCommitment> unitCommitments)
        {
            return unitCommitments.OrderBy(powerPlant => powerPlant.UnitCost).ToList();
        }
        public List<UnitCommitment> GetPowerPlantsOrderByUnitCost(Payload payload)
        {
            List<UnitCommitment> powerPlantsUnitCosts = new List<UnitCommitment>();
            UnitCommitment powerSupplyResponse;
            foreach (var powerPlant in payload.PowerPlants)
            {
                switch (powerPlant.Type.ToString().ToLower())
                {
                    case "gasfired":
                        var gasfiredUnitCost = this.GetPowerUnitCostTotalEfficiency(powerPlant.Efficiency, payload.Fuels.Gasfired);
                        powerSupplyResponse = new UnitCommitment(powerPlant.Name, gasfiredUnitCost, true);
                        powerPlantsUnitCosts.Add(powerSupplyResponse);
                        break;
                    case "turbojet":
                        var unitTurbojetCost = this.GetPowerUnitCostTotalEfficiency(powerPlant.Efficiency, payload.Fuels.Turbojet);
                        powerSupplyResponse = new UnitCommitment(powerPlant.Name, unitTurbojetCost, true);
                        powerPlantsUnitCosts.Add(powerSupplyResponse);
                        break;
                    case "windturbine":
                        var ativate = (payload.Fuels.Windturbine > 0 ? true : false);
                        powerSupplyResponse = new UnitCommitment(powerPlant.Name, 0, ativate);
                        powerPlantsUnitCosts.Add(powerSupplyResponse);
                        break;
                    default:
                        break;
                }
            }
            return this.OrderCommintmentsByUnitCost(powerPlantsUnitCosts);
        }
        private double GetPowerUnitCostTotalEfficiency(double loadEfficiency, double loadFuel)
        {
            double totalCostPerUnit;

            double costBasedOnEfficiency = loadFuel / loadEfficiency;
            double additionalEfficiencyCost = 1 - loadEfficiency;
            double additionalCostToUnitCost = loadFuel / additionalEfficiencyCost;

            totalCostPerUnit = costBasedOnEfficiency + additionalCostToUnitCost;

            return totalCostPerUnit;
        }
    }
}

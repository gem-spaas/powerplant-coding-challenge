using PowerplantCodingChallenge.Models;

namespace PowerplantCodingChallenge.Services
{
    public class PowerProductionService : IPowerProductionService
    {
        public PowerProductionService()
        {
        }
        public Task<List<PayloadResponse>> GetPowerSupply(Payload powerSupplyDemand)
        {
            List<PayloadResponse> listResult = this.GetPowerPlantsActived(powerSupplyDemand);
            return Task.FromResult(listResult);
        }

        private List<PayloadResponse> GetPowerPlantsActived(Payload powerSupplyDemand)
        {
            double load = powerSupplyDemand.Load;
            var listPowerSuplies = this.GetPowerUnitCosts(powerSupplyDemand);
            List<PayloadResponse> listPowerSupplyResponseActivated = new List<PayloadResponse>();
            while (load >= 0 && listPowerSuplies.Count > 0)
            {
                var powerPlantToActivate = listPowerSuplies.OrderBy(x => x.UnitCost).First();
                listPowerSuplies.Remove(powerPlantToActivate);
                if (powerPlantToActivate.Activable == true)
                {
                    if ((load = load - powerPlantToActivate.P) >= 0)
                    {

                        if (GetPowerPlantMaxPower(powerSupplyDemand, powerPlantToActivate) > load)
                        {
                            powerPlantToActivate.P = load;
                            load = 0;
                        }
                        else
                        {
                            powerPlantToActivate.P = GetPowerPlantMaxPower(powerSupplyDemand, powerPlantToActivate);
                            load = load - powerPlantToActivate.P;
                        }
                    }
                    else
                    {
                        powerPlantToActivate.P = 0;
                        load = 0;
                    }
                }
                listPowerSupplyResponseActivated.Add(powerPlantToActivate);
            }

            return listPowerSupplyResponseActivated;
        }

        private static int GetPowerPlantMaxPower(Payload powerSupplyDemand, PayloadResponse? powerPlantToActivate)
        {
            return (powerSupplyDemand.PowerPlants.Find(x => x.Name == powerPlantToActivate.Name).PowerMax);
        }

        private List<PayloadResponse> GetPowerUnitCosts(Payload powerSupplyDemand)
        {
            var powerPlantsUnitCosts = new List<PayloadResponse>();
            PayloadResponse powerSupplyResponse;
            foreach (var powerPlant in powerSupplyDemand.PowerPlants)
            {
                switch (powerPlant.Type.ToString().ToLower())
                {
                    case "gasfired":
                        powerSupplyResponse = new PayloadResponse();
                        powerSupplyResponse.UnitCost = this.GetUnitCost(powerPlant.Efficiency, powerSupplyDemand.Fuels.Gasfired);
                        powerSupplyResponse.Name = powerPlant.Name;
                        powerSupplyResponse.Activable = true;
                        powerPlantsUnitCosts.Add(powerSupplyResponse);
                        break;
                    case "turbojet":
                        powerSupplyResponse = new PayloadResponse();
                        powerSupplyResponse.UnitCost = this.GetUnitCost(powerPlant.Efficiency, powerSupplyDemand.Fuels.Turbojet);
                        powerSupplyResponse.Name = powerPlant.Name;
                        powerSupplyResponse.Activable = true;
                        powerPlantsUnitCosts.Add(powerSupplyResponse);
                        break;
                    case "windturbine":
                        powerSupplyResponse = new PayloadResponse();
                        double unitCost = this.GetUnitCost(powerPlant.Efficiency, powerSupplyDemand.Fuels.Windturbine);
                        if (unitCost > 0)
                        {
                            powerSupplyResponse.Activable = true;
                        }
                        powerSupplyResponse.UnitCost = 0;
                        powerSupplyResponse.Name = powerPlant.Name;

                        powerPlantsUnitCosts.Add(powerSupplyResponse);
                        break;
                    default:
                        break;
                }
            }
            return powerPlantsUnitCosts;
        }

        private double GetUnitCost(double loadEfficiency, double loadCost)
        {
            double totalCostPerUnit;

            double costBasedOnEfficiency = loadCost / loadEfficiency;
            double additionalEfficiencyCost = 1 - loadEfficiency;
            double additionalCostToUnitCost = loadCost / additionalEfficiencyCost;

            totalCostPerUnit = costBasedOnEfficiency + additionalCostToUnitCost;

            return totalCostPerUnit;

        }
    }
}

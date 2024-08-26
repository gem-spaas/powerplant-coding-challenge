using PowerPlantCodingChallenge.ProductionPlan;

namespace PowerPlantCodingChallenge.Services
{
    public class ProductionPlanService : IProductionPlanService
    {
        public ProductionPlanService() { }

        public IEnumerable<UnitCommitment> GenerateProductionPlan(PowerLoadForecast forecast)
        {
            IEnumerable<UnitCommitmentEstimation> unitCommitmentEstimations = GetUnitCommitmentEstimations(forecast);

            IEnumerable<UnitCommitment> productionPlan = GetUnitCommitmentsByMeritOrder(forecast.Load!.Value, unitCommitmentEstimations);

            return productionPlan;
        }

        private IEnumerable<UnitCommitmentEstimation> GetUnitCommitmentEstimations(PowerLoadForecast forecast)
        {
            var unitCommitmentEstimations = new List<UnitCommitmentEstimation>();

            foreach (var powerPlant in forecast.PowerPlants)
            {
                unitCommitmentEstimations.Add(new UnitCommitmentEstimation()
                {
                    Name = powerPlant.Name,
                    CostPerMWh = CalculatePowerPlantCostPerMWh(forecast.Fuels, powerPlant),
                    Pmin = powerPlant.Pmin!.Value,
                    Pmax = CalculatePowerPlantPmax(forecast.Fuels, powerPlant),
                });
            }

            return unitCommitmentEstimations;
        }

        private IEnumerable<UnitCommitment> GetUnitCommitmentsByMeritOrder(float load, IEnumerable<UnitCommitmentEstimation> unitCommitmentEstimations)
        {
            var unitCommitments = new List<UnitCommitment>();

            unitCommitmentEstimations = unitCommitmentEstimations.OrderByDescending(x => x.Pmin).OrderBy(x => x.CostPerMWh).ToArray();

            foreach (var unitCommitmentEstimation in unitCommitmentEstimations)
            {
                unitCommitments.Add(new UnitCommitment()
                {
                    Name = unitCommitmentEstimation.Name,
                    Power = FillLoadWithPowerPlantPower(ref load, unitCommitmentEstimation.Pmax)
                });
            }

            return unitCommitments;
        }

        private float CalculatePowerPlantCostPerMWh(Fuels fuels, PowerPlant powerPlant)
        {
            float cost = 0;

            if (powerPlant.PowerType == PowerPlantPowerType.WindTurbine)
            {
                return cost;
            }
            else if (powerPlant.PowerType == PowerPlantPowerType.GasFired)
            {
                cost = fuels.GasCostPerMWh!.Value / powerPlant.Efficiency!.Value;
                cost += fuels.Co2EmissionCostPerTon!.Value * 0.3F;
            }
            else if (powerPlant.PowerType == PowerPlantPowerType.TurboJet)
            {
                cost = fuels.KerosineCostPerMWh!.Value / powerPlant.Efficiency!.Value;
            }

            return cost;
        }

        private float CalculatePowerPlantPmax(Fuels fuels, PowerPlant powerPlant)
        {
            if (powerPlant.PowerType != PowerPlantPowerType.WindTurbine)
                return powerPlant.Pmax!.Value;

            return powerPlant.Pmax!.Value * (fuels.WindPercentage!.Value / 100);
        }

        private float FillLoadWithPowerPlantPower(ref float load, float power)
        {
            if (load > power)
            {
                load -= power;
            }
            else
            {
                power = load;
                load = 0;
            }

            return MathF.Round(power, 1);
        }
    }
}



namespace WebApplicationOssia.Models
{
    public class ProductionPlanManager
    {
        #region Private Constants
        private const string  _GasFired = "gasfired";
        private const string  _Turbojet = "turbojet";
        private const string  _WindTurbine = "windturbine";
        private const double  _Co2_Emission_GasFired = 0.3;
        private const double  _Co2_Emission_Turbojet = 0.6;
        private const double  _Co2_Emission_WindTurbine = 0;
        #endregion
        #region Private Fields
        private Payload _payload;
        #endregion
        #region Private Methods
        private void GetValuestoTypeFuels(Powerplant powerplant)
        {
            powerplant.Availabilty = 100;

            try
            {
                if (powerplant.Type.Equals(_GasFired, StringComparison.OrdinalIgnoreCase))
                {
                    powerplant.CostFuel = _payload.Fuels.Gas;
                    powerplant.CostCo2 = _payload.Fuels.Co2;
                    powerplant.Co2Emission = _Co2_Emission_GasFired;
                }
                else if (powerplant.Type.Equals(_Turbojet, StringComparison.OrdinalIgnoreCase))
                {
                    powerplant.CostFuel = _payload.Fuels.Kerosine;
                    powerplant.CostCo2 = _payload.Fuels.Co2;
                    powerplant.Co2Emission = _Co2_Emission_Turbojet;
                }
                else if (powerplant.Type.Equals(_WindTurbine, StringComparison.OrdinalIgnoreCase))
                {
                    powerplant.Availabilty = _payload.Fuels.Wind;
                    powerplant.CostFuel = 0;
                    powerplant.CostCo2 = 0;
                    powerplant.Co2Emission = _Co2_Emission_WindTurbine;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }

            
        }
        #endregion
        #region Constructors
        public ProductionPlanManager(Payload payload)
        {
            _payload = payload;
        }
        #endregion
        #region Public Methods
        
        
        public ProductionPlan CalculatePowerProduction()
        {
            try
            {
                ProductionPlan productionPlan = new ProductionPlan()
                { Id = 1, ProductionPlanItems = new List<ProductionPlanItem>(), Result = string.Empty };

                // Calculate cost for each powerplant
                foreach (Powerplant powerplant in _payload.PowerPlants)
                {
                    GetValuestoTypeFuels(powerplant);
                    powerplant.CostToGeneratePower = 1 / powerplant.Efficiency * powerplant.CostFuel;
                    powerplant.CostToGeneratePower += (powerplant.Co2Emission * powerplant.CostCo2);
                }

                // Merit-order by Powerplants
                _payload.PowerPlants = _payload.PowerPlants.OrderBy(ppl => ppl.CostToGeneratePower).ToList();

                // Calculate production plan
                double powerToProvide = _payload.Load;
                for (int idx = 0; idx < _payload.PowerPlants.Count; idx++)
                {
                    Powerplant powerplant = _payload.PowerPlants[idx];
                    double power = 0;
                    if (powerToProvide >= powerplant.Pmin)
                    {
                        power = powerToProvide >= powerplant.Pmax * (powerplant.Availabilty / 100) ?
                                   powerplant.Pmax * (powerplant.Availabilty / 100) : powerToProvide;

                        powerToProvide -= power;
                    }
                    productionPlan.ProductionPlanItems.Add(new ProductionPlanItem() { Name = powerplant.Name, P = Math.Round(power, 1) });
                }

                return productionPlan;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }

            
        }
        #endregion
    }
}

using CalculatePowerGenerationByPowerPlants.Constants;
using CalculatePowerGenerationByPowerPlants.Models;

namespace CalculatePowerGenerationByPowerPlants.Validators
{
    public class ProductionPlanRequestValidator : IPostRequestValidator<ProductionPlanRequest>
    {
        public void Validate(ProductionPlanRequest request)
        {
            //Load validation
            if (request.Load <= 0)
            {
                throw new ArgumentException("Load requested should be greater than 0.");
            }

            //Fuels validation
            if (request.Fuels == null || request.Fuels.Count == 0)
            {
                throw new ArgumentException("No Fuel details provided.");
            }

            //Power plants validation
            if (request.Powerplants == null || request.Powerplants.Count == 0)
            {
                throw new ArgumentException("No power plant details provided.");
            }

            var powerplantsWithoutWindTurbine = request.Powerplants.Where(p => p.Type != PowerPlantTypeConstants.WindTurbine);
            var powerplantsOnlyWindTurbine = request.Powerplants.Where(p => p.Type == PowerPlantTypeConstants.WindTurbine);
            var pmaxWithoutWindTurbine = Convert.ToDouble(powerplantsWithoutWindTurbine?.Select(p => p.Pmax).Sum());
            double pmaxTotal = pmaxWithoutWindTurbine;

            //Validate pmin and pmax wrt load requested
            if (request.Fuels.ContainsKey(FuelConstants.Wind))
            {
                var wind = request.Fuels[FuelConstants.Wind];
                if (wind > 0)
                {
                    var pminAllPowerPlants = request.Powerplants.Select(p => p.Pmin);
                    ValidateWithWind(pmaxTotal, request.Load, wind, pminAllPowerPlants, powerplantsOnlyWindTurbine);
                }
                else
                {
                    ValidateWithoutWind(pmaxTotal, request.Load, powerplantsWithoutWindTurbine);
                }
            }
            else
            {
                ValidateWithoutWind(pmaxTotal, request.Load, powerplantsWithoutWindTurbine);
            }
        }

        private void ValidateWithWind(double pmaxTotal, double load, double wind, IEnumerable<int> pminAllPowerPlants, IEnumerable<PowerPlant>? powerplantsOnlyWindTurbine)
        {
            //pmax validation
            var pmaxOnlyWindTurbines = powerplantsOnlyWindTurbine?.Select(p => p.Pmax);
            if (pmaxOnlyWindTurbines != null && pmaxOnlyWindTurbines.Count() > 0)
            {
                foreach (var pmaxWindTurbine in pmaxOnlyWindTurbines)
                {
                    pmaxTotal += pmaxWindTurbine * (wind / 100);
                }
                if (pmaxTotal < load)
                {
                    throw new ArgumentException("Load requested is greater than sum of all the provided power plants maximum power production.");
                }
            }
            else
            {
                if (pmaxTotal < load)
                {
                    throw new ArgumentException("Load requested is greater than sum of all the provided power plants maximum power production.");
                }
            }
        }

        private void ValidateWithoutWind(double pmaxTotal, double load, IEnumerable<PowerPlant>? powerplantsWithoutWindTurbine)
        {
            //pmax validation
            if (pmaxTotal < load)
            {
                throw new ArgumentException("Load requested is greater than sum of all the provided power plants maximum power production.");
            }

            //pmin validation
            var pminPowerPlantsWithoutWindTurbine = powerplantsWithoutWindTurbine?.Select(x => x.Pmin);
            if (pminPowerPlantsWithoutWindTurbine?.All(pmin => pmin > load) ?? false)
            {
                throw new ArgumentException("Load requested is less than pmin of all the provided power plants.");

            }
        }
    }
}

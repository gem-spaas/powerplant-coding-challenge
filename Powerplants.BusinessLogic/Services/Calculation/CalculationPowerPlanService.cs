using BusinessLogic.Enums;
using BusinessLogic.Models;
using BusinessLogic.Records;

namespace BusinessLogic.Services.Calculation
{
    public class CalculationPowerPlanService : ICalculationPowerPlanService
    {
        private const decimal CO2Mwh = 0.3m;
        public List<ProductResponse> CalculatePowerPlants(ProductPlanInput dto)
        {
            List<ProductResponse> result = new List<ProductResponse>();
            if (dto.Load == 0)
            {
                dto.Powerplants.ForEach(powerPlant => AddProduct(powerPlant.Name, 0, result));
                return result;
            }

            var groupedPowerPlants = dto.Powerplants.GroupBy(x => x.Type);


            CalculatePowerWindTubine(groupedPowerPlants.Where(x => x.Key == TypePowerEnum.Windturbine).SelectMany(x => x), result, dto.Fuels, dto.Load);


            var prices = CalculatePricesNotWindTubines(dto, groupedPowerPlants);

            CalculatePowerNotWindTurbines(dto, result, prices);

            return result;
        }

        private List<PriceMwh> CalculatePricesNotWindTubines(ProductPlanInput dto, IEnumerable<IGrouping<TypePowerEnum, PowerPlantInput>> groupedPowerPlants)
        {
            List<PriceMwh> prices = new List<PriceMwh>();
            foreach (var item in groupedPowerPlants.Where(x => x.Key != TypePowerEnum.Windturbine).SelectMany(x => x))
            {
                if (item.Type == TypePowerEnum.Gasfired)
                {
                    if(item.Efficiency == 0)
                    {
                        prices.Add(new PriceMwh(decimal.MaxValue, item.Name));
                        continue;
                    }
                    var pricePerMhw = dto.Fuels.GasEuroPerMWh * CO2Mwh;
                    prices.Add(new PriceMwh(dto.Fuels.GasEuroPerMWh / item.Efficiency + pricePerMhw, item.Name));
                }
                else
                {
                    prices.Add(new PriceMwh(dto.Fuels.KerosineEuroPerMWh, item.Name));
                }
            }
            return prices;
        }

        private void CalculatePowerWindTubine(IEnumerable<PowerPlantInput> windTurbines, List<ProductResponse> resultList, FuelInput fuels, int load)
        {
            if (windTurbines != null)
            {
                if(fuels.WindPercentage > 0)
                {
                    var sum = resultList.Sum(x => x.P);
                    foreach (var wind in windTurbines)
                    {
                        var maxWindPower = wind.Pmax * (fuels.WindPercentage / 100);
                        if (load <= sum + maxWindPower)
                        {
                            AddProduct(wind.Name, load - sum, resultList);
                            break;
                        }
                        AddProduct(wind.Name, maxWindPower, resultList);
                        sum = sum + maxWindPower;
                    }
                }
                else
                {
                    windTurbines.ToList().ForEach(powerPlant => AddProduct(powerPlant.Name, 0, resultList));
                }

            }
        }
        private void CalculatePowerNotWindTurbines(ProductPlanInput dto, List<ProductResponse> resultList, List<PriceMwh> prices)
        {
            if (dto.Powerplants != null)
            {
                bool isNextEmpty = false;
                var sum = resultList.Sum(x => x.P);
                prices = prices.OrderBy(x => x.Price).ThenBy(x => x.Name).ToList();
                foreach (var price in prices)
                {
                    if (isNextEmpty)
                    {
                        AddProduct(price.Name, 0, resultList);
                        continue;
                    }
                    var plant = dto.Powerplants.First(x => x.Name == price.Name && x.Type != Enums.TypePowerEnum.Windturbine);

                    if (dto.Load <= sum + plant.Pmax)
                    {
                        AddProduct(plant.Name, dto.Load - sum, resultList);
                        isNextEmpty = true;
                        continue;
                    }
                    AddProduct(plant.Name, plant.Pmax, resultList);
                    sum = sum + plant.Pmax;
                }
            }
        }
        private void AddProduct(string name, decimal price, List<ProductResponse> resultList)
        {
            resultList.Add(new ProductResponse()
            {
                Name = name,
                P = price
            });
        }
    }

}

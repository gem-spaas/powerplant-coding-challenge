using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using ProductionPlan.Models;

namespace ProductionPlan;

public interface IProductionPlan
{
    IReadOnlyCollection<ProductionResponseDto> GetProductionPlan(ProductionRequestDto request);
}

public class ProductionPlan : IProductionPlan
{
    private const double Co2GeneratedPerMWh = 0.3;
    private readonly ILogger<ProductionPlan> _logger;
    public ProductionPlan(ILogger<ProductionPlan> logger)
    {
        _logger = logger;
    }
    public IReadOnlyCollection<ProductionResponseDto> GetProductionPlan(ProductionRequestDto productionRequest)
    {
        if (productionRequest == null)
        {
            _logger.LogError("The production request should not be null");
            throw new ArgumentNullException(nameof(productionRequest), "The production request should not be null");
        }

        var powerPlantLoadsByMeritOrder = productionRequest.Powerplants
            .OrderBy(p => CalculateCostOfOperationPerMWh(p, productionRequest.Fuels))
            .ThenByDescending(p => p.Pmax)
            .ToList();

        var energyMin = powerPlantLoadsByMeritOrder.Min(x => x.Pmin);

        if (productionRequest.Load < energyMin)
        {
            _logger.LogError($"All power plants have Pmin greater than requested load: {productionRequest.Load} MWh");
            throw new InvalidOperationException(
                $"All power plants have Pmin greater than requested load: {productionRequest.Load} MWh");
        }

        var energyMax = powerPlantLoadsByMeritOrder.Sum(x => x.Pmax);

        if (productionRequest.Load > energyMax)
        {
            _logger.LogError($"Couldn't generate load: {productionRequest.Load} MWh with available plants");
            throw new InvalidOperationException(
                $"Couldn't generate load: {productionRequest.Load} MWh with available plants");
        }

        var response = new List<ProductionResponseDto>();
        var requiredLoad = productionRequest.Load;

        // 1. Use wind turbines if available
        requiredLoad = GetLoadFromWindTurbine(productionRequest, powerPlantLoadsByMeritOrder, requiredLoad, response);

        // 2. Use gas fired plant if more load required
        requiredLoad = GetLoadFromGasPlants(requiredLoad, powerPlantLoadsByMeritOrder, response);

        // 3. Use turbo jet if more load required
        GetLoadFromTurbojetPlants(requiredLoad, powerPlantLoadsByMeritOrder, response);

        return response;
    }

    private static void GetLoadFromTurbojetPlants(int requiredLoad, IEnumerable<PowerPlant> powerPlantLoadsByMeritOrder, ICollection<ProductionResponseDto> response)
    {
        if (requiredLoad <= 0)
        {
            return;
        }
        var turbojetPlants = powerPlantLoadsByMeritOrder.Where(x => x.Type == PlantType.TurboJet).ToList();
        if (!turbojetPlants.Any())
        {
            return;
        }
        foreach (var turbojetPlant in turbojetPlants)
        {
            if (requiredLoad > 0)
            {
                if (requiredLoad - turbojetPlant.Pmax >= 0)
                {
                    // required load is more the plant maximum capacity
                    response.Add(new ProductionResponseDto()
                    {
                        Name = turbojetPlant.Name,
                        P = turbojetPlant.Pmax,
                    });
                    requiredLoad -= turbojetPlant.Pmax;
                }
                else
                {
                    response.Add(new ProductionResponseDto()
                    {
                        Name = turbojetPlant.Name,
                        P = requiredLoad,
                    });
                    requiredLoad = 0;
                }
            }
            else
            {
                break;
            }
        }
    }

    private static int GetLoadFromGasPlants(int requiredLoad, IEnumerable<PowerPlant> powerPlantLoadsByMeritOrder, IList<ProductionResponseDto> response)
    {
        if (requiredLoad <= 0)
        {
            return requiredLoad;
        }
        var gasPlants = powerPlantLoadsByMeritOrder.Where(x => x.Type == PlantType.GasFired).ToList();
        if (!gasPlants.Any())
        {
            return requiredLoad;
        }
        var energyMin = gasPlants.Min(x => x.Pmin);
        if (requiredLoad < energyMin)
        {
            while (requiredLoad <= energyMin)
            {
                requiredLoad += response.Last().P;
                response.RemoveAt(response.Count - 1);
            }
        }

        foreach (var gasPlant in gasPlants)
        {
            if (requiredLoad > 0)
            {
                // required load is within gas plant capacity
                if (requiredLoad >= gasPlant.Pmin && requiredLoad <= gasPlant.Pmax)
                {
                    response.Add(new ProductionResponseDto()
                    {
                        Name = gasPlant.Name,
                        P = requiredLoad,
                    });
                    requiredLoad = 0;
                }
                else
                {
                    // required load is more the plant maximum capacity
                    if (requiredLoad - gasPlant.Pmax >= 0)
                    {
                        response.Add(new ProductionResponseDto()
                        {
                            Name = gasPlant.Name,
                            P = gasPlant.Pmax,
                        });
                        requiredLoad -= gasPlant.Pmax;
                    }
                    else
                    {
                        response.Add(new ProductionResponseDto()
                        {
                            Name = gasPlant.Name,
                            P = requiredLoad,
                        });
                        requiredLoad = 0;
                    }
                }
            }
            else
            {
                break;
            }
        }

        return requiredLoad;
    }

    private static int GetLoadFromWindTurbine(ProductionRequestDto productionRequest, IEnumerable<PowerPlant> powerPlantLoadsByMeritOrder, int requiredLoad, ICollection<ProductionResponseDto> response)
    {
        var windTurbines = powerPlantLoadsByMeritOrder.Where(x => x.Type == PlantType.WindTurbine).ToList();
        if (!windTurbines.Any() || productionRequest.Fuels.Wind <= 0)
        {
            return requiredLoad;
        }

        windTurbines = windTurbines.OrderByDescending(x => x.Pmax * productionRequest.Fuels.Wind / 100).ToList();
        var energyWithoutCost = windTurbines.Sum(x => x.Pmax * productionRequest.Fuels.Wind / 100);

        if (energyWithoutCost < productionRequest.Load)
        {
            foreach (var turbine in windTurbines)
            {
                if (requiredLoad > 0)
                {
                    var turbineLoad = turbine.Pmax * productionRequest.Fuels.Wind / 100;
                    if (requiredLoad - turbineLoad >= 0)
                    {
                        response.Add(new ProductionResponseDto()
                        {
                            Name = turbine.Name,
                            P = turbineLoad,
                        });
                        requiredLoad -= turbineLoad;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
        }

        return requiredLoad;
    }

    private static double CalculateCostOfOperationPerMWh(PowerPlant powerPlant, Fuels fuels)
    {
        var pricePerMWh = powerPlant.Type switch
        {
            PlantType.WindTurbine => 0,
            PlantType.GasFired => fuels.GasEuroMWh + Co2GeneratedPerMWh * fuels.Co2EuroTon,
            PlantType.TurboJet => fuels.KerosineEuroMWh,
            _ => throw new ArgumentOutOfRangeException(nameof(powerPlant),
                $"{powerPlant.Type} is an unknown power plant type")
        };

        return pricePerMWh / powerPlant.Efficiency;
    }
}
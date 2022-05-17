using CalculatePowerGenerationByPowerPlants.Constants;
using CalculatePowerGenerationByPowerPlants.Models;

namespace CalculatePowerGenerationByPowerPlants.Helpers
{
    public class MeritOrderHelper
    {
        public static List<ProductionPlanResponse> CalculateProductionPlanByMeritOrder(ProductionPlanRequest request)
        {
            //create initial merit order based on cost per MWh 
            var intialMeritOrders = new List<MeritOrder>();
            if (request.Fuels.ContainsKey(FuelConstants.Wind))
            {
                var wind = request.Fuels[FuelConstants.Wind];
                if (wind > 0)
                {
                    CreateInitialMeritOrder(request.Powerplants, request.Fuels, intialMeritOrders);
                }
                else
                {
                    var powerplantsWithoutWindTurbine = request.Powerplants.Where(p => p.Type != PowerPlantTypeConstants.WindTurbine);
                    CreateInitialMeritOrder(powerplantsWithoutWindTurbine, request.Fuels, intialMeritOrders);
                }
            }
            else
            {
                var powerplantsWithoutWindTurbine = request.Powerplants.Where(p => p.Type != PowerPlantTypeConstants.WindTurbine);
                CreateInitialMeritOrder(powerplantsWithoutWindTurbine, request.Fuels, intialMeritOrders);
            }

            //sort by cost per MWh asc
            var sortedMeritOrders = intialMeritOrders.OrderBy(mo => mo.CostPerMWh).ToList();

            //if no gasfired type(all Pmin will be zero)
            if (sortedMeritOrders.All(x => x.PlantType != PowerPlantTypeConstants.GasFired))
            {
                return HandleWhenNoGasFiredType(request.Load, sortedMeritOrders);
            }
            else
            {
                // select plants which can run full Pmax
                var selectedPlants = ChooseFixedPlantsBasedOnMeritOrder(request.Load, sortedMeritOrders);

                //No plants selected 
                if (selectedPlants.Count == 0)
                {
                    var removedSortedMeritedOrders = sortedMeritOrders.Where(x => x.PlantType != PowerPlantTypeConstants.GasFired);
                    return HandleWhenNoGasFiredType(request.Load, removedSortedMeritedOrders);
                }
                //if selected plant has no gas plant then do simple calculation
                else if(selectedPlants.All(x => x.PlantType != PowerPlantTypeConstants.GasFired))
                {
                    return HandleWhenNoGasFiredType(request.Load, selectedPlants);
                }
                else
                {
                    //if selected plants give requested load,return response
                    var totalSelectedPmax = selectedPlants.Select(x => x.Pmax).Sum();
                    if (totalSelectedPmax >= request.Load)
                    {
                        List<ProductionPlanResponse> response = new List<ProductionPlanResponse>();
                        foreach (var plant in selectedPlants)
                        {
                            response.Add(new ProductionPlanResponse
                            {
                                Name = plant.PlantName,
                                P = plant.Pmax
                            });
                        }
                        return response;
                    }
                    else
                    {
                        //if needs to find cost effective solution, create a comparer
                        return PlantComparerBasedOnCost(selectedPlants, sortedMeritOrders, request.Load, totalSelectedPmax);
                    }
                }
            }
        }

        //calculate cost per MWh based on efficiency and cost. Wind is considered as 100% efficient and zero cost
        private static void CreateInitialMeritOrder(IEnumerable<PowerPlant> powerPlants, Dictionary<string, double> fuels, List<MeritOrder> meritOrders)
        {
            foreach (var powerPlant in powerPlants)
            {
                if (powerPlant.Type == PowerPlantTypeConstants.WindTurbine)
                {
                    if (fuels.ContainsKey(FuelConstants.Wind))
                    {
                        meritOrders.Add(new MeritOrder
                        {
                            PlantName = powerPlant.Name,
                            PlantType = PowerPlantTypeConstants.WindTurbine,
                            CostPerMWh = 0,
                            CostPmin = 0,
                            CostPmax = 0,
                            Pmin = powerPlant.Pmin,
                            Pmax = Math.Round((powerPlant.Pmax * (fuels[FuelConstants.Wind] / 100)), MidpointRounding.ToZero)
                        });
                    }
                    else
                    {
                        throw new ArgumentException($"No % available for {PowerPlantTypeConstants.WindTurbine} to calculate power production plan.");
                    }
                }
                else
                {
                    if (powerPlant.Type == PowerPlantTypeConstants.Turbojet)
                    {
                        if (fuels.ContainsKey(FuelConstants.Kerosine))
                        {
                            var costPerMWh = fuels[FuelConstants.Kerosine] / powerPlant.Efficiency;
                            meritOrders.Add(new MeritOrder
                            {
                                PlantName = powerPlant.Name,
                                PlantType = PowerPlantTypeConstants.Turbojet,
                                CostPerMWh = costPerMWh,
                                CostPmin = powerPlant.Pmin * costPerMWh,
                                CostPmax = powerPlant.Pmax * costPerMWh,
                                Pmin = powerPlant.Pmin,
                                Pmax = powerPlant.Pmax
                            });
                        }
                        else
                        {
                            throw new ArgumentException($"No euro/MWh available for {PowerPlantTypeConstants.Turbojet} to calculate power production plan.");
                        }
                    }
                    else
                    {
                        if (fuels.ContainsKey(FuelConstants.Gas))
                        {
                            var costPerMWh = fuels[FuelConstants.Gas] / powerPlant.Efficiency;
                            meritOrders.Add(new MeritOrder
                            {
                                PlantName = powerPlant.Name,
                                PlantType = PowerPlantTypeConstants.GasFired,
                                CostPerMWh = costPerMWh,
                                CostPmin = powerPlant.Pmin * costPerMWh,
                                CostPmax = powerPlant.Pmax * costPerMWh,
                                Pmin = powerPlant.Pmin,
                                Pmax = powerPlant.Pmax
                            });
                        }
                        else
                        {
                            throw new ArgumentException($"No euro/MWh available for {PowerPlantTypeConstants.GasFired} to calculate power production plan.");
                        }
                    }
                }
            }
        }

        //choose plants which can run full pmin/pmax
        private static List<MeritOrder> ChooseFixedPlantsBasedOnMeritOrder(double requestLoad, List<MeritOrder> sortedMeritOrders)
        {
            double totalPowerGenerated = 0;
            double totalPowerRequested = requestLoad;
            var selectedPlants = new List<MeritOrder>();
            var selectingTurbinesCompleted = false;

            //handle wind plants separately as they are more efficient
            var zeroCostMWhPlants = sortedMeritOrders.Where(smo => smo.CostPerMWh == 0).ToList();
            if (zeroCostMWhPlants != null && zeroCostMWhPlants.Count() > 0)
            {
                foreach (var zeroCostMWhPlant in zeroCostMWhPlants)
                {
                    //if total power requested less than pmax of wind
                    if (totalPowerRequested < zeroCostMWhPlant.Pmax)
                    {
                        totalPowerGenerated += totalPowerRequested;
                        selectedPlants.Add(zeroCostMWhPlant);
                        sortedMeritOrders.Remove(zeroCostMWhPlant);
                        selectingTurbinesCompleted = true;
                        break;
                    }
                    else
                    {
                        totalPowerGenerated += zeroCostMWhPlant.Pmax;
                        totalPowerRequested -= zeroCostMWhPlant.Pmax;
                        selectedPlants.Add(zeroCostMWhPlant);
                        sortedMeritOrders.Remove(zeroCostMWhPlant);
                    }
                }
            }

            //when base plant selection not completed
            while (!selectingTurbinesCompleted)
            {
                foreach (var meritOrder in sortedMeritOrders)
                {
                    if (!selectedPlants.Any(x => x.PlantType != PowerPlantTypeConstants.WindTurbine))
                    {
                        if (meritOrder.Pmin >= 0 && meritOrder.Pmin < totalPowerRequested)
                        {
                            if (totalPowerRequested < meritOrder.Pmax)
                            {
                                totalPowerGenerated += totalPowerRequested;
                                meritOrder.Pmax = totalPowerRequested;
                                selectedPlants.Add(meritOrder);
                                selectingTurbinesCompleted = true;
                                break;
                            }
                            else
                            {
                                totalPowerGenerated += meritOrder.Pmax;
                                selectedPlants.Add(meritOrder);
                                sortedMeritOrders.Remove(meritOrder);
                                if (selectedPlants.Select(x => x.Pmax).Sum() >= requestLoad)
                                {
                                    selectingTurbinesCompleted = true;
                                }
                                else
                                {
                                    totalPowerRequested -= meritOrder.Pmax;
                                }
                                break;
                            }
                        }
                    }
                    else
                    {
                        if (totalPowerRequested > meritOrder.Pmax)
                        {
                            totalPowerGenerated += meritOrder.Pmax;
                            selectedPlants.Add(meritOrder);
                            sortedMeritOrders.Remove(meritOrder);
                            if (selectedPlants.Select(x => x.Pmax).Sum() >= requestLoad)
                            {
                                selectingTurbinesCompleted = true;
                            }
                            else
                            {
                                totalPowerRequested -= meritOrder.Pmax;
                            }
                            break;
                        }
                        else
                        {
                            selectingTurbinesCompleted = true;
                            break;
                        }
                    }
                }
            }
            return selectedPlants;
        }

        //when no gas plants are required, calculation is straight forward
        private static List<ProductionPlanResponse> HandleWhenNoGasFiredType(double requestLoad, IEnumerable<MeritOrder> sortedMeritOrders)
        {
            List<ProductionPlanResponse> response = new List<ProductionPlanResponse>();
            var loadRequested = requestLoad;
            foreach (var order in sortedMeritOrders)
            {
                if (loadRequested <= order.Pmax)
                {
                    response.Add(new ProductionPlanResponse
                    {
                        Name = order.PlantName,
                        P = loadRequested
                    });
                    break;
                }
                else
                {
                    response.Add(new ProductionPlanResponse
                    {
                        Name = order.PlantName,
                        P = order.Pmax
                    });
                    loadRequested -= order.Pmax;
                }
            }
            return response;
        }

        //compare combos with fixed plant A and changing plant B to find cost effective response 
        private static List<ProductionPlanResponse> PlantComparerBasedOnCost(List<MeritOrder> selectedPlants, IEnumerable<MeritOrder> sortedMeritOrders, double requestedLoad, double totalSelectedPmax)
        {
            List<ProductionPlanResponse> response = new List<ProductionPlanResponse>();
            var plantCostComparer = new List<PlantCostComparer>();

            //choose the last plant and try in combo with other plants to get cost effective solution
            var selectedPlant = selectedPlants.LastOrDefault();
            foreach (var sortedMeritOrder in sortedMeritOrders)
            {
                if (sortedMeritOrder.Pmax >= (requestedLoad - totalSelectedPmax))
                {
                    if ((requestedLoad - totalSelectedPmax) < sortedMeritOrder.Pmin)
                    {
                        var pmaxSelectedPlant = requestedLoad - sortedMeritOrder.Pmin;
                        var totalCost = (pmaxSelectedPlant * selectedPlant.CostPerMWh) + (sortedMeritOrder.Pmin * sortedMeritOrder.CostPerMWh);
                        plantCostComparer.Add(new PlantCostComparer
                        {
                            Plant1 = selectedPlant.PlantName,
                            Plant2 = sortedMeritOrder.PlantName,
                            Plant1Power = pmaxSelectedPlant,
                            Plant2Power = sortedMeritOrder.Pmin,
                            TotalCost = totalCost
                        });
                    }
                    else
                    {
                        plantCostComparer.Add(new PlantCostComparer
                        {
                            Plant1 = selectedPlant.PlantName,
                            Plant2 = sortedMeritOrder.PlantName,
                            Plant1Power = selectedPlant.Pmax,
                            Plant2Power = (requestedLoad - totalSelectedPmax),
                            TotalCost = 0
                        });
                        break;
                    }
                }
            }
            //get the cheapest possible option from the comparer and add it to selcted plant
            var cheapPlantCombo = plantCostComparer.OrderBy(x => x.TotalCost).FirstOrDefault();
            selectedPlants.AddRange(sortedMeritOrders.Where(x => x.PlantName == cheapPlantCombo.Plant2));

            //create the response based on the selected plants
            foreach (var plant in selectedPlants)
            {
                if (plant.PlantName == cheapPlantCombo.Plant1)
                {
                    response.Add(new ProductionPlanResponse
                    {
                        Name = plant.PlantName,
                        P = cheapPlantCombo.Plant1Power
                    });
                }
                else if (plant.PlantName == cheapPlantCombo.Plant2)
                {
                    response.Add(new ProductionPlanResponse
                    {
                        Name = plant.PlantName,
                        P = cheapPlantCombo.Plant2Power
                    });
                }
                else
                {
                    response.Add(new ProductionPlanResponse
                    {
                        Name = plant.PlantName,
                        P = plant.Pmax
                    });
                }
            }
            return response;
        }
    }
}

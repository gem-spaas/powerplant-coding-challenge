from typing import List
from fastapi import HTTPException

from src.models import FuelCosts, PowerPlant, ProductionPlanResponse, PowerPlantType

def get_cost(plant: PowerPlant, fuels: FuelCosts) -> float:
    """
    Calculate the cost of generating one MWh of electricity for a given power plant.

    Args:
        plant (PowerPlant): The power plant to evaluate.
        fuels (FuelCosts): Current fuel prices and wind percentage.

    Returns:
        float: Cost in euro/MWh, including CO2 if applicable. Wind is always 0.
    """
    if plant.type == PowerPlantType.WINDTURBINE.value:
        return 0.0
    elif plant.type == PowerPlantType.GASFIRED.value:
        return fuels.gas / plant.efficiency + 0.3 * fuels.co2
    elif plant.type == PowerPlantType.TURBOJET.value:
        return fuels.kerosine / plant.efficiency
    else:
        raise ValueError(f"Unknown powerplant type: {plant.type}")

def calculate_production_plan(load: float, fuels: FuelCosts, powerplants: List[PowerPlant]) -> List[ProductionPlanResponse]:
    """
    Calculate the optimal production plan to meet the given load with the available power plants.

    Args:
        load (float): Total required load in MW.
        fuels (FuelCosts): Fuel prices and wind percentage.
        powerplants (List[PowerPlant]): List of available power plants.

    Returns:
        List[ProductionPlanResponse]: List of power allocations per power plant.
    """
    plan = []
    remaining_load = load
    plant_outputs = {}

    # Step 1: Wind power first
    for plant in powerplants:
        if plant.type == PowerPlantType.WINDTURBINE.value:
            max_power = plant.pmax * fuels.wind / 100
            allocated = min(max_power, remaining_load)
            plant_outputs[plant.name] = allocated
            remaining_load -= allocated

    # Step 2: Sort other plants by cost
    other_plants = [p for p in powerplants if p.type != PowerPlantType.WINDTURBINE.value]
    other_plants.sort(key=lambda p: get_cost(p, fuels))

    for plant in other_plants:
        if remaining_load <= 0:
            plant_outputs[plant.name] = 0.0
            continue

        max_available = min(plant.pmax, remaining_load)
        if max_available < plant.pmin:
            plant_outputs[plant.name] = 0.0
            continue

        allocated = max(plant.pmin, max_available)
        plant_outputs[plant.name] = allocated
        remaining_load -= allocated

    # Step 3: Fill in any missing plants with 0.0
    for plant in powerplants:
        if plant.name not in plant_outputs:
            plant_outputs[plant.name] = 0.0
    
    # Step 4: Check if there's any remaining load that cannot be covered
    if remaining_load > 0:
        raise HTTPException(
            status_code=400,
            detail="Unable to meet the required load with given powerplant constraints."
        )

    return [ProductionPlanResponse(name, p) for name, p in plant_outputs.items()]

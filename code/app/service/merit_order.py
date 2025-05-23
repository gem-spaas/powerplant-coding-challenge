from fastapi import HTTPException
from fastapi.responses import JSONResponse
from app.schemas.request import *
from app.schemas.response import ProductionPlanResponse


def get_cost_for_plant(plant: PowerPlant, fuels: Fuels) -> float:
    """ Calculate the costs for a given plant acording its type, applying C02 """
    if plant.type == "gasfired":
        fuel_price = fuels.gas
        co2_price = fuels.co2 * 0.3
        return (fuel_price / plant.efficiency) + co2_price
    elif plant.type == "turbojet":
        fuel_price = fuels.kerosine
        return fuel_price / plant.efficiency
    elif plant.type == "windturbine":
        return 0
    else:
        raise ValueError(f"ERROR: Unknown powerplant type: {plant.type}")

def get_adjusted_pmax(plant: PowerPlant, fuels: Fuels) -> float:
    if plant.type == "windturbine":
        return plant.pmax * (fuels.wind / 100)
    return plant.pmax

def calculate_production_plan(input: ProductionPlanRequest):
    load_remaining = input.load
    result = []
    
    # Calculate costs for each plant
    enriched_plants = []
    for plant in input.powerplants:
        try:
            cost = get_cost_for_plant(plant, input.fuels)
        except ValueError as e:
            raise HTTPException(status_code=400, detail=str(e))
        available_pmax = get_adjusted_pmax(plant, input.fuels)
        enriched_plants.append((plant, cost, available_pmax))

    # Sort plants by cost ASC (objective: use low cost plants first)
    sorted_plants = sorted(enriched_plants, key=lambda p: p[1])

    for plant, cost, available in sorted_plants:
        max_power = available
        min_power = plant.pmin if load_remaining > 0 else 0
        prod = min(max_power, load_remaining)
        prod = max(prod, min_power) if load_remaining >= min_power else 0
        prod = round(min(prod, load_remaining), 1)
        result.append(ProductionPlanResponse(name=plant.name, p=prod))
        load_remaining -= prod

    # Adjustment if sum doesn't match load
    total = sum(p.p for p in result)
    if abs(total - input.load) > 1e-2:
        # raise HTTPException(status_code=400, detail=f"WARNING: Couldn't meet the load exactly. Load achieved: {total:.2f} MWh")
        return JSONResponse(
            status_code=206,
            content={
                "message": f"Couldn't meet the load exactly. Load achieved: {total:.2f} MWh",
                "production_attempt": [p.dict() for p in result]
            }
        )

    return result
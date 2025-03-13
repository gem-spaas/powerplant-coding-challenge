from fastapi import FastAPI
from pydantic import BaseModel, Field
from typing import List

class FuelCosts(BaseModel):
    gas: float = Field(13.4, alias="gas(euro/MWh)")
    kerosine: float = Field(50.8, alias="kerosine(euro/MWh)")
    co2: float = Field(20, alias="co2(euro/ton)")
    wind: float = Field(0, alias="wind(%)")

class PowerPlant(BaseModel):
    name: str
    type: str
    efficiency: float
    pmax: float
    pmin: float

class Payload(BaseModel):
    load: float
    fuels: FuelCosts
    powerplants: List[PowerPlant]

class ProductionPlanResponse(BaseModel):
    name: str
    p: float

def calculate_cost(plant: PowerPlant, fuels: FuelCosts) -> float:
    """
    Calculate the cost of generating power for a given power plant based on its type and efficiency.

    Args:
        plant (PowerPlant): The power plant for which to calculate the generation cost.
        fuels (FuelCosts): The current fuel costs, including gas and kerosine prices.

    Returns:
        float: The cost of generating one MWh of electricity for the specified power plant.
               Returns infinity for non-gas and non-turbojet plants as they are not considered.
    """
    if plant.type == "gasfired":
        return fuels.gas / plant.efficiency
    elif plant.type == "turbojet":
        return fuels.kerosine / plant.efficiency
    elif plant.type == "windturbine":
        return 0.0  # Wind power is free
    return float('inf')  # Other types are not considered here

def calculate_production_plan(load: float, fuels: FuelCosts, powerplants: List[PowerPlant]) -> List[ProductionPlanResponse]:
    """
    Calculate the optimal production plan to meet the given load with the available power plants.

    Args:
        load (float): The total load that needs to be met.
        fuels (FuelCosts): The current fuel costs, including gas and kerosine prices.
        powerplants (List[PowerPlant]): A list of available power plants.

    Returns:
        List[ProductionPlanResponse]: A list of allocations for each power plant to meet the load.
    """
    allocations = []

    # Step 1: Get Merit Order for All Plants
    merit_order = powerplants.copy()
    merit_order.sort(key=lambda x: calculate_cost(x, fuels))

    remaining_load = load

    # Step 2: Allocate Power Based on Merit Order
    for i, plant in enumerate(merit_order):
        if remaining_load <= 0:
            # If the load is already met, allocate 0 power for the remaining plants
            allocations.append(ProductionPlanResponse(name=plant.name, p=0.0))
            continue

        if plant.type == "windturbine":
            # Calculate the available wind power based on the wind percentage
            wind_power = plant.pmax * (fuels.wind / 100)
            # Allocate power from wind turbines as much as needed to meet the remaining load
            allocated_power = min(wind_power, remaining_load)
            allocations.append(ProductionPlanResponse(name=plant.name, p=round(allocated_power, 1)))

            # Reduce the remaining load by the allocated wind power
            remaining_load -= allocated_power

        elif remaining_load < plant.pmin:
            # If the remaining load is less than the minimum power the plant can produce,
            # we have to allocate at least the plant's minimum power
            allocations.append(ProductionPlanResponse(name=plant.name, p=plant.pmin))

            # Adjust the allocation of the previous plant to ensure we don't exceed the total load
            if i > 0:
                previous_allocation = allocations[-2]
                previous_allocation.p = max(0.0, load - plant.pmin)
            
            # Set remaining load to 0 as we have allocated the required load
            remaining_load = 0
        
        else:
            # Allocate the minimum power required for the current plant
            allocation = plant.pmin
            allocations.append(ProductionPlanResponse(name=plant.name, p=allocation))
            remaining_load -= allocation

            # Allocate additional power up to the plant's maximum capacity if needed
            if remaining_load > 0:
                max_increase = min(plant.pmax - allocation, remaining_load)
                allocations[-1].p += max_increase
                remaining_load -= max_increase

    # Ensure all plants are included in the response
    for plant in powerplants:
        if not any(p.name == plant.name for p in allocations):
            allocations.append(ProductionPlanResponse(name=plant.name, p=0.0))

    return allocations

app = FastAPI()

@app.post("/productionplan", response_model=list[ProductionPlanResponse])
def production_plan(payload: Payload):
    load = payload.load
    fuels = payload.fuels
    powerplants = payload.powerplants
    
    # Calculate production plan
    response = calculate_production_plan(load, fuels, powerplants)
    return response

if __name__ == '__main__':
    import uvicorn
    uvicorn.run(app, host="0.0.0.0", port=8888)
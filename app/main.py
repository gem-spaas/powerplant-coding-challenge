from fastapi import FastAPI

from lib.models import PowerPlantFactory
from lib.pydantic_models import Payload
from lib.utils import (
    calculate_total_co2_emmissions,
    calculate_co2_costs,
    calculate_total_fuel_costs,
)

app = FastAPI()

@app.post("/productionplan")
def calculate_production_plan(payload: Payload):

    power_plants = []
    production_plan = []

    factory = PowerPlantFactory()
    for powerplant in payload.powerplants:
        power_plants.append(factory.create_powerplant(powerplant, payload.fuels))

    # sort by efficiency
    power_plants.sort(key=lambda plant: plant.efficiency, reverse=True)

    # Initialize remaining load
    remaining_load = payload.load
    # Iterate oveer power plants
    for plant in power_plants:

        if plant.pmin > remaining_load:
            # reduce the load in the last plant
            production_plan[len(production_plan) - 1]["p"] -= plant.pmin - remaining_load
            
        allocated_power = plant.calculate_allocated_power(remaining_load)
        remaining_load -= allocated_power
       
        production_plan.append(
            {
                "name": plant.name,
                "p": allocated_power
            }
        )

        if remaining_load <= 0:
            break

    # Report
    total_co2_emmissions = calculate_total_co2_emmissions(production_plan)
    total_co2_costs = calculate_co2_costs(total_co2_emmissions, payload.fuels)
    total_fuel_costs = calculate_total_fuel_costs(production_plan, payload.fuels, power_plants)
    total_costs = round(total_co2_costs + total_fuel_costs)
    
    print("Total CO2 Emissions: ", total_co2_emmissions)
    print("Total CO2 Costs: ", total_co2_costs)
    print("Total Fuel Costs: ", total_fuel_costs)
    print("Grand Total: ", total_costs)

    return production_plan

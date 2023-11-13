from lib.models import CO2_EMISSION_FACTOR


def calculate_total_co2_emmissions(production_plan):
    total_co2_emmissions = 0
    for plant in production_plan:
        if plant["name"].startswith("gas"):
            total_co2_emmissions += CO2_EMISSION_FACTOR * plant["p"]
        elif plant["name"].startswith("turbo"):
            total_co2_emmissions += CO2_EMISSION_FACTOR * plant["p"]
        elif plant["name"].startswith("wind"):
            total_co2_emmissions += 0
        else:
            raise Exception("Unknown power plant type")
    return round(total_co2_emmissions)

def calculate_co2_costs(total_co2_emmissions, fuels):
    return round(total_co2_emmissions * fuels.co2_euro_ton)

def calculate_total_fuel_costs(production_plan, fuels, power_plants):
    total_fuel_costs = 0
    for plant in production_plan:
        plant_name = plant["name"]
        allocated_power = plant["p"]

        # Find the corresponding power plant object
        power_plant = next((p for p in power_plants if p.name == plant_name), None)
        if power_plant is None:
            raise Exception("Power plant not found")

        # Calculate fuel cost based on power plant type and efficiency
        if power_plant.type == 'gasfired':
            total_fuel_costs += (fuels.gas_euro_mwh / power_plant.efficiency) * allocated_power
        elif power_plant.type == 'turbojet':
            total_fuel_costs += (fuels.kerosine_euro_mwh / power_plant.efficiency) * allocated_power
        elif power_plant.type == 'windturbine':
            total_fuel_costs += 0  # Wind turbines have zero fuel cost
        else:
            raise Exception("Unknown power plant type")

    return round(total_fuel_costs)


import json

def calculate_production_plan(data):
    load = data['load']
    fuels = data['fuels']
    powerplants = data['powerplants']

    # Calculate the production cost for each power plant based on its type
    for plant in powerplants:
        if plant['type'] == 'windturbine':
            # Zero cost for wind turbines
            plant['cost'] = 0
            # Adjust max power based on wind percentage
            plant['pmax'] *= fuels['wind(%)'] / 100
        elif plant['type'] == 'gasfired':
            # Cost for gas-fired plants, including CO2 cost
            plant['cost'] = (fuels['gas(euro/MWh)'] / plant['efficiency']) + (0.3 * fuels['co2(euro/ton)'])
        elif plant['type'] == 'turbojet':
            # Cost for turbojets
            plant['cost'] = fuels['kerosine(euro/MWh)'] / plant['efficiency']

    # Sort power plants in ascending order of cost
    powerplants.sort(key=lambda x: x['cost'])

    # Initialize variables to store results and track remaining load
    result = []
    remaining_load = load

    # Allocate production respecting the merit order
    for plant in powerplants:
        if remaining_load <= 0:
            # If load is already satisfied, set production to zero
            production = 0
        else:
            # Calculate production for this plant within pmin and pmax limits
            if plant['type'] == 'windturbine':
                # Wind turbines have no pmin; their production is limited only by adjusted pmax
                production = min(remaining_load, plant['pmax'])
            else:
                # Respect pmin and pmax for other types of power plants
                production = max(plant['pmin'], min(remaining_load, plant['pmax']))
            
            # Reduce the remaining load by the production amount
            remaining_load -= production

        # Round production to the nearest 0.1 MW
        production = round(production, 1)
        result.append({"name": plant["name"], "p": production})

    # Adjust so the total production exactly matches the load
    total_production = sum([r['p'] for r in result])
    difference = round(load - total_production, 1)

    # Fine-tune adjustment to correct any rounding errors
    if difference != 0:
        # Find a power plant that can support the adjustment
        for plant in result:
            # Check limits to adjust production by a multiple of 0.1
            plant_data = next((p for p in powerplants if p["name"] == plant["name"]), None)
            if plant_data and plant_data['pmin'] <= plant['p'] + difference <= plant_data['pmax']:
                plant['p'] += difference
                break

    # Final check that the total production equals the load
    final_total_production = sum([r['p'] for r in result])
    if round(final_total_production, 1) != load:
        raise ValueError("Adjustment error: Unable to meet the required load with available power plants.")
    
    return result



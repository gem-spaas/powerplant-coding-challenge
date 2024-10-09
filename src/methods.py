def calculate_production_plan(payload):
    """
    This function will implement the algorithm for the calculation of the production
    plan based on the payload provided and sort the payload accordingly.

    Parameters:
        payload (dict): A dictionary containing the load, fuels, and powerplants.

    Returns:
        dict: A dictionary containing the sorted production plan.

    Raises:
        ValueError: If the payload is missing required keys or if the load is not a number.
    """
    # Extract values from payload
    load = payload['load']
    fuels = payload['fuels']
    powerplants = payload['powerplants']

    # Extract the relevant fuel costs
    gas_price = fuels['gas(euro/MWh)']
    kerosine_price = fuels['kerosine(euro/MWh)']
    co2_price = fuels['co2(euro/ton)']
    wind_percentage = fuels['wind(%)']

    # Sort powerplants based on cost-efficiency (cheapest first)
    powerplants = sorted(
        powerplants,
        key=lambda p: get_cost_efficiency(p, gas_price, kerosine_price, co2_price, wind_percentage)
    )

    # Initialize result list
    production_plan = []
    remaining_load = load

    for plant in powerplants:
        if remaining_load <= 0:
            # If the load is already met, the plant remains off
            production_plan.append({"name": plant['name'], "p": 0})
            continue

        p_min = plant['pmin']
        p_max = plant['pmax']

        if plant['type'] == 'windturbine':
            # Wind turbine generates power based on wind percentage
            p_max = p_max * (wind_percentage / 100)
            production = min(remaining_load, p_max)
            production_plan.append({"name": plant['name'], "p": production})
            remaining_load -= production
        else:
            # Gas-fired or turbojet plants must produce at least pmin when switched on
            production = min(remaining_load, p_max)
            if production >= p_min:
                production_plan.append({"name": plant['name'], "p": production})
                remaining_load -= production
            else:
                # If production cannot meet pmin, the plant remains off
                production_plan.append({"name": plant['name'], "p": 0})

    # If we still have a load that could not be met
    if remaining_load > 0:
        raise ValueError("It is not possible to meet the load with the available power plants.")

    return production_plan


def get_cost_efficiency(plant, gas_price, kerosine_price, co2_price, wind_percentage):
    """
    This function will provide the costs of a power plant based on its type and fuel costs.

    Parameters:
        plant (dict): A dictionary containing information about the power plant.
        gas_price (float): The price of gas (euro/MWh).
        kerosine_price (float): The price of kerosine (euro/MWh).
        co2_price (float): The price of CO2 (euro/ton).
        wind_percentage (float): The percentage of wind power produced by the plant.

    Returns:
        float: The cost-efficiency of the power plant.

    Raises:
        ValueError: If the power plant type is unknown.
    """
    if plant['type'] == 'gasfired':
        # Gas-fired plants have a fuel cost and a CO2 cost (0.3 tons of CO2 emitted per MWh)
        co2_emissions_per_mwh = 0.3
        return (gas_price + co2_emissions_per_mwh * co2_price) / plant['efficiency']
    elif plant['type'] == 'turbojet':
        # Turbojets use kerosine as fuel, no CO2 costs assumed here
        return kerosine_price / plant['efficiency']
    elif plant['type'] == 'windturbine':
        # Wind turbines have no fuel cost, so their cost-efficiency is 0
        return 0
    else:
        raise ValueError(f"Unknown power plant type: { plant['type'] }")

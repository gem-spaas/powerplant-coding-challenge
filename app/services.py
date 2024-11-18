from app.utils import allocate_powerplant_load


def calculate_power_distribution(load: int, fuels: dict, powerplants: list) -> list:
    """Calculate power distribution based on load, fuels and powerplants.

    Args:
        load (int): The amount of energy (MWh) that need to be generated during one hour.
        fuels (dict): A dictionary containing the cost of different fuels.
        powerplants (list): A list of powerplants with their respective properties.

    Raises:
        ValueError: If a property value is not accepted.

    Returns:
        list: A list of powerplants with their respective power output.
    """
    # Parse fuels
    gas_cost = fuels["gas(euro/MWh)"] / 0.5  # efficiency 50%
    kerosine_cost = fuels["kerosine(euro/MWh)"] / 0.3  # efficiency 30%
    wind_percentage = fuels["wind(%)"] / 100

    # Calculate cost per MWh per powerplant
    powerplant_costs = []
    for plant in powerplants:
        if plant["type"] == "gasfired":
            cost = gas_cost / plant["efficiency"]
        elif plant["type"] == "turbojet":
            cost = kerosine_cost / plant["efficiency"]
        elif plant["type"] == "windturbine":
            cost = 0  # wind is free
        else:
            raise ValueError("Unknown powerplant type")

        # Calculate effective max power for wind turbines
        if plant["type"] == "windturbine":
            max_power = plant["pmax"] * wind_percentage
        else:
            max_power = plant["pmax"]

        powerplant_costs.append(
            {
                "name": plant["name"],
                "type": plant["type"],
                "cost": cost,
                "pmin": plant["pmin"],
                "pmax": max_power,
            }
        )

    # Sort powerplants by cost (merit order)
    sorted_plants = sorted(powerplant_costs, key=lambda x: x["cost"])

    # Allocate load based on merit order
    power_distribution = allocate_powerplant_load(load, sorted_plants)

    # Ensure the total load matches exactly
    if abs(sum(item["p"] for item in power_distribution) - load) > 0.1:
        raise ValueError("Unable to satisfy load exactly with given powerplants.")

    return power_distribution

def allocate_powerplant_load(load: float, sorted_plants: list) -> list:
    """Allocate load to powerplants based on merit order.

    Args:
        load (float): The total load to be allocated.
        sorted_plants (list): A list of powerplants sorted by cost.

    Raises:
        ValueError: If Pmin exceeds Pmax for a powerplant.

    Returns:
        list: A list of powerplants with their respective power output.
    """
    # Allocate load based on merit order
    remaining_load = load
    power_distribution = []
    for plant in sorted_plants:
        if remaining_load <= 0:
            # If load is already satisfied, set output to 0 for remaining plants
            power_distribution.append({"name": plant["name"], "p": 0.0})
            continue

        # Determine how much power the plant can contribute
        if plant["pmax"] < plant["pmin"]:
            raise ValueError(f"Pmin cannot exceed Pmax for plant {plant['name']}.")

        power = 0
        if remaining_load >= plant["pmin"]:
            power = min(plant["pmax"], remaining_load)
            remaining_load -= power

        # If plant is not needed, output 0.0
        if power < plant["pmin"]:
            power = 0.0

        power_distribution.append({"name": plant["name"], "p": round(power, 1)})
    return power_distribution

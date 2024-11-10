from core.classes import Fuels, Powerplant
from typing import List
from core.utils import logger


def solve(powerplants_list: List[Powerplant], load: int, fuels: Fuels, with_co2: bool = False):
    """
    Solve the power production plan considering optional CO2 costs.
    """
    logger.info(f"Starting production plan calculation (CO2: {'enabled' if with_co2 else 'disabled'})")
    logger.info(f"Target load: {load} MW")

    # Calculate costs for each plant
    for plant in powerplants_list:
        co2_em = 0.3 if with_co2 else 0
        plant.price = plant.get_cost_per_mwh(fuels, co2_em)
        logger.debug(f"Plant {plant.name} cost: {plant.price}/MWh")

    # Sort by price
    sorted_plants = sorted(powerplants_list, key = lambda x: x.price)
    logger.debug("Merit order: " + ", ".join(p.name for p in sorted_plants))

    total_cost = 0
    remaining_load = load

    # Assign power to each plant
    for plant in sorted_plants:
        min_power, max_power = plant.get_available_power(fuels)
        logger.debug(f"Processing {plant.name} (min: {min_power}, max: {max_power})")

        if remaining_load > 0:
            usable_power = min(max_power, remaining_load)
            if usable_power >= min_power:
                plant.injection = round(usable_power, 2)
                remaining_load -= usable_power
                total_cost += usable_power * plant.price
                logger.debug(f"Assigned {plant.injection}MW to {plant.name}")
            else:
                plant.injection = 0
                logger.debug(f"Skipped {plant.name} - minimum power requirement not met")
        else:
            plant.injection = 0
            logger.debug(f"Skipped {plant.name} - load requirement met")

    total_power = sum(p.injection for p in powerplants_list)
    logger.info(f"Total power assigned: {total_power}MW")
    logger.info(f"Total cost: €{total_cost:.2f}")
    load_met = abs(total_power - load) < 0.1,
    if not load_met:
        logger.warning(f"Load not matched. {abs(total_power - load):.2f}MW needed.")

    return sorted_plants,total_cost

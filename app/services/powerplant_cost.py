# Local application import
from app.models.powerplant import PowerPlant, PlantTypes


def calculate_powerplant_cost(
    plant: PowerPlant,
    gas_price: float,
    kerosine_price: float,
    co2_price: float,
    wind_availability_percent: float
) -> None:
    """
    Calculate the cost per MWh and available capacity for a given power plant
    based on its type and current fuel prices. Modifies the `plant` object in place.

    Parameters:
        plant (PowerPlant):      The power plant for which the cost is calculated.
        gas_price (float):       Current price of gas in €/MWh.
        kerosine_price (float):  Current price of kerosine in €/MWh.
        co2_price (float):       Current CO2 emission price in €/ton.
        wind_availability_percent (float): Wind availability percentage (0-100).

    Raises:
        ValueError:              If the plant type is not recognized or if
                                 wind_availability_percent is out of range.
    """
    if plant.type == PlantTypes.WINDTURBINE:
        # Wind turbines have no fuel cost; power depends on wind availability
        plant.cost_per_mwh = 0.0
        plant.available_capacity = (plant.pmax * wind_availability_percent / 100)

    elif plant.type == PlantTypes.GASFIRED:
        # Calculate fuel and CO2 costs for gas-fired plants
        fuel_cost = gas_price / plant.efficiency
        co2_cost = (0.3 * co2_price) / plant.efficiency
        plant.cost_per_mwh = fuel_cost + co2_cost
        plant.available_capacity = plant.pmax

    elif plant.type == PlantTypes.TURBOJET:
        # Only fuel cost for turbojets; no CO2 considered
        fuel_cost = kerosine_price / plant.efficiency
        plant.cost_per_mwh = fuel_cost
        plant.available_capacity = plant.pmax

    else:
        # Raise error for unknown power plant types
        raise ValueError(f"Unknown plant type: {plant.type}")

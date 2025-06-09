# Third-party imports
import pytest
from app.models.powerplant import PowerPlant, PlantTypes
from app.services.powerplant_cost import calculate_powerplant_cost

def test_windturbine_cost_and_capacity():
    """
    Test that wind turbine cost is zero and available capacity
    is correctly calculated based on wind availability percentage.
    """
    plant = PowerPlant("Wind1", PlantTypes.WINDTURBINE, efficiency=1.0, pmin=0, pmax=100)
    calculate_powerplant_cost(plant, gas_price=10, kerosine_price=20, co2_price=30, wind_availability_percent=50)
    assert plant.cost_per_mwh == 0.0
    assert plant.available_capacity == 50  # 50% of 100


def test_gasfired_cost_and_capacity():
    """
    Test gas-fired plant cost calculation including fuel and CO2 costs,
    and ensure available capacity equals pmax.
    """
    plant = PowerPlant("Gas1", PlantTypes.GASFIRED, efficiency=0.5, pmin=0, pmax=100)
    gas_price = 10
    co2_price = 30
    calculate_powerplant_cost(plant, gas_price, kerosine_price=0, co2_price=co2_price, wind_availability_percent=0)
    expected_cost = (gas_price / plant.efficiency) + (0.3 * co2_price / plant.efficiency)
    assert plant.cost_per_mwh == expected_cost
    assert plant.available_capacity == 100


def test_turbojet_cost_and_capacity():
    """
    Test turbojet plant cost calculation based on kerosine price,
    and verify available capacity equals pmax.
    """
    plant = PowerPlant("Turbo1", PlantTypes.TURBOJET, efficiency=0.8, pmin=0, pmax=100)
    kerosine_price = 20
    calculate_powerplant_cost(plant, gas_price=0, kerosine_price=kerosine_price, co2_price=0, wind_availability_percent=0)
    expected_cost = kerosine_price / plant.efficiency
    assert plant.cost_per_mwh == expected_cost
    assert plant.available_capacity == 100


def test_unknown_plant_type_raises():
    """
    Test that an unknown plant type raises a ValueError.
    """
    plant = PowerPlant("Unknown1", "solar", efficiency=1.0, pmin=0, pmax=100)
    with pytest.raises(ValueError, match="Unknown plant type: solar"):
        calculate_powerplant_cost(plant, gas_price=0, kerosine_price=0, co2_price=0, wind_availability_percent=0)

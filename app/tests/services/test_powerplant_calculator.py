# Third-party imports
import pytest
from app.models.powerplant import ProductionPlanRequest, Fuels, PowerPlantData, PlantTypes
from app.services.powerplant_calculator import ProductionPlanCalculator

@pytest.fixture
def sample_payload():
    """
    Provides a sample valid ProductionPlanRequest payload
    with fuels and powerplants data for testing.
    
    Returns:
        ProductionPlanRequest: Sample payload object.
    """
    fuels_data = {
        "gas(euro/MWh)": 13.4,
        "kerosine(euro/MWh)": 50.8,
        "co2(euro/ton)": 20,
        "wind(%)": 60
    }

    fuels = Fuels.model_validate(fuels_data)
    
    powerplants = [
        PowerPlantData(name="Wind1", type=PlantTypes.WINDTURBINE, efficiency=1.0, pmin=0, pmax=150),
        PowerPlantData(name="Gas1", type=PlantTypes.GASFIRED, efficiency=0.53, pmin=100, pmax=460),
        PowerPlantData(name="Turbo1", type=PlantTypes.TURBOJET, efficiency=0.3, pmin=0, pmax=16),
    ]
    return ProductionPlanRequest(load=480, fuels=fuels, powerplants=powerplants)


def test_generate_optimized_plan_total_load(sample_payload):
    """
    Test that the generated optimized plan:
    - Is a list of dicts with 'name' and 'p' keys.
    - The sum of the production matches approximately the requested load.
    """
    calculator = ProductionPlanCalculator()
    plan = calculator.generate_optimized_plan(sample_payload)

    assert isinstance(plan, list)
    assert all("name" in p and "p" in p for p in plan)

    total_production = sum(p["p"] for p in plan)
    assert abs(total_production - sample_payload.load) < 1e-1


def test_production_allocation_prioritizes_wind(sample_payload):
    """
    Test that wind turbine production does not exceed its maximum 
    available power based on wind availability percentage.
    """
    calculator = ProductionPlanCalculator()
    plan = calculator.generate_optimized_plan(sample_payload)

    wind_plant = next(p for p in plan if p["name"] == "Wind1")
    assert wind_plant["p"] <= 150 * 0.6


def test_error_on_invalid_payload():
    """
    Test that passing an invalid payload (None) raises an Exception.
    """
    calculator = ProductionPlanCalculator()
    with pytest.raises(Exception):
        calculator.generate_optimized_plan(None)


def test_merit_order_sorting(sample_payload):
    """
    Test that powerplants are sorted in ascending order of cost_per_mwh
    after generating the plan.
    """
    calculator = ProductionPlanCalculator()
    _ = calculator.generate_optimized_plan(sample_payload)
    plants = calculator.plants

    costs = [p.cost_per_mwh for p in plants]
    assert costs == sorted(costs)


def test_fine_tune_adjustment():
    """
    Test the internal fine-tuning method adjusts production to match
    target load within acceptable tolerance without violating pmin/pmax.
    """
    calculator = ProductionPlanCalculator()
    from app.models.powerplant import PowerPlant, PlantTypes

    p1 = PowerPlant("P1", PlantTypes.GASFIRED, 0.5, 100, 200)
    p1.cost_per_mwh = 10
    p1.available_capacity = 200
    p2 = PowerPlant("P2", PlantTypes.GASFIRED, 0.7, 50, 150)
    p2.cost_per_mwh = 20
    p2.available_capacity = 150
    calculator.plants = [p1, p2]

    production_plan = [{"name": "P1", "p": 190}, {"name": "P2", "p": 50}]
    target_load = 240
    calculator._fine_tune_production_allocation(production_plan, target_load)

    total = sum(p["p"] for p in production_plan)
    assert abs(total - target_load) < 0.1

    for i, p in enumerate(calculator.plants):
        assert p.pmin <= production_plan[i]["p"] <= p.pmax

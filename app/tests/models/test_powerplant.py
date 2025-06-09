# Third-party imports
import pytest
from pydantic import ValidationError
from app.models.powerplant import (
    PowerPlantData, PlantTypes, Fuels, ProductionPlanRequest, ProductionPlanResponse
)


def test_powerplantdata_valid():
    """
    Test creation of a valid PowerPlantData instance.
    Verifies that the fields are set correctly.
    """
    data = PowerPlantData(
        name="plant1",
        type=PlantTypes.GASFIRED,
        efficiency=0.5,
        pmin=10.0,
        pmax=100.0
    )
    assert data.name == "plant1"
    assert data.efficiency == 0.5


def test_powerplantdata_invalid_efficiency():
    """
    Test that a PowerPlantData instance with negative efficiency
    raises a ValidationError.
    """
    with pytest.raises(ValidationError):
        PowerPlantData(
            name="plant1",
            type=PlantTypes.TURBOJET,
            efficiency=-0.1,
            pmin=0,
            pmax=10
        )


def test_powerplantdata_invalid_pmin_greater_than_pmax():
    """
    Test that a PowerPlantData instance with pmin greater than pmax
    raises a ValidationError with the appropriate message.
    """
    with pytest.raises(ValidationError) as exc_info:
        PowerPlantData(
            name="plant2",
            type=PlantTypes.WINDTURBINE,
            efficiency=0.9,
            pmin=50,
            pmax=10
        )
    assert "pmin must be less than or equal to pmax" in str(exc_info.value)


def test_fuels_parsing_from_alias():
    """
    Test that Fuels model correctly parses input data using field aliases.
    """
    input_data = {
        "gas(euro/MWh)": 10.5,
        "kerosine(euro/MWh)": 5.2,
        "co2(euro/ton)": 20.0,
        "wind(%)": 60.0
    }
    fuels = Fuels(**input_data)
    assert fuels.wind == 60.0
    assert fuels.co2 == 20.0


def test_production_plan_request_valid():
    """
    Test creation of a valid ProductionPlanRequest from input dictionary.
    Verifies correct parsing of nested Fuels and PowerPlantData.
    """
    input_data = {
        "load": 100.0,
        "fuels": {
            "gas(euro/MWh)": 13.0,
            "kerosine(euro/MWh)": 50.0,
            "co2(euro/ton)": 20.0,
            "wind(%)": 40.0
        },
        "powerplants": [
            {
                "name": "gasplant1",
                "type": "gasfired",
                "efficiency": 0.8,
                "pmin": 10,
                "pmax": 100
            },
            {
                "name": "windplant1",
                "type": "windturbine",
                "efficiency": 1.0,
                "pmin": 0,
                "pmax": 50
            }
        ]
    }

    plan = ProductionPlanRequest(**input_data)
    assert plan.load == 100.0
    assert len(plan.powerplants) == 2
    assert plan.fuels.gas == 13.0


def test_production_plan_response_rounding():
    """
    Test that ProductionPlanResponse rounds the power output to one decimal place.
    Also checks that model_dump returns the rounded value.
    """
    response = ProductionPlanResponse(name="plant1", p=12.345)
    assert response.p == 12.3  # rounded to 1 decimal place
    dumped = response.model_dump()
    assert dumped["p"] == 12.3  # correctly formatted in the output

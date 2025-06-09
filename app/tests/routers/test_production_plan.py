# Third-party imports
import pytest
from fastapi.testclient import TestClient
from app.main import app

client = TestClient(app)


@pytest.fixture
def valid_payload():
    """
    Provides a valid payload for testing the /production_plan endpoint.
    
    Returns:
        dict: A sample valid payload with load, fuels, and powerplants data.
    """
    return {
        "load": 100,
        "fuels": {
            "gas(euro/MWh)": 13.0,
            "kerosine(euro/MWh)": 50.0,
            "co2(euro/ton)": 20.0,
            "wind(%)": 60.0
        },
        "powerplants": [
            {
                "name": "gasfire1",
                "type": "gasfired",
                "efficiency": 0.8,
                "pmin": 10,
                "pmax": 100
            },
            {
                "name": "wind1",
                "type": "windturbine",
                "efficiency": 1.0,
                "pmin": 0,
                "pmax": 50
            }
        ]
    }


def test_production_plan_success(valid_payload):
    """
    Tests that the /production_plan endpoint returns a successful response
    when provided with a valid payload.
    
    Asserts:
        - Response status code is 200 OK.
        - Response JSON is a list of plants with 'name' and 'p' keys.
        - The sum of power outputs 'p' approximately equals the requested load.
    """
    response = client.post("/production_plan", json=valid_payload)

    assert response.status_code == 200
    data = response.json()

    assert isinstance(data, list)
    assert all("name" in plant and "p" in plant for plant in data)
    assert pytest.approx(sum(p["p"] for p in data), 0.1) == 100


def test_production_plan_invalid_payload():
    """
    Tests that the /production_plan endpoint returns a 422 Unprocessable Entity
    when provided with an invalid payload (e.g., pmin > pmax in powerplant).
    """
    invalid_payload = {
        "load": 50,
        "fuels": {
            "gas(euro/MWh)": 10,
            "kerosine(euro/MWh)": 20,
            "co2(euro/ton)": 30,
            "wind(%)": 40
        },
        "powerplants": [
            {
                "name": "brokenplant",
                "type": "gasfired",
                "efficiency": 0.9,
                "pmin": 60,  # pmin is greater than pmax, invalid input
                "pmax": 40
            }
        ]
    }

    response = client.post("/production_plan", json=invalid_payload)
    assert response.status_code == 422  # Unprocessable Entity


def test_production_plan_with_specific_case():
    """
    Test the /production_plan endpoint with a detailed input scenario.

    This test verifies that the endpoint correctly computes the production distribution
    among various types of power plants (gas-fired, turbojet, windturbines) based on
    fuel costs, plant efficiencies, and wind availability, ensuring:
    """
    payload = {
        "load": 910,
        "fuels": {
            "gas(euro/MWh)": 13.4,
            "kerosine(euro/MWh)": 50.8,
            "co2(euro/ton)": 20,
            "wind(%)": 60
        },
        "powerplants": [
            {
                "name": "gasfiredbig1",
                "type": "gasfired",
                "efficiency": 0.53,
                "pmin": 100,
                "pmax": 460
            },
            {
                "name": "gasfiredbig2",
                "type": "gasfired",
                "efficiency": 0.53,
                "pmin": 100,
                "pmax": 460
            },
            {
                "name": "gasfiredsomewhatsmaller",
                "type": "gasfired",
                "efficiency": 0.37,
                "pmin": 40,
                "pmax": 210
            },
            {
                "name": "tj1",
                "type": "turbojet",
                "efficiency": 0.3,
                "pmin": 0,
                "pmax": 16
            },
            {
                "name": "windpark1",
                "type": "windturbine",
                "efficiency": 1,
                "pmin": 0,
                "pmax": 150
            },
            {
                "name": "windpark2",
                "type": "windturbine",
                "efficiency": 1,
                "pmin": 0,
                "pmax": 36
            }
        ]
    }

    expected_response = [
        {"name": "windpark1", "p": 90.0},
        {"name": "windpark2", "p": 21.6},
        {"name": "gasfiredbig1", "p": 460.0},
        {"name": "gasfiredbig2", "p": 338.4},
        {"name": "gasfiredsomewhatsmaller", "p": 0.0},
        {"name": "tj1", "p": 0.0}
    ]

    response = client.post("/production_plan", json=payload)

    assert response.status_code == 200

    result = response.json()

    # Validate total load matches expected
    total_production = sum(plant["p"] for plant in result)
    assert abs(total_production - payload["load"]) < 0.01, \
        f"Total production {total_production} does not match required load {payload['load']}"

    # Validate each plant's output against expected values
    for expected in expected_response:
        match = next((item for item in result if item["name"] == expected["name"]), None)
        assert match is not None, f"Missing plant: {expected['name']}"
        assert abs(match["p"] - expected["p"]) < 0.01, \
            f"Incorrect production for {expected['name']}: got {match['p']}, expected {expected['p']}"

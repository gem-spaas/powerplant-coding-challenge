import pytest
from fastapi.testclient import TestClient
import json

from api import app

client = TestClient(app)

@pytest.mark.parametrize("payload_file,expected_response_file", [
    ("src/tests/example_payloads/payload1.json", "src/tests/example_payloads/response1.json"),
    ("src/tests/example_payloads/payload2.json", "src/tests/example_payloads/response2.json"),
    ("src/tests/example_payloads/payload3.json", "src/tests/example_payloads/response3.json"),
])
def test_production_plan(payload_file, expected_response_file):
    # Load the JSON payload from file
    with open(payload_file, 'r') as f:
        payload = json.load(f)

    # Load the expected response from file
    with open(expected_response_file, 'r') as f:
        expected_response = json.load(f)

    # Make a POST request to the /productionplan endpoint
    response = client.post("/productionplan", json=payload)
    
    # Assert the status code and the response data
    assert response.status_code == 200
    assert response.json() == expected_response


@pytest.mark.parametrize("payload_file", [
    "src/tests/example_payloads/payload1.json",
    "src/tests/example_payloads/payload2.json",
    "src/tests/example_payloads/payload3.json",
])
def test_production_plan_data_types(payload_file):
    # Load the JSON payload from file
    with open(payload_file, 'r') as f:
        payload = json.load(f)

    # Make a POST request to the /productionplan endpoint
    response = client.post("/productionplan", json=payload)

    # Assert the status code
    assert response.status_code == 200
    
    # Assert the response is a list
    response_data = response.json()
    assert isinstance(response_data, list)

    # Check if each element in the response has the correct structure
    for plant in response_data:
        assert "name" in plant
        assert "p" in plant
        assert isinstance(plant["name"], str)
        assert isinstance(plant["p"], (float, int))
        assert round(plant["p"], 2) == plant["p"], f"Value for {plant['name']} is not rounded to 2 decimals: {plant['p']}"
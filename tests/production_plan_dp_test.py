
import os
import pytest
import logging
import json
from src.main_dynamic_programming import app

logging.basicConfig(level=logging.INFO)
logger = logging.getLogger(__name__)


@pytest.fixture
def client():
    app.config['TESTING'] = True
    with app.test_client() as client:
        yield client


def test_production_plan_returns_correct_format(client):
    payload_path = os.path.join(os.path.dirname(__file__), '..', 'example_payloads', 'payload3.json')

    with open(payload_path, 'r') as file:
        payload = json.load(file)

    response = client.post('/productionplan', json=payload)
    logger.info(f"Response: {response.get_json()}")

    assert response.status_code == 200
    data = response.get_json()
    assert isinstance(data, list)
    assert all('name' in plant and 'p' in plant for plant in data)


def test_production_plan_golden(client):
    payload_path = os.path.join(os.path.dirname(__file__), '..', 'example_payloads', 'payload3.json')
    expected_response_path = os.path.join(os.path.dirname(__file__), '..', 'example_payloads', 'response3.json')

    with open(payload_path, 'r') as file:
        payload = json.load(file)

    with open(expected_response_path, 'r') as file:
        expected_response = json.load(file)

    response = client.post('/productionplan', json=payload)
    assert response.status_code == 200

    actual_response = response.get_json()

    expected_dict = {plant['name']: plant['p'] for plant in expected_response}
    actual_dict = {plant['name']: plant['p'] for plant in actual_response}

    assert set(expected_dict.keys()) == set(actual_dict.keys())

    for name in expected_dict:
        assert abs(actual_dict[name] - expected_dict[name]) < 0.10001


@pytest.mark.parametrize("payload_file", ["payload1.json", "payload2.json", "payload3.json"])
def test_sum_of_the_power_produced_by_all_powerplants_should_equal_the_load(client, payload_file):
    payload_path = os.path.join(os.path.dirname(__file__), '..', 'example_payloads', payload_file)

    with open(payload_path, 'r') as file:
        payload = json.load(file)

    response = client.post('/productionplan', json=payload)
    logger.info(f"Response: {response.get_json()}")
    data = response.get_json()
    assert payload['load'] == sum([plant['p'] for plant in data])


if __name__ == '__main__':
    pass

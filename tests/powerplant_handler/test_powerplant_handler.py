import json

import pytest

from powerplant_handler.powerplant_handler import PowerplantHandler


@pytest.fixture
def complex_payload():
    with open('test_data/payload3.json', 'r') as file:
        return json.load(file)


@pytest.fixture
def sample_config():
    from configparser import ConfigParser
    config = ConfigParser()
    config.add_section('SETTINGS')
    config.set('SETTINGS', 'INCLUDE_CO2_EMISSIONS', 'True')
    config.set('SETTINGS', 'GASFIRED_CO2_TONS', '0.3')
    config.set('SETTINGS', 'TURBOJET_CO2_TONS', '0.3')
    return config


def test_complex_production_plan(complex_payload, sample_config):

    expected_output_file = 'test_data/response3.json'
    with open(expected_output_file, 'r') as file:
        expected_output = json.load(file)

    result = PowerplantHandler.calculate_production_plan(complex_payload, sample_config)
    for expected_plant in expected_output:
        # Find the corresponding result plant by name
        result_plant = None
        for plant in result:
            if plant['name'] == expected_plant['name']:
                result_plant = plant
                break

        assert result_plant is not None, f"Missing plant in result: {expected_plant['name']}"
        assert result_plant['p'] == expected_plant['p'], f"Mismatch for plant {expected_plant['name']}: expected {expected_plant['p']}, got {result_plant['p']}"

    total_power = sum(plant['p'] for plant in result)
    assert total_power == complex_payload['load'], "Total production does not match load"

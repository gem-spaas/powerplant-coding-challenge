import unittest
import json
from unittest.mock import patch, mock_open

from src.utils import load_config
from src.production_plan import ProductionPlanGenerator
from src.api_server import app


class LoadConfigTestCase(unittest.TestCase):
    """
    Test cases for the project utils library.
    """

    @patch('builtins.open', mock_open(read_data='key: value'))
    def test_01_load_config_with_config_file(self):
        """Tests loading the config file when it exists."""
        config = load_config()
        self.assertDictEqual(config, {'key': 'value'})

    def test_02_load_config_without_config_file(self):
        """Tests loading the config file when it does not exist."""
        mo = mock_open()
        with patch('builtins.open', mo) as mocked_open:
            mocked_open.side_effect = FileNotFoundError
            config = load_config()
        self.assertDictEqual(config, {})


class ProductionPlanTestCase(unittest.TestCase):
    """
    Test case for the production plan generator.
    """

    def test_01_correct_production_plan(self):
        """Tests a correct production plan generation."""

        load = 910
        fuels = {
            "gas(euro/MWh)": 13.4,
            "kerosine(euro/MWh)": 50.8,
            "co2(euro/ton)": 20,
            "wind(%)": 60
        }
        powerplants = [
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

        expected_plan = [
            {
                "name": "windpark1",
                "p": 90.0
            },
            {
                "name": "windpark2",
                "p": 21.6
            },
            {
                "name": "gasfiredbig1",
                "p": 460.0
            },
            {
                "name": "gasfiredbig2",
                "p": 338.4
            },
            {
                "name": "gasfiredsomewhatsmaller",
                "p": 0.0
            },
            {
                "name": "tj1",
                "p": 0.0
            }
        ]
        generated_plan = ProductionPlanGenerator(load, fuels, powerplants).get_production_plan()
        self.assertListEqual(generated_plan, expected_plan)

    def test_02_correct_production_plan_with_excessive_pmin(self):
        """Tests a correct production plan generation when a pmin is more than the remaining load."""

        load = 130
        fuels = {
            "gas(euro/MWh)": 13.4,
            "kerosine(euro/MWh)": 50.8,
            "co2(euro/ton)": 20,
            "wind(%)": 100
        }
        powerplants = [
            {
                "name": "gasfiredbig1",
                "type": "gasfired",
                "efficiency": 1,
                "pmin": 50,
                "pmax": 460
            },
            {
                "name": "windpark1",
                "type": "windturbine",
                "efficiency": 1,
                "pmin": 0,
                "pmax": 80
            },
            {
                "name": "windpark2",
                "type": "windturbine",
                "efficiency": 1,
                "pmin": 0,
                "pmax": 40
            }
        ]

        expected_plan = [
            {
                "name": "windpark1",
                "p": 80.0
            },
            {
                "name": "windpark2",
                "p": 0.0
            },
            {
                "name": "gasfiredbig1",
                "p": 50.0
            }
        ]
        generated_plan = ProductionPlanGenerator(load, fuels, powerplants).get_production_plan()
        self.assertListEqual(generated_plan, expected_plan)

    def test_03_correct_production_plan_with_excessive_pmin(self):
        """Tests a correct production plan generation when a pmin is more than the remaining load."""

        load = 130
        fuels = {
            "gas(euro/MWh)": 13.4,
            "kerosine(euro/MWh)": 50.8,
            "co2(euro/ton)": 20,
            "wind(%)": 100
        }
        powerplants = [
            {
                "name": "gasfiredbig1",
                "type": "gasfired",
                "efficiency": 1,
                "pmin": 50,
                "pmax": 460
            },
            {
                "name": "windpark1",
                "type": "windturbine",
                "efficiency": 1,
                "pmin": 30,
                "pmax": 80
            },
            {
                "name": "windpark2",
                "type": "windturbine",
                "efficiency": 1,
                "pmin": 20,
                "pmax": 40
            }
        ]

        expected_plan = [
            {
                "name": "windpark1",
                "p": 80.0
            },
            {
                "name": "windpark2",
                "p": 20.0
            },
            {
                "name": "gasfiredbig1",
                "p": 50.0
            }
        ]
        generated_plan = ProductionPlanGenerator(load, fuels, powerplants).get_production_plan()
        self.assertListEqual(generated_plan, expected_plan)


class ProductionPlanAPITestCase(unittest.TestCase):
    """
    Test case for the production plan API.
    """

    def test_01_successful_production_plan_request(self):
        """Tests a successful production plan request."""
        app_test_client = app.test_client()

        payload = {
            "load": 100,
            "fuels":
                {
                    "gas(euro/MWh)": 13.4
                },
            "powerplants": [
                {
                    "name": "gasfiredbig1",
                    "type": "gasfired",
                    "efficiency": 0.5,
                    "pmin": 10,
                    "pmax": 100
                }
            ]
        }
        expected_response = [
            {
                "name": "gasfiredbig1",
                "p": 100.0
            }
        ]

        response = app_test_client.post('/productionplan', data=json.dumps(payload), content_type='application/json')
        response_data = json.loads(response.data.decode('utf-8'))

        self.assertEqual(response.status_code, 200)
        self.assertListEqual(response_data, expected_response)

    def test_02_invalid_production_plan_request_missing_powerplant(self):
        """Tests an invalid production plan request. Missing list of power plants."""
        app_test_client = app.test_client()

        payload = {
            "load": 910,
            "fuels":
                {
                    "gas(euro/MWh)": 13.4,
                    "kerosine(euro/MWh)": 50.8,
                    "co2(euro/ton)": 20,
                    "wind(%)": 60
                }
        }
        expected_response = {'message': 'Invalid or missing request parameters'}

        response = app_test_client.post('/productionplan', data=json.dumps(payload), content_type='application/json')
        response_data = json.loads(response.data.decode('utf-8'))

        self.assertEqual(response.status_code, 400)
        self.assertDictEqual(response_data, expected_response)

    def test_03_invalid_production_plan_request_missing_fuels(self):
        """Tests an invalid production plan request. Missing fuel details."""
        app_test_client = app.test_client()

        payload = {
            "load": 910,
            "powerplants": [
                {
                    "name": "gasfiredbig1",
                    "type": "gasfired",
                    "efficiency": 0.53,
                    "pmin": 100,
                    "pmax": 460
                }
            ]
        }
        expected_response = {'message': 'Invalid or missing request parameters'}

        response = app_test_client.post('/productionplan', data=json.dumps(payload), content_type='application/json')
        response_data = json.loads(response.data.decode('utf-8'))

        self.assertEqual(response.status_code, 400)
        self.assertDictEqual(response_data, expected_response)

    def test_04_invalid_production_plan_request_missing_load(self):
        """Tests an invalid production plan request. Missing load."""
        app_test_client = app.test_client()

        payload = {
            "load": 910
        }
        expected_response = {'message': 'Invalid or missing request parameters'}

        response = app_test_client.post('/productionplan', data=json.dumps(payload), content_type='application/json')
        response_data = json.loads(response.data.decode('utf-8'))

        self.assertEqual(response.status_code, 400)
        self.assertDictEqual(response_data, expected_response)

    def test_05_production_plan_request_error(self):
        """Tests when the production plan request throws an error."""
        app_test_client = app.test_client()

        payload = {
            "load": 910,
            "fuels":
                {
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
                }
            ]
        }
        expected_response = {'message': 'Unexpected error during the Production Plan generation'}

        with patch('production_plan.ProductionPlanGenerator.get_production_plan', side_effect=KeyError):
            response = app_test_client.post('/productionplan', data=json.dumps(payload), content_type='application/json')
        response_data = json.loads(response.data.decode('utf-8'))

        self.assertEqual(response.status_code, 500)
        self.assertDictEqual(response_data, expected_response)

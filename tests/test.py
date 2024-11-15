import unittest
from flask import json
from src.api.main import app, Plant, ProductionPlanRequest

#test running capabilities
class TestProductionPlan(unittest.TestCase):
    #Setup for testing mode on flask app
    def setUp(self):
        self.app = app.test_client()
        self.app.testing = True

    #main test method tp check the functionality of the /productionplan endpoint.
    def test_calculate_costs(self):
        #payload3
        test_data = {
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

        #POST request simulation to the /productionplan endpoint with the test data.
        response = self.app.post('/productionplan', 
                                 data=json.dumps(test_data),
                                 content_type='application/json')
        
        self.assertEqual(response.status_code, 200)
        
        data = json.loads(response.data)
        self.assertIsInstance(data, list)
        self.assertEqual(len(data), 6)  # Ensure all plants are in the response
        
        # Check if the sum of power outputs equals the load
        total_power = sum(plant['p'] for plant in data)
        self.assertAlmostEqual(total_power, test_data['load'], places=1)

        # Check if wind turbines are at their maximum possible output
        wind_plants = [p for p in data if p['name'].startswith('windpark')]
        for plant in wind_plants:
            max_output = next(p['pmax'] for p in test_data['powerplants'] if p['name'] == plant['name'])
            self.assertAlmostEqual(plant['p'], max_output * test_data['fuels']['wind(%)'] / 100, places=1)

    #Test to check how API handles invalid input
    def test_invalid_input(self):
        invalid_data = {"load": "not a number"}
        response = self.app.post('/productionplan', 
                                data=json.dumps(invalid_data),
                                content_type='application/json')
        print(f"Response status: {response.status_code}")
        print(f"Response content: {response.data}")
        self.assertEqual(response.status_code, 400)

if __name__ == '__main__':
    unittest.main()
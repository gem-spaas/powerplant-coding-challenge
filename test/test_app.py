import unittest
import json
from src.app import app  # Import the Flask app from src/app.py

class ProductionPlanTestCase(unittest.TestCase):
    def setUp(self):
        # Set up the Flask test client
        self.client = app.test_client()
        self.client.testing = True

        # Sample input data similar to payload3.json
        self.input_data = {
  "load": 480,
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

    def test_production_plan(self):
        # Send a POST request to the /productionplan endpoint
        response = self.client.post(
            '/productionplan',
            data=json.dumps(self.input_data),
            content_type='application/json'
        )

        # Assert that the response status code is 200 (success)
        self.assertEqual(response.status_code, 200)

        # Parse the JSON response data
        response_data = json.loads(response.data)

        # Check that the response contains a success message with the filename
        self.assertIn("message", response_data)
        self.assertTrue("Merouane_Hadouch_HEADMIND_production_plan_result.json" in response_data["message"])

   
if __name__ == '__main__':
    unittest.main()
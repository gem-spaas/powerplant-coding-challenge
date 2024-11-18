import unittest
import json
from app import create_app


class ProductionPlanTestCase(unittest.TestCase):
    """Test case for the production_plan route.

    Args:
        unittest (TestCase): The base test case class.
    """

    def setUp(self):
        self.app = create_app()
        self.app.testing = True
        self.client = self.app.test_client()
        self.app_context = self.app.app_context()
        self.app_context.push()

    def tearDown(self):
        self.app_context.pop()

    def test_production_plan_success(self):
        """Test a successful power distribution scenario."""

        payload = {
            "load": 480,
            "fuels": {"gas(euro/MWh)": 13.4, "kerosine(euro/MWh)": 50.8, "wind(%)": 60},
            "powerplants": [
                {
                    "name": "gasfiredbig1",
                    "type": "gasfired",
                    "efficiency": 0.53,
                    "pmin": 100,
                    "pmax": 460,
                },
                {
                    "name": "tj1",
                    "type": "turbojet",
                    "efficiency": 0.3,
                    "pmin": 0,
                    "pmax": 16,
                },
                {
                    "name": "windpark1",
                    "type": "windturbine",
                    "efficiency": 1,
                    "pmin": 0,
                    "pmax": 150,
                },
            ],
        }

        response = self.client.post(
            "/productionplan", data=json.dumps(payload), content_type="application/json"
        )
        self.assertEqual(response.status_code, 200)
        self.assertIsInstance(response.json, list)


if __name__ == "__main__":
    unittest.main()

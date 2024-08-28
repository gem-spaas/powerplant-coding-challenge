import json
import unittest
from fastapi.testclient import TestClient
from main import app
from models import PriceMap
import utils

client = TestClient(app)


def _reply2dict(data):
    return {item["name"]: item["p"] for item in data}


class IntegrationTest(unittest.TestCase):
    def test_default_payloads(self):
        for i in range(4):
            with open(f"./example_payloads/payload{i}.json") as file:
                req_body = json.load(file)
            with open(f"./example_payloads/response{i}.json") as file:
                res_body = json.load(file)

            response = client.post("/productionplan", json=req_body)
            self.assertEqual(response.status_code, 200)
            self.assertDictEqual(_reply2dict(response.json()), _reply2dict(res_body))

    def test_invalid_payload(self):
        response = client.post("/productionplan", json={"data": "invalid"})
        self.assertEqual(response.status_code, 422)

    def test_overproduction(self):
        # Low fuel cost but high Pmin
        body = {
            "load": 10,
            "fuels": {
                "gas(euro/MWh)": 1,
                "kerosine(euro/MWh)": 1000,
                "co2(euro/ton)": 0,
                "wind(%)": 0,
            },
            "powerplants": [
                {
                    "name": "gas1",
                    "type": "gasfired",
                    "efficiency": 1,
                    "pmin": 1000,
                    "pmax": 2000,
                },
                {
                    "name": "tj1",
                    "type": "turbojet",
                    "efficiency": 1,
                    "pmin": 0,
                    "pmax": 100,
                },
            ],
        }

        body['overproduction'] = True
        response = client.post("/productionplan", json=body)
        self.assertEqual(response.status_code, 200)
        self.assertDictEqual(_reply2dict(response.json()), {'gas1': 1000.0, 'tj1': 0.0}) # Cost = 1000$

        body['overproduction'] = False
        response = client.post("/productionplan", json=body)
        self.assertEqual(response.status_code, 200)
        self.assertDictEqual(
            _reply2dict(response.json()), {'gas1': 0.0, 'tj1': 10.0}  # Cost = 10 000$
        )

        body['load'] = 110
        response = client.post("/productionplan", json=body)
        self.assertEqual(response.status_code, 418)
        self.assertEqual(response.content.decode(), "Power production gap")


        body['load'] = 10000
        response = client.post("/productionplan", json=body)
        self.assertEqual(response.status_code, 418)
        self.assertEqual(response.content.decode(), "Not enough power")



class UnitTest(unittest.TestCase):
    def test_intersect(self):
        self.assertFalse(utils.find_intersection(1, 2, 3, 4, 4, 6))
        self.assertEqual(utils.find_intersection(1, 2, 3, 4, 5, 6), (7, 8))
        self.assertEqual(utils.find_intersection(1, 2, 3, 6, 3, 5), (2, 4))
        self.assertEqual(utils.find_intersection(1, 2, 3, 4, 4, 8), (-1, 0))

    def test_binary_search(self):
        pricelist = [PriceMap(i, i + 100, 0, 0, [], [], 0) for i in range(0, 1000, 100)]

        self.assertEqual(utils.binary_search(pricelist, -500), -1)
        self.assertEqual(utils.binary_search(pricelist, 200), 2)
        self.assertEqual(utils.binary_search(pricelist, 250), 2)
        self.assertEqual(utils.binary_search(pricelist, 5000), len(pricelist) - 1)


if __name__ == "__main__":
    unittest.main()

from app.models import EnergyDemand
from app.main import production_plan
import json


def test_production_plan():
    actual_response_powerplant_list = [
        "gasfiredbig1",
        "gasfiredbig2",
        "gasfiredsomewhatsmaller",
        "tj1",
        "windpark1",
        "windpark2",
    ]
    with open("example_payloads/payload1.json", "r") as json_file:
        payload = json.load(json_file)
    energy_demand = EnergyDemand(
        load=payload["load"], fuels=payload["fuels"], powerplants=payload["powerplants"]
    )
    response = production_plan(energy_demand)
    for powerplant in response:
        assert "name" in powerplant.keys()
        assert "p" in powerplant.keys()
        assert powerplant["name"] in actual_response_powerplant_list
        assert isinstance(powerplant["p"], (int, float))

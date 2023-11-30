from app.models import EnergyDemand
from app.main import production_plan
import json


def test_payload1():
    actual_repsponse = [
        {"name": "windpark1", "p": 90.0},
        {"name": "windpark2", "p": 21.6},
        {"name": "gasfiredbig1", "p": 368.4},
        {"name": "gasfiredbig2", "p": 0},
        {"name": "gasfiredsomewhatsmaller", "p": 0},
        {"name": "tj1", "p": 0},
    ]
    with open("example_payloads/payload1.json", "r") as json_file:
        payload = json.load(json_file)
    energy_demand = EnergyDemand(
        load=payload["load"], fuels=payload["fuels"], powerplants=payload["powerplants"]
    )
    response = production_plan(energy_demand)
    for powerplant in response:
        for actual_powerplant in actual_repsponse:
            if actual_powerplant["name"] == powerplant["name"]:
                assert actual_powerplant["p"] == powerplant["p"]
                continue


def test_payload2():
    actual_repsponse = [
        {"name": "windpark1", "p": 0.0},
        {"name": "windpark2", "p": 0.0},
        {"name": "gasfiredbig1", "p": 380.0},
        {"name": "gasfiredbig2", "p": 100.0},
        {"name": "gasfiredsomewhatsmaller", "p": 0},
        {"name": "tj1", "p": 0},
    ]
    with open("example_payloads/payload2.json", "r") as json_file:
        payload = json.load(json_file)
    energy_demand = EnergyDemand(
        load=payload["load"], fuels=payload["fuels"], powerplants=payload["powerplants"]
    )
    response = production_plan(energy_demand)
    for powerplant in response:
        for actual_powerplant in actual_repsponse:
            if actual_powerplant["name"] == powerplant["name"]:
                assert actual_powerplant["p"] == powerplant["p"]
                continue


def test_payload3():
    actual_repsponse = [
        {"name": "windpark1", "p": 90.0},
        {"name": "windpark2", "p": 21.6},
        {"name": "gasfiredbig1", "p": 460.0},
        {"name": "gasfiredbig2", "p": 338.4},
        {"name": "gasfiredsomewhatsmaller", "p": 0},
        {"name": "tj1", "p": 0},
    ]
    with open("example_payloads/payload3.json", "r") as json_file:
        payload = json.load(json_file)
    energy_demand = EnergyDemand(
        load=payload["load"], fuels=payload["fuels"], powerplants=payload["powerplants"]
    )
    response = production_plan(energy_demand)
    for powerplant in response:
        for actual_powerplant in actual_repsponse:
            if actual_powerplant["name"] == powerplant["name"]:
                assert actual_powerplant["p"] == powerplant["p"]
                continue

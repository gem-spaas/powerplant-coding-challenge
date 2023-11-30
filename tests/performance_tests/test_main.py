from app.models import EnergyDemand
from app.main import production_plan


def test_performance_main():
    demand = {
        "load": 480,
        "fuels": {
            "gas(euro/MWh)": 13.4,
            "kerosine(euro/MWh)": 50.8,
            "co2(euro/ton)": 20,
            "wind(%)": 60,
        },
        "powerplants": [
            {
                "name": "gasfiredbig1",
                "type": "gasfired",
                "efficiency": 0.9,
                "pmin": 600,
                "pmax": 650,
            },
            {
                "name": "gasfiredbig2",
                "type": "gasfired",
                "efficiency": 1,
                "pmin": 20,
                "pmax": 200,
            },
            {
                "name": "gasfiredsomewhatsmaller",
                "type": "gasfired",
                "efficiency": 0.5,
                "pmin": 170,
                "pmax": 210,
            },
            {
                "name": "tj1",
                "type": "turbojet",
                "efficiency": 0.3,
                "pmin": 0,
                "pmax": 1500,
            },
        ],
    }
    actual_repsponse = [
        {"name": "gasfiredbig1", "p": 600.0},
        {"name": "gasfiredbig1", "p": 0},
        {"name": "gasfiredsomewhatsmaller", "p": 0},
        {"name": "tj1", "p": 0},
    ]
    energy_demand = EnergyDemand(
        load=demand["load"], fuels=demand["fuels"], powerplants=demand["powerplants"]
    )
    response = production_plan(energy_demand)
    for powerplant in response:
        for actual_powerplant in actual_repsponse:
            if actual_powerplant["name"] == powerplant["name"]:
                assert actual_powerplant["p"] == powerplant["p"]
                continue

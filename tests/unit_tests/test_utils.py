from app.models import EnergyDemand
from app.utils import categorize_powerplants
from app.models import Gasfired
from app.models import Turbojet
from app.models import Windturbine
import json


def test_categorize_powerplants():
    with open("example_payloads/payload1.json", "r") as json_file:
        payload = json.load(json_file)
    energy_demand = EnergyDemand(
        load=payload["load"], fuels=payload["fuels"], powerplants=payload["powerplants"]
    )
    powerplants = categorize_powerplants(energy_demand)
    assert isinstance(powerplants, list)
    assert len(powerplants) == 6
    assert isinstance(powerplants[0], Windturbine)
    assert isinstance(powerplants[1], Windturbine)
    assert isinstance(powerplants[2], Gasfired)
    assert isinstance(powerplants[3], Gasfired)
    assert isinstance(powerplants[4], Gasfired)
    assert isinstance(powerplants[5], Turbojet)

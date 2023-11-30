from app.ucs import find_cost_ucs
from app.utils import categorize_powerplants
from app.models import EnergyDemand
import json


def test_find_cost_ucs():
    with open("example_payloads/payload1.json", "r") as json_file:
        demand = json.load(json_file)
    energy_demand = EnergyDemand(
        load=demand["load"], fuels=demand["fuels"], powerplants=demand["powerplants"]
    )
    powerplants = categorize_powerplants(energy_demand)
    demanded_load = energy_demand.load
    for powerplant in powerplants:
        produced_load, cost = find_cost_ucs(powerplant, demanded_load)
        assert isinstance(cost, (int, float))
        assert isinstance(produced_load, (int, float))

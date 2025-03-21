from typing import List, Dict
from enum import Enum

class PowerPlantType(Enum):
    GASFIRED = "gasfired"
    TURBOJET = "turbojet"
    WINDTURBINE = "windturbine"

class FuelCosts:
    def __init__(self, fuels: Dict[str, float]):
        self.gas = fuels.get("gas(euro/MWh)", 0.0)
        self.kerosine = fuels.get("kerosine(euro/MWh)", 0.0)
        self.co2 = fuels.get("co2(euro/ton)", 0.0)
        self.wind = fuels.get("wind(%)", 0.0)

class PowerPlant:
    def __init__(self, name: str, type: str, efficiency: float, pmax: float, pmin: float):
        self.name = name
        self.type = type
        self.efficiency = efficiency
        self.pmax = pmax
        self.pmin = pmin

class Payload:
    def __init__(self, load: float, fuels: Dict[str, float], powerplants: List[Dict]):
        self.load = load
        self.fuels = FuelCosts(fuels)
        self.powerplants = [PowerPlant(**pp) for pp in powerplants]

class ProductionPlanResponse:
    def __init__(self, name: str, p: float):
        self.name = name
        self.p = round(p, 1)

    def to_dict(self):
        return {"name": self.name, "p": self.p}
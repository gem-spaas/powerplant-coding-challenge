from dataclasses import dataclass
from typing import Dict,List

@dataclass
class FuelData:
    gas: float
    kerosine:float
    co2_cost:float
    wind:float

    @classmethod
    def get_data(cls,data:Dict):
        return cls(
        gas      = data.get("gas(euro/MWh)"),
        kerosine = data.get("kerosine(euro/MWh)"),
        co2_cost = data.get("co2(euro/ton)"),
        wind     = data.get("wind(%)") / 100,
        )
    

@dataclass
class PowerPlantData:
    name: str
    type: str
    efficiency:str
    pmin:int
    pmax:int

    @classmethod
    def get_data(cls,data:Dict):
        return cls(
            name = data.get('name'),
            type = data.get('type'),
            efficiency = data.get('efficiency'),
            pmin = data.get('pmin'),
            pmax = data.get('pmax'),
        )
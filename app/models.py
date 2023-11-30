from typing import List
from pydantic import BaseModel, Field
import math


class PowerPlantAssetModel(BaseModel):
    name: str
    type: str
    efficiency: float
    pmin: float
    pmax: float


class Fuels(BaseModel):
    gas_euro_mwh: float = Field(alias="gas(euro/MWh)")
    kerosine_euro_mwh: float = Field(alias="kerosine(euro/MWh)")
    co2_euro_ton: float = Field(alias="co2(euro/ton)")
    wind_percentage: float = Field(alias="wind(%)")


class EnergyDemand(BaseModel):
    load: float
    fuels: Fuels
    powerplants: List[PowerPlantAssetModel]


class PowerPlantAsset:
    def __init__(self, id, name, type, efficiency, pmin, pmax):
        self.id = id
        self.name = name
        self.type = type
        self.efficiency = efficiency
        self.min_generated_mwh = pmin
        self.max_generated_mwh = pmax
        self.current_cost = None
        self.current_load = None
        self.total_cost_per_mwh = None

    def __lt__(self, other):
        # This method is used for sorting instances
        return self.total_cost_per_mwh < other.total_cost_per_mwh

    def total_cost(self, load):
        return load * self.total_cost_per_mwh


class Gasfired(PowerPlantAsset):
    type = "gasfired"

    def __init__(self, id, name, efficiency, pmin, pmax, gas_euro_mwh, co2_euro_ton):
        super().__init__(id, name, self.type, efficiency, pmin, pmax)
        self.gas_euro_mwh = gas_euro_mwh
        self.co2_euro_ton = co2_euro_ton
        self.total_cost_per_mwh = (
            self.gas_euro_mwh / self.efficiency + 0.3 * self.co2_euro_ton
        )


class Turbojet(PowerPlantAsset):
    type = "turbojet"
    efficiency = 0.3
    pmin = 0

    def __init__(self, id, name, pmax, kerosine_euro_mwh):
        super().__init__(id, name, self.type, self.efficiency, self.pmin, pmax)
        self.kerosine_euro_mwh = kerosine_euro_mwh
        self.total_cost_per_mwh = self.kerosine_euro_mwh / self.efficiency


class Windturbine(PowerPlantAsset):
    type = "windturbine"
    efficiency = 1
    pmin = 0

    def __init__(self, id, name, pmax, wind_percentage):
        super().__init__(id, name, self.type, self.efficiency, self.pmin, pmax)
        self.wind_percentage = wind_percentage
        self.max_generated_mwh = math.floor(pmax * self.wind_percentage) / 100
        self.min_generated_mwh = self.pmin
        self.total_cost_per_mwh = 0

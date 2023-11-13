from typing import List
from pydantic import BaseModel, Field


class Fuels(BaseModel):
    gas_euro_mwh: float = Field(alias="gas(euro/MWh)")
    kerosine_euro_mwh: float = Field(alias="kerosine(euro/MWh)")
    co2_euro_ton: float = Field(alias="co2(euro/ton)")
    wind_percentage: float = Field(alias="wind(%)")

class PowerPlant(BaseModel):
    name: str
    pmin: float
    pmax: float
    efficiency: float
    type: str


class Payload(BaseModel):
    load: float
    fuels: Fuels
    powerplants: List[PowerPlant]

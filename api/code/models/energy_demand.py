"""
    This class provide the abstraction of the payload messages received from client using pydantic library.



"""

from typing import List
from pydantic import BaseModel
from pydantic.fields import Field


class PowerPlantDemand(BaseModel):
    name: str
    type: str    
    efficiency: float
    pmax: float
    pmin: float


class FuelsEnergyDemand(BaseModel):
    gas: float = Field(
        title="gas(euro/MWh)",
        description="The price of gas per MWh"
    )
    kerosine: float = Field(
        title="kerosine(euro/MWh)",
        description="The price of kerosine per MWh"
    )
    co2: float = Field(
        title="co2(euro/ton)",
        description="The price of emission allowances"
    )
    wind: float = Field(
        title="wind(%)",
        description="Percentage of wind"
    )


class EnergyDemand(BaseModel):
    load: int
    fuels: FuelsEnergyDemand
    powerplants: List[PowerPlantDemand]

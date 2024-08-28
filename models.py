from dataclasses import dataclass
from enum import Enum
from typing import List

from pydantic import BaseModel, Field, NonNegativeFloat, PositiveFloat, model_validator


class PlantTypes(str, Enum):
    GASFIRED = "gasfired"
    TURBOJET = "turbojet"
    WINDTURBINE = "windturbine"


class Powerplant(BaseModel):
    name: str
    type: PlantTypes
    efficiency: PositiveFloat
    pmin: NonNegativeFloat
    pmax: NonNegativeFloat

    @model_validator(mode="after")
    def check_min_max_power(self):
        assert self.pmin <= self.pmax, "pmin > pmax"
        return self


class Fuels(BaseModel):
    gas: NonNegativeFloat = Field(alias="gas(euro/MWh)")
    kerosine: NonNegativeFloat = Field(alias="kerosine(euro/MWh)")
    co2: NonNegativeFloat = Field(alias="co2(euro/ton)")
    wind: NonNegativeFloat = Field(alias="wind(%)")


class Req(BaseModel):
    load: NonNegativeFloat
    fuels: Fuels
    powerplants: List[Powerplant]
    overproduction: bool = Field(default=False)


class Res(BaseModel):
    name: str
    p: NonNegativeFloat


@dataclass
class PriceMap:
    """
        holds the cost of generating power between pmin and pmax (cmin cmax)
        when the plants in low are runing at minimum power and the one in high runing at max
        and we vary the power produce by plant pid
    """
    pmin: float
    pmax: float
    cmin: float
    cmax: float
    low: List[int]
    high: List[int]
    pid: int

from typing import Literal, Annotated

from annotated_types import MinLen
from pydantic import BaseModel, Field, NonNegativeFloat, ConfigDict


class Fuels(BaseModel):
    gas: float = Field(alias="gas(euro/MWh)")
    kerosine: float = Field(alias="kerosine(euro/MWh)")
    co2: float = Field(alias="co2(euro/ton)")
    wind: float = Field(alias="wind(%)")

    model_config = ConfigDict(populate_by_name=True)


class PowerPlantIn(BaseModel):
    name: str
    type: Literal["gasfired", "turbojet", "windturbine"]
    efficiency: float
    pmin: float
    pmax: float


class ProductionPlanRequest(BaseModel):
    load: NonNegativeFloat
    fuels: Fuels
    powerplants: Annotated[list[PowerPlantIn], MinLen(1)]

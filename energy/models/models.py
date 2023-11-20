from pydantic import BaseModel, Field, validator
from typing import List


class PowerPlant(BaseModel):
    name: str
    type: str
    efficiency: float
    pmax: float
    pmin: float


class FuelCost(BaseModel):
    gas: float = Field(..., alias="gas(euro/MWh)")
    kerosine: float = Field(..., alias="kerosine(euro/MWh)")
    co2: int = Field(..., alias="co2(euro/ton)")
    wind: int = Field(..., alias="wind(%)")

    @validator("wind")
    def validate_wind(cls, v: int) -> int:
        if (v < 0) or (v > 100):
            raise ValueError("Wind should be between 0 and 100")
        return v


class Payload(BaseModel):
    load: float
    fuels: FuelCost
    powerplants: List[PowerPlant]

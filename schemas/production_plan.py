from enum import Enum

from pydantic import BaseModel, Field


class Fuels(BaseModel):
    gas_euro_per_MWh: float = Field(alias="gas(euro/MWh)", gt=0.0)
    kerosine_euro_per_MWh: float = Field(alias="kerosine(euro/MWh)", gt=0.0)
    co2_euro_per_ton: float = Field(alias="co2(euro/ton)", gt=0.0)
    wind_percent: int = Field(alias="wind(%)", ge=0, le=100)


class PowerPlantType(str, Enum):
    GAS_FIRED = "gasfired"
    WIND_TURBINE = "windturbine"
    TURBOJET = "turbojet"


class PowerPlant(BaseModel):
    name: str
    type: PowerPlantType
    efficiency: float = Field(..., gt=0.0)
    pmin: float = Field(..., ge=0.0)
    pmax: float = Field(..., gt=0.0)


class ProductionPlanRequest(BaseModel):
    load: float = Field(..., gt=0.0)
    fuels: Fuels
    powerplants: list[PowerPlant]


class ProductionPlanResponseItem(BaseModel):
    name: str
    p: float = Field(..., ge=0.0)


ProductionPlanResponse = list[ProductionPlanResponseItem]

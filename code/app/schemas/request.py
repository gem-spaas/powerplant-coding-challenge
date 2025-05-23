from pydantic import BaseModel, Field, confloat, conlist
from typing import List, Literal

PowerPlantType = Literal["gasfired", "turbojet", "windturbine"]

class Fuels(BaseModel):
    gas: float = Field(..., alias="gas(euro/MWh)", gt=0, description="Price of gas per megawatt-hour euro/MWh)")
    kerosine: float = Field(..., alias="kerosine(euro/MWh)", gt=0, description="Price of kerosene per megawatt-hour (euro/MWh)")
    co2: float = Field(..., alias="co2(euro/ton)", ge=0, description="Cost of CO2 emission allowances per ton (euro/ton)")
    wind: float = Field(..., alias="wind(%)", ge=0, le=100, description="Wind availability percentage (0-100%)")

class PowerPlant(BaseModel):
    name: str
    type: PowerPlantType
    efficiency: confloat(gt=0, le=1)
    pmin: float = Field(..., ge=0, description="Potencia mínima cuando está activa")
    pmax: float = Field(..., ge=0, description="Potencia máxima generable")

class ProductionPlanRequest(BaseModel):
    load: float = Field(..., gt=0, description="Total load required")
    fuels: Fuels
    powerplants: conlist(PowerPlant, min_length=1)
from typing import List, Literal

from pydantic import BaseModel, Field, model_validator


class Fuels(BaseModel):
    """
    Fuels

    Args:
        gas (float): Gas euro/MWh
        kerosine (float): Kerosine euro/MWh
        co2 (float): Co2 euro/ton
        wind (float): Wind %
    """

    gas: float = Field(alias="gas(euro/MWh)", ge=0)
    kerosine: float = Field(alias="kerosine(euro/MWh)", ge=0)
    co2: float = Field(alias="co2(euro/ton)", ge=0)
    wind: float = Field(alias="wind(%)", ge=0, le=100)

    # pydantic config that allows to populate by name
    model_config = {"populate_by_name": True}


class PowerPlant(BaseModel):
    """
    PowerPlant

    Args:
        name (str): Name
        type (Literal["gasfired", "turbojet", "windturbine"]): Type
        efficiency (float): Efficiency
        pmax (float): Pmax
        pmin (float): Pmin
    """

    name: str
    type: Literal["gasfired", "turbojet", "windturbine"]
    efficiency: float = Field(gt=0, le=1)
    pmax: float = Field(gt=0)
    pmin: float = Field(ge=0)

    @model_validator(mode="after")
    def check_pmax_gt_pmin(self):
        if self.pmin > self.pmax:
            raise ValueError("pmax must be greater than pmin")
        return self


class ProductionPlanRequest(BaseModel):
    """
    ProductionPlanRequest

    Args:
        load (float): Load
        fuels (Fuels): Fuels
        powerplants (List[PowerPlant]): Powerplants
    """

    load: float = Field(gt=0)
    fuels: Fuels
    powerplants: List[PowerPlant] = Field(min_length=1)

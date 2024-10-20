from decimal import Decimal
from pydantic import BaseModel, Field


class FuelModel(BaseModel):
    gas: Decimal = Field(alias="gas(euro/MWh)")
    kerosine: Decimal = Field(alias="kerosine(euro/MWh)")
    co2: Decimal = Field(alias="co2(euro/ton)")
    wind: Decimal = Field(alias="wind(%)")


class PowerplantsModel(BaseModel):
    name: str
    type: str
    efficiency: Decimal = Field(le=1, gt=0)
    pmin: Decimal
    pmax: Decimal

    def get_cost_eficiency(self, price):
        if self.type == 'windturbine':
            return 0
        return price / self.efficiency


class Payload(BaseModel):
    load: int
    fuels: FuelModel
    powerplants: list[PowerplantsModel]


class Response(BaseModel):
    name: str
    p: Decimal

from enum import Enum

from pydantic import BaseModel, Field
from pydantic.types import Decimal


class PlantType(str,Enum):
    """The different types of powerplants we care"""
    gasfired = "gasfired"
    turbojet = "turbojet"
    windturbine = "windturbine"


class Fuels(BaseModel):
    """The available fueltype and amount/price for powerplants"""
    gas: float = Field(description="I have questions...", alias="gas(euro/MWh)")
    kerosine: float = Field(description="I have questions...", alias="kerosine(euro/MWh)")
    co2: float = Field(description="I have questions...", alias="co2(euro/ton)")
    wind: float = Field(description="I have questions...", alias="wind(%)")


class PowerPlant(BaseModel):
    """The properties of a given PowerPlant"""
    name: str = Field(description="Name of the Powerplant")
    type: PlantType = Field(description="Type of the Powerplant")
    efficiency: float = Field(description="Efficiency to turn fuel to Electricity of the Powerplant")
    pmin: float
    pmax: float


class ProductionPlanInput(BaseModel):
    """The input model that contains all the necessary data to generate the ProductionPlan"""
    load: float = Field(description="The amount of continuous demand of power")
    fuels: Fuels = Field(description="I have questions...")
    powerplants: list[PowerPlant] = Field(description="List of available Powerplants with its properties")


class ProductionPlan(BaseModel):
    """The input model that contains all the necessary data to generate the ProductionPlan"""
    name: str = Field(description="Name of the Powerplant")
    p: Decimal = Field(description="Requested electricity from the powerplant, rounded o the closest integer", decimal_places=1)

# Third-party imports
from enum import Enum
from typing import List
from pydantic import BaseModel, PositiveFloat, NonNegativeFloat, field_validator, model_validator, Field


# Internal runtime class used during the production plan calculation
class PowerPlant:
    """
    Represents a power plant used in runtime calculations.
    Contains computed attributes such as cost per MWh and available power.
    """
    def __init__(self, name: str, type: str, efficiency: float, pmin: float, pmax: float):
        self.name = name
        self.type = type
        self.efficiency = efficiency
        self.pmin = pmin
        self.pmax = pmax
        self.cost_per_mwh = 0.0        # Computed cost in €/MWh
        self.available_capacity = 0.0  # Adjusted maximum power after wind, etc.


# Enum representing allowed types of power plants
class PlantTypes(str, Enum):
    """
    Allowed power plant types.
    """
    GASFIRED = "gasfired"
    TURBOJET = "turbojet"
    WINDTURBINE = "windturbine"


# Input model for a power plant (from client request)
class PowerPlantData(BaseModel):
    """
    Input schema for each power plant.
    Validates efficiency and power constraints.
    """
    name: str
    type: PlantTypes
    efficiency: PositiveFloat
    pmin: NonNegativeFloat
    pmax: NonNegativeFloat

    @model_validator(mode="after")
    def check_min_max_power(self):
        """
        Ensures that pmin does not exceed pmax.
        """
        if self.pmin > self.pmax:
            raise ValueError(f"{self.name}: pmin must be less than or equal to pmax")
        return self


# Input model for fuel prices and wind percentage
class Fuels(BaseModel):
    """
    Fuel prices and wind availability used in cost calculations.
    Field aliases match the keys expected in the input JSON.
    """
    gas: NonNegativeFloat = Field(alias="gas(euro/MWh)")
    kerosine: NonNegativeFloat = Field(alias="kerosine(euro/MWh)")
    co2: NonNegativeFloat = Field(alias="co2(euro/ton)")
    wind: NonNegativeFloat = Field(alias="wind(%)")


# Main input model for the API request
class ProductionPlanRequest(BaseModel):
    """
    Full request body containing the required load,
    fuel data, power plant list, and an optional overproduction flag.
    """
    load: NonNegativeFloat
    fuels: Fuels
    powerplants: List[PowerPlantData]


# Output model for the API response
class ProductionPlanResponse(BaseModel):
    """
    Response model for the production plan output.
    Indicates how much power each plant will produce.
    """
    name: str
    p: NonNegativeFloat

    @field_validator('p')
    def round_p_to_one_decimal(cls, v):
        return round(v, 1)
    
    def model_dump(self, *args, **kwargs):
        d = super().model_dump(*args, **kwargs)
        d["p"] = float(f"{d['p']:.1f}") 
        return d

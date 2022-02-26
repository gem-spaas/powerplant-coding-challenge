import enum
import typing
from http import HTTPStatus

from pydantic import BaseModel, Field, validator
from fastapi import HTTPException


class FuelsCosts(BaseModel):
    gas: float = Field(alias="gas(euro/MWh)")
    kerosine: float = Field(alias="kerosine(euro/MWh)")
    co2: int = Field(alias="co2(euro/ton)")
    wind: int = Field(alias="wind(%)")


class PowerPlantType(str, enum.Enum):
    TURBO_JET = "turbojet"
    GAS_FIRED = "gasfired"
    WIND_TURBINE = "windturbine"


class PowerPlant(BaseModel):
    name: str
    efficiency: float
    power_min: int = Field(alias="pmin")
    power_max: int = Field(alias="pmax")
    power_plant_type: PowerPlantType = Field(alias="type")

    def compute_cost_per_mwh(self, fuels: FuelsCosts):
        cost = 0
        if self.power_plant_type == PowerPlantType.GAS_FIRED:
            cost = fuels.gas / self.efficiency
        if self.power_plant_type == PowerPlantType.TURBO_JET:
            cost = fuels.kerosine / self.efficiency
        if self.power_plant_type == PowerPlantType.WIND_TURBINE:
            cost = 0
        return cost

    def compute_real_power(self, power_in: int) -> int:
        return round(power_in * self.efficiency)


class PlantLoad(BaseModel):
    plant: PowerPlant
    load: int

    @validator("load")
    def validate_load(cls, value: int, values, **kwargs):
        if value != 0:
            if value > values["plant"].power_max or value < values["plant"].power_min:
                raise ValueError("Load cannot be applied to power plant.")
        return value

    def compute_max_load(self, wind_percentage: int):
        load = self.plant.compute_real_power(self.plant.power_max)
        if self.plant.power_plant_type == PowerPlantType.WIND_TURBINE:
            real_load = load * wind_percentage / 100.0
        else:
            real_load = load
        return real_load

    def compute_min_load(self, wind_percentage: int):
        load = self.plant.compute_real_power(self.plant.power_min)
        if self.plant.power_plant_type == PowerPlantType.WIND_TURBINE:
            real_load = load * wind_percentage / 100.0
        else:
            real_load = load
        return real_load


class PlantLoadResult(BaseModel):
    plant_name: str = Field(alias="name")
    power: int = Field(alias="p")

    @classmethod
    def from_power_plants_load(cls, power_plants_load: typing.List[PlantLoad]):
        return [
            cls(name=power_plant_load.plant.name, p=power_plant_load.load)
            for power_plant_load in power_plants_load
        ]


class ProductionPlan(BaseModel):
    class Config:
        underscore_attrs_are_private = True
    load: int
    fuels: FuelsCosts
    power_plants: typing.List[PowerPlant] = Field(alias="powerplants")
    _used_plant_loads: typing.Optional[typing.List[PlantLoad]]
    _available_plant_loads: typing.Optional[typing.List[PlantLoad]]

    def optimize(self):
        self._used_plant_loads = []
        missing_load = self.load
        self._available_plant_loads = [
            PlantLoad(plant=plant, load=0) for plant in self.power_plants
        ]
        self._available_plant_loads.sort(
            key=lambda x: x.plant.compute_cost_per_mwh(fuels=self.fuels), reverse=True
        )
        while missing_load:
            if self._available_plant_loads:
                missing_load = self.assign_load(missing_load)
            else:
                raise HTTPException(
                    status_code=HTTPStatus.BAD_REQUEST,
                    detail="Plants are not enough for load",
                )
        self._used_plant_loads.extend(self._available_plant_loads)
        self._available_plant_loads = []
        return PlantLoadResult.from_power_plants_load(self._used_plant_loads)

    def assign_load(self, missing_load: int):
        plant_load_to_be_assign = self._available_plant_loads.pop()
        max_load = plant_load_to_be_assign.compute_max_load(self.fuels.wind)
        load_to_be_taken = max_load if missing_load > max_load else missing_load
        if load_to_be_taken < plant_load_to_be_assign.compute_min_load(self.fuels.wind):
            load_to_be_taken, plant_load_to_be_assign = self.compensate_min_load(
                load_to_be_taken, plant_load_to_be_assign
            )
        plant_load_to_be_assign.load += load_to_be_taken
        missing_load -= load_to_be_taken
        self._used_plant_loads.insert(0, plant_load_to_be_assign)
        return missing_load

    def compensate_min_load(self, _load_to_be_taken: int, _plant_load_to_be_assign):
        load_to_be_taken = _load_to_be_taken
        plant_load_to_be_assign = _plant_load_to_be_assign.copy()
        missing_load_for_the_min = (
            plant_load_to_be_assign.compute_min_load(self.fuels.wind) - load_to_be_taken
        )
        for plant in self._used_plant_loads:
            available = plant.load - plant.compute_min_load(self.fuels.wind)
            to_remove_load = (
                missing_load_for_the_min
                if available >= missing_load_for_the_min
                else available
            )
            missing_load_for_the_min -= to_remove_load
            plant.load -= to_remove_load
            plant_load_to_be_assign.load += to_remove_load
            if not missing_load_for_the_min:
                break
        return load_to_be_taken, plant_load_to_be_assign

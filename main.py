from typing import Optional

import uvicorn
from fastapi import FastAPI
from pydantic import BaseModel, Field

app = FastAPI()


class Fuels(BaseModel):
    gas: float = Field(gt=0, alias="gas(euro/MWh)")
    kerosine: float = Field(gt=0, alias="kerosine(euro/MWh)") 
    co2: int = Field(ge=0, alias="co2(euro/ton)")
    wind: int = Field(ge=0, alias="wind(%)")


class Powerplant(BaseModel):
    name: str
    type: str
    efficiency: float
    pmin: int
    pmax: int
    cost_per_MWH: Optional[float] = None
    pmax: Optional[int] = None


class InputData(BaseModel):
    load: int
    fuels: Fuels
    powerplants: list[Powerplant]


class ResultItem(BaseModel):
    name: str
    p: float


class InvalidPlantTypeProvided(Exception):
    pass


class ManualInterventionNeeded(Exception):
    pass


class ProductionPlanCalculator:
    def __init__(self, input_data: InputData):
        self.load = input_data.load
        self.fuels = input_data.fuels
        self.powerplants = input_data.powerplants

    def calculate_cost_per_MWH(self, powerplant: Powerplant):
        if powerplant.type == "gasfired":
            cost_per_MWH = self.fuels.gas / powerplant.efficiency
        elif powerplant.type == "turbojet":
            cost_per_MWH = self.fuels.kerosine / powerplant.efficiency
        elif powerplant.type == "windturbine":
            cost_per_MWH = 0
        else:
            raise InvalidPlantTypeProvided

        return cost_per_MWH

    def calculate_pmax(self, powerplant: Powerplant):
        if powerplant.type == "windturbine":
            pmax = powerplant.pmax * self.fuels.wind / 100
        elif powerplant.type in ["gasfired", "turbojet"]:
            pmax = powerplant.pmax
        return pmax

    def get_sorted_powerplants_with_pmax_and_cost(self):
        plants = list(self.powerplants)
        for powerplant in plants:
            powerplant.cost_per_MWH = self.calculate_cost_per_MWH(powerplant)
            powerplant.pmax = self.calculate_pmax(powerplant)

        return sorted(plants, key=lambda x: x.cost_per_MWH)

    def get_production_plan(self):
        plants = self.get_sorted_powerplants_with_pmax_and_cost()
        production_plan = []
        remaining_load = self.load
        for plant in plants:
            if remaining_load > 0:
                if remaining_load < plant.pmin:
                    raise ManualInterventionNeeded("Remaining load greater than pmin."
                                                   "Manual intervention required to adjust the load.")
                elif remaining_load >= plant.pmax:
                    power = float(round(plant.pmax, 1))
                elif plant.pmin <= remaining_load < plant.pmax:
                    power = float(round(remaining_load, 1))
                remaining_load = remaining_load - power
                production_plan.append(ResultItem(name=plant.name, p=power))
            else:
                power = float(0)
                production_plan.append(ResultItem(name=plant.name, p=power))
        return production_plan


@app.get("/")
async def root():
    return {"message": "Post a correct payload to /productionplan"}


@app.post("/productionplan")
async def get_production_plan(input_data: InputData):
    prodplan_calculator = ProductionPlanCalculator(input_data)
    try:
        result = prodplan_calculator.get_production_plan()
    except ManualInterventionNeeded:
        result = {"error": "Failed to calculate production plan automatically. "
                           "Remaining load exceeds next powerplant min power. "
                           "Some cheaper plants might need to be switched off."}
    return result


if __name__ == "__main__":
    uvicorn.run(app, host="0.0.0.0", port=8888)

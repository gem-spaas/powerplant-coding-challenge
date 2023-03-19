import math
from collections import namedtuple
from typing import List, Tuple

from models import PowerPlant
from models.powerplant_model import Fuels, PlantType, ProductionPlanInput, ProductionPlan

OrderedPlant = namedtuple("OrderedPlant", "Name PMin PMax UnitOfCost")


def _unit_of_cost_in_euro(powerPlant: PowerPlant, fuel_cost: Fuels) -> float:
    """Calculates how many euros it cost to generate 1Kwh Electricity in a given Powerplant"""
    if powerPlant.type == PlantType.windturbine:
        return 0.0
    elif powerPlant.type == PlantType.gasfired:
        return 1.0 / powerPlant.efficiency * fuel_cost.gas
    elif powerPlant.type == PlantType.turbojet:
        return 1.0 / powerPlant.efficiency * fuel_cost.kerosine
    else:
        raise AttributeError(f"Unknown PowerPlant type: {powerPlant.type}")


def _get_merit_order(powerplants: List[PowerPlant], fuel_cost: Fuels) -> list[OrderedPlant]:
    """Orders the Powerlants to merit order"""
    unordered_plant_properties = [
        OrderedPlant(plant.name, plant.pmin, plant.pmax, _unit_of_cost_in_euro(plant, fuel_cost)) for
        plant in powerplants]
    return sorted(unordered_plant_properties, key=lambda x: x[3])


def generate_productionplan(input: ProductionPlanInput):
    merit_ordered_plants = _get_merit_order(input.powerplants, input.fuels)
    load_remained = input.load

    for plant in merit_ordered_plants:
        if math.isclose(load_remained, 0.0):
            yield ProductionPlan(name=plant.Name, p=0)
        if plant.PMin > load_remained:
            yield ProductionPlan(name=plant.Name, p=plant.PMin)
            load_remained = 0
        elif plant.PMin <= load_remained <= plant.PMax:
            yield ProductionPlan(name=plant.Name, p=load_remained)
            load_remained = 0
        else:
            yield ProductionPlan(name=plant.Name, p=plant.PMax)
            load_remained -= plant.PMax

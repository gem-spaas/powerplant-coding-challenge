import json
import re

from flask import current_app

class plantInfo:
    def __init__(self, name=None, type=None, efficiency=0.0, pmin=0, pmax=0):
        self.name = name
        self.type = type
        self.pmin = pmin
        self.pmax = pmax
        self.powerToProduce = 0
        self.efficiency = efficiency

class payLoadSolverService:
    def __init__(self, payLoad=0, fuels=None, powerplants=None):
        self.payLoad = payLoad
        self.fuels = {}
        self.powerplants = []
        self.availablePowerplants = []

    def calculateMeritOrder(self):
        plants = self.powerplants
        order = []
        if self.fuels["wind"].price == 0:
            plants = self.windPlantsRemover(plants)
        for plant in plants:
            order.append((self.powerplants.index(plant), (self.fuels[checkDict[plant.type]].price * (1 - plant.efficiency))))
        order.sort(key=lambda p: p[1])
        return order

    def compute_production_plan(self):
        load = 0
        plantMark = 0
        while load != self.payLoad:
            merit_order = self.calculateMeritOrder()
            candidatePlant = self.powerplants[merit_order[plantMark][0]]
            multiplicator = 1
            if candidatePlant.type == "windturbine":
                multiplicator = self.fuels["wind"].price / 100
            powerGenerated = (candidatePlant.pmax - candidatePlant.pmin) * multiplicator
            loadLeft = powerGenerated + load
            if loadLeft > self.payLoad:
                powerGenerated -= loadLeft - self.payLoad
                if powerGenerated < candidatePlant.pmin:
                    plantMark += 1
                    continue
            candidatePlant.powerToProduce = round(powerGenerated, 1)
            load += candidatePlant.powerToProduce
            plantMark += 1
            self.availablePowerplants.append(candidatePlant)

    def create_response(self):
        response = []
        for candidate in self.powerplants:
            response.append({"name": candidate.name, "p": candidate.powerToProduce})
        return response

    def windPlantsRemover(self, plants):
        return list(filter(
            lambda p: p.type != "windturbine", plants
        ))

    @staticmethod
    def fileReader(payload):
        content = open(payload, 'r').read()
        payload = json.loads(content)
        payLoadSolverObject = payLoadSolverService()
        payLoadSolverObject.payLoad = payload["load"]
        fuels = payload["fuels"]
        for key, val in fuels.items():
            name = key.split("(")[0]
            unit = re.search(r".*\((.*)\)", key).group(1)
            price = val
            payLoadSolverObject.fuels[name] = Fuel(name, unit, price)
        for candidate in payload["powerplants"]:
            payLoadSolverObject.powerplants.append(plantInfo(**candidate))

        return payLoadSolverObject

checkDict = {
    "gasfired": "gas",
    "turbojet": "kerosine",
    "windturbine": "wind"
}

class Fuel:
    def __init__(self, name, unit, price):
        self.name = name
        self.unit = unit
        self.price = price

def solver(path = "payload1.json"):
    payLoadSolverObject = payLoadSolverService.fileReader(path)
    payLoadSolverObject.compute_production_plan()
    response = payLoadSolverObject.create_response()
    return response

# if __name__ == "__main__":
#     result = solver()
#     print(result)

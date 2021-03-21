import json
import re

from flask import current_app

_lookup = {
    "gasfired": "gas",
    "turbojet": "kerosine",
    "windturbine": "wind"
}


class PowerPlant:
    def __init__(self, name=None, type=None, efficiency=0.0, pmin=0, pmax=0):
        self.name = name
        self.type = type
        self.efficiency = efficiency
        self.pmin = pmin
        self.pmax = pmax
        self.produced_power = 0

    def __str__(self):
        return f"{self.name}: {self.type}, power parameters: {self.pmin} - {self.pmax} / {self.efficiency}\nproduced: {self.produced_power}"

    def __repr__(self):
        return f"{self.name}"


class Fuel:
    def __init__(self, name, unit, price):
        self.name = name
        self.unit = unit
        self.price = price


class Network:
    def __init__(self, load=0, fuels=None, powerplants=None):
        self.load = load
        self.fuels = {} if fuels is None else fuels
        self.powerplants = [] if powerplants is None else powerplants
        self.activated_plants = []

    def remove_wind_from_plants(self, plants):
        return list(filter(
            lambda p: p.type != "windturbine", plants
        ))

    def define_merit_order(self):
        plants = self.powerplants
        # print(plants)
        order = []
        if self.fuels["wind"].price == 0:
            plants = self.remove_wind_from_plants(plants)
            # print("after checking",plants)
        mark = 0
        for plant in plants:
            mark += 1
            order.append((self.powerplants.index(plant), (self.fuels[_lookup[plant.type]].price * (1 - plant.efficiency))))
            # print("this is mark", mark)
            # print(plant)
            # print(self.fuels[_lookup[plant.type]].price)
        order.sort(key=lambda p: p[1])
        # print(order)
        return order

    def compute_production_plan(self):
        load = 0
        plant_count = 0
        while load != self.load:
            merit_order = self.define_merit_order()
            # print(merit_order)
            # current_app.logger.debug(f"merit_order: {merit_order}")
            current_plant = self.powerplants[merit_order[plant_count][0]]
            # print(current_plant)
            # print("test\n")
            multiplicator = 1
            if current_plant.type == "windturbine":
                multiplicator = self.fuels["wind"].price / 100
            produced_power = (current_plant.pmax - current_plant.pmin) * multiplicator
            # print(produced_power)
            new_load = produced_power + load
            if new_load > self.load:
                # if the power outweights the target load
                # we remove the difference between the "new" load that will
                # be reached by adding that produced_power
                # and the target load.
                produced_power -= new_load - self.load
                if produced_power < current_plant.pmin:
                    plant_count += 1
                    continue
            current_plant.produced_power = round(produced_power, 1)
            load += current_plant.produced_power
            # current_app.logger.debug(f"current load on network is {load}")
            # Increment the counter in order to get the next plant
            plant_count += 1
            self.activated_plants.append(current_plant)

    def create_response(self):
        response = []
        for plant in self.powerplants:
            response.append({"name": plant.name, "p": plant.produced_power})
        return response

    @staticmethod
    def load_payload_info_from_json(payload):
        content = open(payload, 'r').read()
        payload = json.loads(content)

        network = Network()

        network.load = payload["load"]

        fuels = payload["fuels"]
        for key, val in fuels.items():
            name = key.split("(")[0]
            unit = re.search(r".*\((.*)\)", key).group(1)
            price = val
            network.fuels[name] = Fuel(name, unit, price)

        for plant in payload["powerplants"]:
            network.powerplants.append(PowerPlant(**plant))

        # current_app.logger.info("Loaded new network")
        # current_app.logger.debug(network)

        return network

# if __name__ == "__main__":
def solver(path = "C:\\Users\\15754\\Desktop\\homework\\payload1.json"):
    # network = Network.load_payload_info_from_json("C:\\Users\\15754\\Desktop\\homework\\payload1.json")
    network = Network.load_payload_info_from_json(path)
    network.compute_production_plan()
    response = network.create_response()
    return response

if __name__ == "__main__":
    result = solver()
    print(result)

#!/usr/bin/env python3

from abc import abstractmethod

class PowerPlant:
    """Class definition for PowerPlant."""
    def __init__(self, plant=None):
        if plant is not None:
            self.plant=plant
        else:
            self.plant={}

    @abstractmethod
    def maxSupply(self):
        pass

    def name(self):
        return self.plant["name"] if self.plant["name"] else ""
    def type(self):
        return self.plant["type"] if self.plant["type"] else ""
    def efficiency(self):
        return self.plant["efficiency"] if self.plant["efficiency"] else 0
    def pmin(self):
        return self.plant["pmin"] if self.plant["pmin"] else 0
    def pmax(self):
        return self.plant["pmax"] if self.plant["pmax"] else 0
    def cost(self):
        return self.plant["cost(euro/MWh)"] if self.plant["cost(euro/MWh)"] else 0

    def __str__(self):
        return "{} is type {} with efficiency={}, Pmax={}, Pmin={} and cost={}.\n".format(
            self.name(), self.type(), self.efficiency(), self.pmin(), self.pmax(), self.cost()
        )

class WindTurbine(PowerPlant):
    def __init__(self, plant, fuels):
        super().__init__(plant)
        self.plant["cost(euro/MWh)"] = 0
        self.wind = fuels["wind(%)"]

    def maxSupply(self):
        return self.pmax() * self.wind / 100

class TurboJet(PowerPlant):
    def __init__(self, plant, fuels):
        super().__init__(plant)
        self.plant["cost(euro/MWh)"] = fuels["kerosine(euro/MWh)"] / self.plant["efficiency"]

    def maxSupply(self):
        return self.pmax()

class GasFired(PowerPlant):
    def __init__(self, plant, fuels):
        super().__init__(plant)
        self.plant["cost(euro/MWh)"] = fuels["gas(euro/MWh)"] / self.plant["efficiency"]
   
    def maxSupply(self):
        return self.pmax()

class PowerPlantFactory:
    def powerPlant(self, plant, fuels):
        if plant is None:
            raise ValueError("Invalid power plant provided: {}".format(plant))
        elif plant["type"] == "gasfired":
            return GasFired(plant, fuels)
        elif plant["type"] == "turbojet":
            return TurboJet(plant, fuels)
        elif plant["type"] == "windturbine":
            return WindTurbine(plant, fuels)
        else:
            raise ValueError("Invalid power plant type provided: '{}'".format(plant["type"]))

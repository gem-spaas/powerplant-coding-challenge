#!/usr/bin/env python3

from gem.plant import PowerPlantFactory

class PowerSupply:
    """Class definition for PowerSupply."""
    def __init__(self, payload=None):
        self.payload = payload if payload is not None else None

    def setPayload(self, payload=None):
        if payload is not None:
            self.payload = payload

    def meritOrder(self, fuels, power_plants):
        plants = []
        plantFactory = PowerPlantFactory()
        for p in power_plants:
            power_plant = plantFactory.powerPlant(p, fuels)
            plants.append(power_plant)        
        ordered_plants = sorted(plants, key=lambda k: k.cost(), reverse=True)
        return ordered_plants
    
    def supply(self, remainingLoad, power_plants):
        response = []
        while remainingLoad > 0:
            if len(power_plants):
                power_plant = power_plants.pop()
                load = min(power_plant.maxSupply(), remainingLoad)
                response.append({ "name": power_plant.name(), "p": load })
                remainingLoad -= load
            else:
                raise ValueError("CRITICAL: Power plants exhausted. Remaining load: {} MWh".format(load))
        while len(power_plants) > 0:
            response.append({ "name": power_plants.pop().name(), "p": 0 })
        return response

    def calculate_production(self):
        return self.supply( self.payload.load(), self.meritOrder(self.payload.fuels(), self.payload.powerPlants()) )

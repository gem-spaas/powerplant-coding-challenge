class Powerplant:
    def __init__(self, name, type, efficiency, pmin, pmax):
        self.name = name
        self.type = type
        self.efficiency = efficiency
        self.pmin = pmin
        self.pmax = pmax
        self.fuel = None
        self.fuel_type = None


class Windplant(Powerplant):
    def __init__(self, name, type, efficiency, pmin, pmax, data):
        super().__init__(name, type, efficiency, pmin, pmax)
        self.fuel_type = "wind(%)"
        self.fuel = data.get("fuels").get(self.fuel_type) / 100
        self.pmwh = self.efficiency * self.fuel


class Turbojet(Powerplant):
    def __init__(self, name, type, efficiency, pmin, pmax, data):
        super().__init__(name, type, efficiency, pmin, pmax)
        self.fuel_type = "kerosine(euro/MWh)"
        self.fuel = data.get("fuels").get(self.fuel_type)
        self.pmwh = self.pmwh = self.efficiency * self.fuel


class Gasfire(Powerplant):
    def __init__(self, name, type, efficiency, pmin, pmax, data):
        super().__init__(name, type, efficiency, pmin, pmax)
        self.fuel_type = "gas(euro/MWh)"
        self.fuel = data.get("fuels").get(self.fuel_type)
        self.pmwh = float(self.efficiency) * self.fuel

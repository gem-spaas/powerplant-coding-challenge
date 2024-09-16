TYPE_COST_MAPPING = {"gasfired": "gas(euro/MWh)", "turbojet": "kerosine(euro/MWh)"}


class PowerPlant:

    def __init__(self, values):
        for key, value in values.items():
            setattr(self, key, value)

    def get_producted_load(self, required_load):
        # To extend method, depending powerplant type
        pass


class WindPowerPlant(PowerPlant):

    def __init__(self, values):
        super().__init__(values)
        self.wind_rate = values.get("wind")

    def get_producted_load(self, required_load):
        if required_load <= 0:
            return 0
        load_to_product = required_load if self.pmax > required_load else self.pmax
        return round(load_to_product * self.wind_rate, 1)


class FossilePowerPlant(PowerPlant):

    def __init__(self, values):
        super().__init__(values)
        theorical_cost = values.get("cost")
        self.cost = round(theorical_cost * (1 / (self.efficiency)), 1)

    def get_producted_load(self, required_load):
        if required_load <= 0:
            return 0
        elif self.pmin < required_load < self.pmax:
            return required_load
        return self.pmin if required_load < self.pmin else self.pmax


def init_all_powerplant_units(powerplant_values, productivity_settings):
    powerplants = []
    breakpoint()
    for values in powerplant_values:
        type = values.get("type")
        cost_type = TYPE_COST_MAPPING.get(type, "wind")
        values.update(
            {
                "cost": productivity_settings.get(cost_type, 0.0),
                "wind": productivity_settings.get("wind(%)", 0.0) / 100,
            }
        )
        ToInstanceClass = WindPowerPlant if type == "windturbine" else FossilePowerPlant
        powerplant = ToInstanceClass(values)
        powerplants.append(powerplant)
    return powerplants


def get_merit_order_production_plan(required_load, powerplants):
    production_plan = []
    ordered_powerplants = sorted(powerplants, key=lambda powerplant: powerplant.cost)
    for powerplant in ordered_powerplants:
        producted_load = powerplant.get_producted_load(required_load)
        production_plan.append({"name": powerplant.name, "p": producted_load})
        required_load = round(required_load - producted_load, 1)
    return production_plan

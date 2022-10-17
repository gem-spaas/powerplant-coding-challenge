from decimal import Decimal


GAS = "gas"
KEROSINE = "kerosine"
WIND = "wind"

TYPE4FUEL_LABEL = {GAS: "gas(euro/MWh)", KEROSINE: "kerosine(euro/MWh)"}
WIND_POWER = "wind(%)"
FUEL4TYPE = {"gasfired": GAS, "turbojet": KEROSINE, "windturbine": WIND}


class PowerPlant:
    def __init__(self):
        self.name = ""
        self.power_supply = Decimal(0)
        self.fuel_type = GAS
        self.fuel_price = Decimal(0)
        self.efficiency = Decimal(0)
        self.pmin = Decimal(0)
        self.pmax = Decimal(0)

    def __repr__(self):
        return f"{self.pmax}-{self.pmin}-{self.power_supply}"

    @property
    def power_price(self):
        return self.fuel_price / self.efficiency


def build_powerplant(plant_data, fuels, wind_power_factor):
    powerplant = PowerPlant()
    powerplant.name = plant_data["name"]
    fuel_type = FUEL4TYPE[plant_data["type"]]
    fuel_price = fuels[fuel_type]["price"]
    powerplant.fuel_type = fuel_type
    powerplant.fuel_price = fuel_price
    if fuel_type == WIND:
        powerplant.pmin = plant_data["pmax"] * wind_power_factor
        powerplant.pmax = powerplant.pmin
    else:
        powerplant.pmin = plant_data["pmin"]
        powerplant.pmax = plant_data["pmax"]
    powerplant.efficiency = plant_data["efficiency"]
    return powerplant


def get_fuels(fuel_data):
    return {
        GAS: {"price": fuel_data[TYPE4FUEL_LABEL[GAS]]},
        KEROSINE: {"price": fuel_data[TYPE4FUEL_LABEL[KEROSINE]]},
        WIND: {"price": 0},
    }


class ProductionPlanResolver:
    def __init__(self, data):
        self.data = data
        self.load = data["load"]
        self.fuels = get_fuels(data["fuels"])
        wind_power_factor = data["fuels"][WIND_POWER] / 100
        self.power_plants = [build_powerplant(p, self.fuels, wind_power_factor) for p in data["powerplants"]]
        self.plan = self.generate_production_plan()

    def generate_production_plan(self):
        self.power_plants.sort(key=lambda x: (x.power_price, x.pmax))
        target = self.load
        plan = []
        unused_plants = []
        index = 0
        while target > 0 and index < len(self.power_plants):
            plant = self.power_plants[index]
            if target - plant.pmin < 0:  # plant too powerful to match load with current plan
                prev = plan[index - 1] if plan else None
                if prev:
                    prev_target = target + prev.power_supply
                    if prev.pmin + plant.pmin <= prev_target <= prev.pmax + plant.pmax:
                        # load in range of current plant and previous one
                        prev.power_supply = prev_target - plant.pmin  # lower previous plant power supply
                        target = plant.pmin  # match plant
                if not prev or target != plant.pmin:
                    unused_plants.append(plant)
                    continue
            power_supply = Decimal(min(plant.pmax, target))
            target -= power_supply
            plant.power_supply = power_supply
            plan.append(plant)
            index += 1
        unused_plants.extend(self.power_plants[index:])
        return plan + unused_plants

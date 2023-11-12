class InvalidPlantTypeProvided(Exception):
    pass


class ManualInterventionNeeded(Exception):
    pass


class ProductionPlanCalculator:
    def __init__(self, load: int, fuels: dict, powerplants: list):
        self.load = load
        self.fuels = fuels
        self.powerplants = powerplants

    def calculate_cost_per_MWH(self, powerplant: dict):
        if powerplant["type"] == "gasfired":
            cost_per_MWH = self.fuels["gas(euro/MWh)"] / powerplant["efficiency"]
        elif powerplant["type"] == "turbojet":
            cost_per_MWH = self.fuels["kerosine(euro/MWh)"] / powerplant["efficiency"]
        elif powerplant["type"] == "windturbine":
            cost_per_MWH = 0
        else:
            raise InvalidPlantTypeProvided

        return cost_per_MWH

    def calculate_pmax(self, powerplant: dict):
        if powerplant["type"] == "windturbine":
            pmax = powerplant["pmax"] * self.fuels['wind(%)'] / 100
        elif powerplant["type"] in ["gasfired", "turbojet"]:
            pmax = powerplant["pmax"]
        return pmax

    def get_sorted_powerplants_with_pmax_and_cost(self):
        plants = list(self.powerplants)
        for powerplant in plants:
            powerplant["cost"] = self.calculate_cost_per_MWH(powerplant)
            powerplant["pmax"] = self.calculate_pmax(powerplant)

        return sorted(plants, key=lambda x: x["cost"])

    def get_production_plan(self):
        plants = self.get_sorted_powerplants_with_pmax_and_cost()
        production_plan = []
        remaining_load = self.load
        for plant in plants:
            if remaining_load > 0:
                if remaining_load < plant["pmin"]:
                    raise ManualInterventionNeeded("Remaining load greater than pmin."
                                                   "Manual intervention required to adjust the load.")
                elif remaining_load >= plant["pmax"]:
                    power = float(round(plant["pmax"], 1))
                elif plant["pmin"] <= remaining_load < plant["pmax"]:
                    power = float(round(remaining_load, 1))
                remaining_load = remaining_load - power
                production_plan.append({"name": plant["name"], "p": power})
            else:
                power = float(0)
                production_plan.append({"name": plant["name"], "p": power})
        return production_plan
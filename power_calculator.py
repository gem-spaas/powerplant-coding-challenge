from models import FuelModel, PowerplantsModel
from decimal import Decimal


class PowerCalculator():
    load: int
    fuels: FuelModel
    powerplants: list[PowerplantsModel]
    price_costs: dict

    def __init__(self, load: int, fuels: FuelModel, powerplants: list[PowerplantsModel]) -> None:
        self.load = load
        self.fuels = fuels
        self.powerplants = powerplants
        self.price_cost = {
            'gasfired': self.fuels.gas,
            'turbojet': self.fuels.kerosine,
            'windturbine': 0,
        }
        self.sorted_powerplants()

    def sorted_powerplants(self):
        self.powerplants = sorted(
            self.powerplants,
            key=lambda powerplant: powerplant.get_cost_eficiency(
                self.price_cost.get(powerplant.type)
            )
        )

    def calc_power(self, powerplant):
        if powerplant.type == 'gasfired':
            return self.fuels.gas * powerplant.efficiency
        if powerplant.type == 'turbojet':
            return self.fuels.kerosine * powerplant.efficiency
        if powerplant.type == 'windturbine':
            return powerplant.pmax * self.fuels.wind / 100
        raise ValueError('Powerplant not identified')

    def get_powerplants_power(self):
        pending_power = Decimal(self.load)
        result = []
        for powerplant in self.powerplants:
            power = 0
            if pending_power > 0:
                ref_value = pending_power if powerplant.type != 'windturbine' else powerplant.pmin
                power = max(self.calc_power(powerplant), ref_value)
                power = min(power, powerplant.pmax)
                power = power

            result.append({
                'name': powerplant.name,
                'p': round(power, 1)
            })
            pending_power -= power

        return result

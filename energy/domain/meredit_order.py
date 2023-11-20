from energy.models.models import Payload
from energy.models.models import PowerPlant
from typing import List
import logging

logger = logging.getLogger(__name__)

class MeritOrder:
    def __init__(self, payload: Payload) -> None:
        self.payload = payload

    def type_plant(self, plantype: str):
        if plantype == "gasfired":
            return "gas"
        elif plantype == "turbojet":
            return "kerosine"
        else:
            return "wind"

    def calculate_cost(self, powerplant: PowerPlant) -> float:
        try:
            if powerplant.type == "windturbine":
                return 0
            else:
                fuel_price = getattr(
                    self.payload.fuels, self.type_plant(powerplant.type.lower())
                )
                fuel_cost = fuel_price / powerplant.efficiency
                co2_cost = 0.3 * getattr(self.payload.fuels, "co2")
                return fuel_cost + co2_cost
        except Exception as e:
            logger.error(e)

    def calculate_energy(self, powerplant: PowerPlant) -> float:
        if powerplant.type == "windturbine":
            return powerplant.pmax * (self.payload.fuels.wind / 100)
        else:
            return powerplant.pmax

    def calculate_merit_order(self) -> List:
        try:
            pwr_plants = {}
            costs = [
                (powerplant.name, self.calculate_cost(powerplant))
                for powerplant in self.payload.powerplants
            ]
            sorted_powerplants = sorted(costs, key=lambda x: x[1])

            activated_powerplants = []
            remaining_load = self.payload.load

            for name, cost in sorted_powerplants:
                powerplant = next(
                    (p for p in self.payload.powerplants if p.name == name), None
                )
                if powerplant:
                    if remaining_load <= 0:
                        activated_powerplants.append(
                            dict(pwr_plants, **{"name": name, "p": 0})
                        )
                    else:
                        energy_generated = self.calculate_energy(powerplant)
                        activated_power = {
                            "name": powerplant.name,
                            "p": min(
                                max(energy_generated, powerplant.pmin), powerplant.pmax
                            ),
                        }
                        remaining_load -= activated_power["p"]
                        activated_powerplants.append(activated_power)
        except Exception as e:
            logger.error(e)


        return activated_powerplants

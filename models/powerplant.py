from dataclasses import dataclass
from abc import ABC, abstractmethod


# Constants
CO2_EMMISION_FACTOR = 0.3


@dataclass
class PowerPlant(ABC):
    name: str
    type: str
    efficiency: float
    pmin: int
    pmax: int

    @property
    @abstractmethod
    def power_cost(self):
        pass


class Windturbine(PowerPlant):
    @property
    def power_cost(self):
        return 0


@dataclass
class Gasfired(PowerPlant):
    # gas(euro/MWh) / efficiency x (0.3 * co2(euro/ton))
    gas_price: float
    co2_price: float

    @property
    def power_cost(self):
        return self.gas_price / self.efficiency * (CO2_EMMISION_FACTOR * self.co2_price)


@dataclass
class Turbojet(PowerPlant):
    # kerosine(euro/MWh) / efficiency
    kerosine_price: float

    @property
    def power_cost(self):
        return self.kerosine_price / self.efficiency

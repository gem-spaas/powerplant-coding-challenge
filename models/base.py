from abc import ABC


class BasePowerPlant(ABC):
    _name: str
    _pmin: float
    _pmax: float
    _efficiency: float
    _price_per_MWh: float
    _cost: float
    _energy_produced_per_MWh: float
    _generated_power_in_MWh: float

    def __init__(
        self,
        name: str,
        pmin: float,
        pmax: float,
        price_per_MWh: float,
        efficiency: float,
    ):
        self._name = name
        self._pmin = pmin
        self._pmax = pmax
        self._price_per_MWh = price_per_MWh
        self._efficiency = efficiency
        self._generated_power_in_MWh = 0
        self._cost = self.calculate_cost()

    def generate_power(self, load: float):
        if load < self._pmin:
            self._generated_power_in_MWh = round(self._pmin, 1)
        self._generated_power_in_MWh = round(min(self._pmax, load), 1)

    def calculate_cost(self):
        return self._price_per_MWh / self._efficiency

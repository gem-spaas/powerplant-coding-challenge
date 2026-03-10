from dataclasses import dataclass


@dataclass
class PowerPlantProfile:
    name: str
    cost_per_MWh: float
    pmin: float
    pmax: float


@dataclass
class DispatchEntry:
    profile: PowerPlantProfile
    production: float


class WindTurbineProfile(PowerPlantProfile):
    def __init__(self, name: str, pmin: float, pmax: float, wind_percentage: float):
        adjusted_pmax = pmax * (wind_percentage / 100)

        super().__init__(name=name, cost_per_MWh=0, pmin=pmin, pmax=adjusted_pmax)


class GasFiredProfile(PowerPlantProfile):
    CO2_PER_MWH = 0.3

    def __init__(self, name: str, pmin: float, pmax: float, gas_price: float, efficiency: float, co2_price: float):
        cost = (gas_price / efficiency) + (co2_price * self.CO2_PER_MWH)

        super().__init__(name=name, cost_per_MWh=cost, pmin=pmin, pmax=pmax)


class TurbojetProfile(PowerPlantProfile):
    def __init__(self, name: str, pmin: float, pmax: float, kerosine_price: float, efficiency: float):
        cost = kerosine_price / efficiency

        super().__init__(name=name, cost_per_MWh=cost, pmin=pmin, pmax=pmax)

from dataclasses import dataclass

@dataclass
class Fuels:
    gas: int
    kerosine: int
    co2: int
    wind: int

@dataclass
class Powerplant:
    pid: int # Not Used Here but might be use for Slack Bus when Loss = Load + cable length ( etc...)
    name: str
    type: str
    efficiency: float
    pmin: int
    pmax: int
    injection: float = 0
    price: float = 0

    def get_cost_per_mwh(self, fuels: Fuels, mw_co2_em:int=0) -> float:
        """Calculate cost / MWh based on fuel type and efficiency"""

        costs = {
            'gasfired':    fuels.gas / self.efficiency,
            'turbojet':    fuels.kerosine / self.efficiency,
            'windturbine': 0
            }

        if mw_co2_em:
            co2_costs = {
                'gasfired':    mw_co2_em * fuels.co2,
                'turbojet':    mw_co2_em * fuels.co2,
                'windturbine': 0
            }
            return costs[self.type] + co2_costs[self.type]
        return costs[self.type]


    def get_available_power(self, fuels: Fuels) -> tuple[float, float]:
        if self.type == 'windturbine':
            max_power = self.pmax * (fuels.wind / 100.0)
            return 0, max_power
        return self.pmin, self.pmax



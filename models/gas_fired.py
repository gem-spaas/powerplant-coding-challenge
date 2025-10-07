from models.base import BasePowerPlant


class GasFiredPowerPlant(BasePowerPlant):
    CO2_EMISSION_COST: float = 0.3
    _co2_price_per_ton: float

    def __init__(self, co2_price_per_ton: float, **kwargs):
        self._co2_price_per_ton = co2_price_per_ton
        super().__init__(**kwargs)

    def calculate_cost(self) -> float:
        return super().calculate_cost() + (
            self.CO2_EMISSION_COST * self._co2_price_per_ton
        )

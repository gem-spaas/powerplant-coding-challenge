from models.base import BasePowerPlant


class WindTurbinePowerPlant(BasePowerPlant):
    _wind_percent: int

    def __init__(self, wind_percent: int, **kwargs):
        self._wind_percent = wind_percent
        super().__init__(**kwargs)

    def calculate_cost(self) -> float:
        return super().calculate_cost() * (self._wind_percent / 100)

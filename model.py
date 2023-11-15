from dataclasses import dataclass


@dataclass
class PowerPlant:
    name: str
    type: str
    efficiency: float
    pmin: int
    pmax: int

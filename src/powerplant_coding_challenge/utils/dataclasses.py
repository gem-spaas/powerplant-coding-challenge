from dataclasses import dataclass


@dataclass(frozen=True)
class Fuels:
    gas: float
    kerosine: float
    co2: float
    wind: float


@dataclass(frozen=True)
class Powerplant:
    name: str
    type: str
    efficiency: float
    pmin: float
    pmax: float
    cost: float = 0
    min_cost: float = 0
    consumption: float = 0


@dataclass(frozen=True)
class Payload:
    load: int
    fuels: Fuels
    powerplants: list

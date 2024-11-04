import logging
from abc import ABC, abstractmethod
from typing import Tuple

from config.constants import BS_LOGGER
from models.linear_programming import LinearProgrammingProblem
from models.energy_demand import EnergyDemand


class LinearProgrammingProblemBuilder(ABC):
    def __init__(self):
        self._logger = logging.getLogger(BS_LOGGER)

    @abstractmethod
    def from_energy_demand(self, demand: EnergyDemand) -> Tuple[list[str], LinearProgrammingProblem]:
        """Gets a linear programming problem, given an energy demand.

        Parameters:
        demand (EnergyDemand): energy demand.

        Returns:
        The linear programming problem corresponding to the demand

        """            
        raise NotImplementedError("Subclasses should implement this!")
import logging
from abc import ABC, abstractmethod

from config.constants import BS_LOGGER
from models.linear_programming import ProblemResult
from models.production_plan import ProductionPlanItem


class ProductionPlanBuilder(ABC):
    def __init__(self):
        self._logger = logging.getLogger(BS_LOGGER)

    @abstractmethod
    def from_problem_result(self, powerplant_names: list[str], problem_result: ProblemResult) -> list[ProductionPlanItem]:
        """Gets a production plan, given a linear programming problem result.

        Parameters:
        powerplant_names: array of powerplant names
        problem_result (ProblemResult): linear programming problem result.

        Returns:
        The production plan corresponding to the linear programming problem result

        """        
        raise NotImplementedError("Subclasses should implement this!")

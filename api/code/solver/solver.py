import logging
from abc import ABC, abstractmethod

from config.constants import BS_LOGGER
from models.linear_programming import LinearProgrammingProblem, ProblemResult


class Solver(ABC):
    def __init__(self): 
        self._logger = logging.getLogger(BS_LOGGER)

    @abstractmethod
    def solve(self, problem: LinearProgrammingProblem) -> ProblemResult :
        """Solves the linear programming problem given as parameter.

        Parameters:
        problem (LinearProgrammingProblem): linear programming problem to solve.

        Returns:
        The solution of de linear programming problem given as argument

        """         
        raise NotImplementedError("Subclasses should implement this!")
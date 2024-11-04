import logging

from config.constants import BS_LOGGER
from solver.solver import Solver
from models.linear_programming import LinearProgrammingProblem, ProblemResult
from exceptions.exceptions import SolverException

from scipy.optimize import linprog 

class SolverImpl(Solver):
    def __init__(self):
        super().__init__()
        self._logger = logging.getLogger(BS_LOGGER)

    def solve(self, problem: LinearProgrammingProblem) -> ProblemResult :
        self._logger.debug(f"Objective function coeficients: {problem.objective_function}")
        self._logger.debug(f"Constraint matrix: {problem.constraint_matrix}")
        self._logger.debug(f"Constraint vector: {problem.constraint_vector}")
        self._logger.debug(f"Bounds: {problem.bounds}")
        solution = linprog(c=problem.objective_function, 
                          A_eq=problem.constraint_matrix, 
                          b_eq=problem.constraint_vector, 
                          bounds=problem.bounds, 
                          method='highs-ds')
        self._logger.debug(solution.message)
        result = ProblemResult(solution=solution.x, opt=solution.fun, status=solution.status)
        if result.status != 0:
            raise SolverException(result.status)
        return result
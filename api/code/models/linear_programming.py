"""
    This class provide the abstraction of the LinearProgrammingProblem and ProblemResult data structures.



"""


import logging
from abc import ABC, abstractmethod

from config.constants import BS_LOGGER

class LinearProgrammingProblem():
    def __init__(self, objective_function=None, constraint_matrix=None, constraint_vector=None, bounds=None):
        """Linear programming problem (simplyfied) data sructure.

        Parameters
            ----------
            objective_function : 1-D array
                The coefficients of the linear objective function to be minimized.
            constraint_matrix : 2-D array, optional
                The equality constraint matrix. Each row of ``constraint_matrix`` specifies the
                coefficients of a linear equality constraint on ``x``.
            constraint_vector : 1-D array, optional
                The equality constraint vector. Each element of ``constraint_matrix @ x`` must equal
                the corresponding element of ``constraint_vector``.
            bounds : sequence, optional
                A sequence of ``(min, max)`` pairs for each element in ``x``, defining
                the minimum and maximum values of that decision variable. Use ``None``
                to indicate that there is no bound. By default, bounds are
                ``(0, None)`` (all decision variables are non-negative).
        """    
        self._logger = logging.getLogger(BS_LOGGER)
        self.objective_function = objective_function
        self.constraint_matrix = constraint_matrix
        self.constraint_vector = constraint_vector
        self.bounds = bounds

class ProblemResult():
    def __init__(self, solution, opt, status):          
        """Linear programming problem result (simplyfied) data sructure.

        Parameters
        ----------
        solution : 1-D array
            The values of the decision variables that minimizes the
            objective function while satisfying the constraints.
        opt : float
            The optimal value of the objective function ``c @ x``.
        status : int
            An integer representing the exit status of the solving operation.
            ``0`` : Terminated successfully.
            ``-1`` : Terminated unsuccessfully.            
        """          
        self._logger = logging.getLogger(BS_LOGGER)
        self.solution = solution
        self.opt = opt
        self.status = status

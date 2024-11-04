import logging
from abc import ABC, abstractmethod
from typing import Tuple

from config.constants import BS_LOGGER
from models.linear_programming import LinearProgrammingProblem
from models.energy_demand import EnergyDemand
from exceptions.exceptions import BuilderException

class LinearProgrammingProblemBuilderImpl(ABC):
    def __init__(self):
        self._logger = logging.getLogger(BS_LOGGER)

    def from_energy_demand(self, demand: EnergyDemand) -> Tuple[list[str], LinearProgrammingProblem]:

        try:
            load = demand.load
            fuels = demand.fuels
            powerplant_list = demand.powerplants

            constraint_vector = [load]

            fuels_dict = {
                "gasfired": fuels.gas,
                "turbojet": fuels.kerosine,
                "windturbine": 0
            }

            powerplant_names = []
            objective_function = []
            constraint_matrix = []
            constraint_matrix_first_row = []
            bounds = []

            for powerplant in powerplant_list:
                powerplant_names.append(powerplant.name)
                objective_function.append(fuels_dict[powerplant.type])
                if (powerplant.type == "windturbine"):
                    constraint_matrix_first_row.append(fuels.wind / 100)
                else:
                    constraint_matrix_first_row.append(powerplant.efficiency)
                powerplant_bounds = (powerplant.pmin, powerplant.pmax)
                bounds.append(powerplant_bounds)
            
            constraint_matrix = [constraint_matrix_first_row]

            lp_problem = LinearProgrammingProblem(objective_function=objective_function,
                                                constraint_matrix = constraint_matrix,
                                                constraint_vector = constraint_vector,
                                                bounds=bounds)
        except Exception as e:
            raise BuilderException(arg=e)

        return powerplant_names, lp_problem
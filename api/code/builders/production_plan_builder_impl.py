import logging

from config.constants import BS_LOGGER
from planner.planner import Planner
from solver.solver import Solver
from models.energy_demand import EnergyDemand
from models.production_plan import ProductionPlanItem
from models.linear_programming import LinearProgrammingProblem, ProblemResult

from builders.production_plan_builder import ProductionPlanBuilder


class ProductionPlanBuilderImpl(ProductionPlanBuilder):
    def __init__(self):
        super().__init__()
        self._logger = logging.getLogger(BS_LOGGER)

    def from_problem_result(self, powerplant_names: list[str], problem_result: ProblemResult) -> list[ProductionPlanItem]:
        result_matrix = problem_result.solution
        production_plan = []
        for n in range(len(result_matrix)):
            production_plan_item = ProductionPlanItem(name=powerplant_names[n],p=result_matrix[n])
            production_plan.append(production_plan_item)
        return production_plan
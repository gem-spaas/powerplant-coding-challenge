import logging
from abc import ABC, abstractmethod

from config.constants import BS_LOGGER
from planner.planner import Planner
from planner.planner_factory import PlannerFactory
from planner.planner_impl import PlannerImpl
from solver.solver_impl import SolverImpl
from builders.production_plan_builder_impl import ProductionPlanBuilderImpl
from builders.lp_problem_builder_impl import LinearProgrammingProblemBuilderImpl

class PlannerFactoryImpl(PlannerFactory):
    def __init__(self):
        super().__init__()        
        self._logger = logging.getLogger(BS_LOGGER)

    def create_planner(self) -> Planner:
        solver = SolverImpl()
        lp_problem_builder = LinearProgrammingProblemBuilderImpl()
        production_plan_builder = ProductionPlanBuilderImpl()
        planner = PlannerImpl(solver=solver, 
                              lp_problem_builder=lp_problem_builder, 
                              production_plan_builder=production_plan_builder)
        return planner
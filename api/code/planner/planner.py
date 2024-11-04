"""
    This class provide the abstraction of class with the responsability of generating a valid
    ProductionPlan given an EnergyDemand.



"""


import logging
from abc import ABC, abstractmethod

from config.constants import BS_LOGGER
from models.energy_demand import EnergyDemand
from models.production_plan import ProductionPlanItem


class Planner(ABC):
    def __init__(self):
        self._logger = logging.getLogger(BS_LOGGER)

    @abstractmethod
    def production_plan(self, demand: EnergyDemand) -> list[ProductionPlanItem] :
        """Generates a valid ProductionPlan given an EnergyDemand.

        Parameters:
        demand (EnergyDemand): energy demand.

        Returns:
        The a production plan according with the demand

        """                   
        raise NotImplementedError("Subclasses should implement this!")

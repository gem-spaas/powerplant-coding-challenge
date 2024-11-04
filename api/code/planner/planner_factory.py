import logging
from abc import ABC, abstractmethod

from config.constants import BS_LOGGER
from planner.planner import Planner

class PlannerFactory(ABC):
    def __init__(self):
        self._logger = logging.getLogger(BS_LOGGER)

    @abstractmethod
    def create_planner(self) -> Planner:
        """Abstract"""
        raise NotImplementedError("Subclasses should implement this!")
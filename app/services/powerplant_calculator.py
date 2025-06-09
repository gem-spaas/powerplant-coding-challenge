# Third-party imports
from typing import List, Dict
import logging

# Local application imports
from app.models.powerplant import ProductionPlanRequest, PowerPlant, PlantTypes
from app.services.powerplant_cost import calculate_powerplant_cost

# Logger setup
logger = logging.getLogger(__name__)


class ProductionPlanCalculator:
    """
    Core class to calculate the production plan using the merit order strategy.
    """

    def __init__(self) -> None:
        # Stores list of PowerPlant instances
        self.plants: List[PowerPlant] = []

    def generate_optimized_plan(self, payload: ProductionPlanRequest) -> List[Dict[str, float]]:
        """
        Process input payload and return an optimized production plan.

        Args:
            payload (ProductionPlanRequest): Input request containing load, fuels, and power plants.

        Returns:
            List[Dict[str, float]]: List of dicts with plant name and assigned production.

        Raises:
            Exception: Propagates any exception encountered during processing.
        """
        try:
            load = payload.load
            fuels = payload.fuels
            powerplants_data = payload.powerplants

            self.plants = []
            for plant_data in powerplants_data:
                # Instantiate PowerPlant and calculate cost
                plant = PowerPlant(
                    plant_data.name,
                    plant_data.type,
                    plant_data.efficiency,
                    plant_data.pmin,
                    plant_data.pmax,
                )
                calculate_powerplant_cost(
                    plant,
                    fuels.gas,
                    fuels.kerosine,
                    fuels.co2,
                    fuels.wind,
                )
                self.plants.append(plant)

            # Sort plants by cost (ascending) and available power (descending)
            self.plants.sort(key=lambda p: (p.cost_per_mwh, -p.available_capacity))
            logger.info(f"Merit order: {[f'{p.name}({p.cost_per_mwh:.2f})' for p in self.plants]}")

            plan = self._allocate_production_capacity(load)

            logger.info(f"Final production plan: {plan}")
            return plan

        except Exception as e:
            logger.error(f"Error calculating production plan: {e}")
            raise

    def _allocate_production_capacity(self, target_load: float) -> List[Dict[str, float]]:
        """
        Optimize production allocation among power plants to meet the target load.

        Args:
            target_load (float): The required total production load.

        Returns:
            List[Dict[str, float]]: Production plan with allocated power per plant.
        """
        production_plan = [{"name": plant.name, "p": 0.0} for plant in self.plants]
        remaining_load = target_load

        # Assign wind power first as it is usually cost-free and limited by availability
        remaining_load = self._allocate_wind_power_capacity(production_plan, remaining_load)

        # Then assign power from conventional plants using merit order
        remaining_load = self._allocate_conventional_power_capacity(production_plan, remaining_load)

        # Adjust minor differences to meet exact load within tolerance
        self._fine_tune_production_allocation(production_plan, target_load)

        return production_plan

    def _allocate_wind_power_capacity(
        self, production_plan: List[Dict[str, float]], remaining_load: float
    ) -> float:
        """
        Assign production from wind turbines up to their available capacity.

        Args:
            production_plan (List[Dict[str, float]]): Current production allocations.
            remaining_load (float): Load left to be allocated.

        Returns:
            float: Updated remaining load after wind power assignment.
        """
        for i, plant in enumerate(self.plants):
            if plant.type == PlantTypes.WINDTURBINE and remaining_load > 0:
                production = min(plant.available_capacity, remaining_load)
                production_plan[i]["p"] = production
                remaining_load -= production
        return remaining_load

    def _allocate_conventional_power_capacity(
        self, production_plan: List[Dict[str, float]], remaining_load: float
    ) -> float:
        """
        Assign production from conventional plants based on merit order and load requirements.

        Args:
            production_plan (List[Dict[str, float]]): Current production allocations.
            remaining_load (float): Load left to be allocated.

        Returns:
            float: Updated remaining load after conventional power assignment.
        """
        for i, plant in enumerate(self.plants):
            if plant.type == PlantTypes.WINDTURBINE:
                continue
            if remaining_load <= 0:
                break

            prev_prod = production_plan[i]["p"]
            min_prod = plant.pmin if prev_prod == 0 else 0
            max_prod = plant.available_capacity

            if remaining_load < min_prod and prev_prod == 0:
                continue

            possible_prod = min(max_prod, remaining_load + prev_prod)
            
            production = max(min_prod, prev_prod)
            production = min(production, possible_prod)

            extra_needed = remaining_load - (production - prev_prod)
            if extra_needed > 0:
                production = min(production + extra_needed, possible_prod)
            
            production_plan[i]["p"] = production
            remaining_load -= (production - prev_prod)

        return remaining_load

    def _fine_tune_production_allocation(
        self, production_plan: List[Dict[str, float]], target_load: float
    ) -> None:
        """
        Adjust minor differences in production plan to exactly meet the target load.

        Args:
            production_plan (List[Dict[str, float]]): Current production allocations.
            target_load (float): Required total production load.
        """
        total = sum(p["p"] for p in production_plan)
        diff = target_load - total

        if abs(diff) > 0.1:
            for i in reversed(range(len(production_plan))):
                plant = self.plants[i]
                new_p = production_plan[i]["p"] + diff
                if plant.pmin <= new_p <= plant.pmax:
                    production_plan[i]["p"] = new_p
                    break

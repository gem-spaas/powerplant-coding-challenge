from typing import List

from app.data.power_plant_profiles import (
    DispatchEntry,
    GasFiredProfile,
    PowerPlantProfile,
    TurbojetProfile,
    WindTurbineProfile,
)
from app.data.request import Fuels, PowerPlant
from app.data.response import ProductionPlanElement


class ProductionPlanner:
    """
    This class is responsible for planning the production of power.
    """

    def create_plan(self, load: float, fuels: Fuels, powerplants: List[PowerPlant]) -> List[ProductionPlanElement]:
        """
        Compute a power production plan that satisfies the required load using a
        merit-order dispatch algorithm.

        Powerplants are first ranked by their marginal production cost (derived from
        fuel price and plant efficiency). The algorithm then allocates generation
        starting from the cheapest units while respecting operational constraints:
        each plant's pmin and pmax limits. Wind turbines are treated as zero-cost
        producers with output limited by the available wind percentage.

        The resulting plan ensures that total generated power matches the load while
        minimizing generation cost.


        Args:
            load (float): Load
            fuels (Fuels): Fuels
            powerplants (List[PowerPlant]): Powerplants

        Returns:
            ProductionPlan
        """
        # Create profiles
        profiles = [self._create_profile(plant, fuels) for plant in powerplants]

        # Order profiles by cost per MWh (merit order)
        ordered_profiles = sorted(profiles, key=lambda x: x.cost_per_MWh)

        # Calculate production plan
        return self._calculate_plan(load, ordered_profiles)

    def _rebalance_prev_production(self, dispatch: List[DispatchEntry], excess: float) -> bool:
        """
        Rebalance production of previous production plan.

        Args:
            dispatch (List[DispatchEntry]): Dispatch
            excess (float): Excess

        Returns:
            bool
        """
        for entry in reversed(dispatch):
            reducible = entry.production - entry.profile.pmin

            if reducible <= 0:
                continue

            reduction = min(reducible, excess)
            entry.production -= reduction
            excess -= reduction

            if excess <= 0:
                return True

        return False

    def _calculate_plan(self, load: float, ordered_profiles: List[PowerPlantProfile]) -> List[ProductionPlanElement]:
        """
        Calculate production plan based on load and ordered profiles.

        Args:
            load (float): Load
            ordered_profiles (List[PowerPlantProfile]): Ordered profiles

        Returns:
            List[ProductionPlanElement]
        """
        remaining_load = load
        dispatch: List[DispatchEntry] = []

        for profile in ordered_profiles:
            if remaining_load <= 0:
                dispatch.append(DispatchEntry(profile, 0))
                continue

            prod_candidate = min(profile.pmax, remaining_load)

            if prod_candidate >= profile.pmin:
                dispatch.append(DispatchEntry(profile, prod_candidate))
                remaining_load -= prod_candidate
                continue

            if 0 < remaining_load < profile.pmin:
                excess = profile.pmin - remaining_load

                if self._rebalance_prev_production(dispatch, excess):
                    dispatch.append(DispatchEntry(profile, profile.pmin))
                    remaining_load = 0
                else:
                    dispatch.append(DispatchEntry(profile, 0))
                continue

            dispatch.append(DispatchEntry(profile, 0))

        if remaining_load > 0:
            raise ValueError("Unable to satisfy load with given powerplants")

        return [
            ProductionPlanElement(
                name=entry.profile.name,
                p=round(entry.production, 1),
            )
            for entry in dispatch
        ]

    def _create_profile(self, plant: PowerPlant, fuels: Fuels) -> PowerPlantProfile:
        """
        Create a profile for a power plant based on its type and fuels.

        Args:
            plant (PowerPlant): Power plant
            fuels (Fuels): Fuels

        Returns:
            PowerPlantProfile
        """
        if plant.type == "gasfired":
            return GasFiredProfile(
                name=plant.name,
                pmin=plant.pmin,
                pmax=plant.pmax,
                gas_price=fuels.gas,
                efficiency=plant.efficiency,
                co2_price=fuels.co2,
            )
        elif plant.type == "turbojet":
            return TurbojetProfile(
                name=plant.name,
                pmin=plant.pmin,
                pmax=plant.pmax,
                kerosine_price=fuels.kerosine,
                efficiency=plant.efficiency,
            )
        elif plant.type == "windturbine":
            return WindTurbineProfile(name=plant.name, pmin=plant.pmin, pmax=plant.pmax, wind_percentage=fuels.wind)
        else:
            raise ValueError(f"Unknown power plant type: {plant.type}")

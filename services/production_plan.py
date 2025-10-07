from factories.powerplant import powerplant_factory
from models.gas_fired import GasFiredPowerPlant
from models.turbojet import TurboJetPowerPlant
from models.wind_turbine import WindTurbinePowerPlant
from schemas.production_plan import ProductionPlanRequest

# For type hints
PowerplantFactories = GasFiredPowerPlant | TurboJetPowerPlant | WindTurbinePowerPlant


class ProductionPlanService:

    @staticmethod
    def calculate_production_plan(
        production_plan_request: ProductionPlanRequest,
    ) -> list[PowerplantFactories]:
        # Create appropriate powerplant objects
        powerplant_objs: list[PowerplantFactories] = []
        for powerplant in production_plan_request.powerplants:
            powerplant_obj = powerplant_factory(
                powerplant, fuels=production_plan_request.fuels
            )
            powerplant_objs.append(powerplant_obj)

        # Step 1: Sort the powerplants based on cost asc, Pmax desc and Pmin asc (most efficient to least)
        sorted_powerplants = sorted(
            powerplant_objs, key=lambda p: (p._cost, -p._pmax, p._pmin)
        )

        # Step 2: Produce the maximum possible load in each powerplant
        remaining_load = production_plan_request.load
        for powerplant in sorted_powerplants:
            if remaining_load <= 0:
                break
            powerplant.generate_power(load=remaining_load)
            remaining_load -= powerplant._generated_power_in_MWh

        return sorted_powerplants

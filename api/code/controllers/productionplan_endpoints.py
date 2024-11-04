"""
    This module defines the main POST endpoint for the Powerplan Coding Challenge API.
    It includes functions to handle the primary data submission operation.

    Endpoints:
    - POST /productionplan: Handle the productionplan request according with the energy demand provided


"""
import logging

from fastapi import HTTPException, APIRouter, Depends

from config.constants import BS_CORRELATION_LOGGER
from config.traceability import generate_correlation_id
from models.energy_demand import EnergyDemand
from planner.planner import Planner
from planner.planner_factory_impl import PlannerFactoryImpl
from exceptions.exceptions import PlannerException

productionplan_router = APIRouter()

planner_factory = PlannerFactoryImpl()
planner = planner_factory.create_planner()

# Dependency injection with FastAPI using Depends
def get_planner() -> Planner:
    return planner

@productionplan_router.post("/productionplan", status_code=200)
def post_production_plan(energy_demand: EnergyDemand, planner = Depends(get_planner)):
    """
    This code defines a FastAPI endpoint that returns a ProductionPlan 
    according to the EnergyDemand.

    :param payload:
    :return:
    """

    try:
        correlation_id = generate_correlation_id()

        logging.getLogger(BS_CORRELATION_LOGGER).info(
            f"Processing event with correlation ID {correlation_id}"
        )

        # Create response
        production_plan = planner.production_plan(energy_demand)
        return production_plan

    except PlannerException as pe:
        logging.getLogger(BS_CORRELATION_LOGGER).error(f"Planner Exception {pe}")
        raise HTTPException(
            status_code=400,
            detail=f"BadRequest. {pe}",
        )

    except TypeError as te:
        logging.getLogger(BS_CORRELATION_LOGGER).error(f"TypeError {te}")
        raise HTTPException(
            status_code=400,
            detail=f"BadRequest Typerror!! {te}",
        )

    except Exception as e:
        logging.getLogger(BS_CORRELATION_LOGGER).error(f"Unexpected error: {e}")
        raise HTTPException(status_code=500, detail="Internal Server Error")

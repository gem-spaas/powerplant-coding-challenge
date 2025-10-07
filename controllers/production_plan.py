import logging

from fastapi import APIRouter

from schemas.production_plan import (
    ProductionPlanResponse,
    ProductionPlanRequest,
    ProductionPlanResponseItem,
)
from services.production_plan import ProductionPlanService

router = APIRouter(prefix="/productionplan", tags=["productionplan"])


@router.post("", response_model=ProductionPlanResponse)
async def get_production_plan(
    production_plan_request: ProductionPlanRequest,
) -> ProductionPlanResponse:
    production_plan_service = ProductionPlanService()
    logging.info("Started calculating production plan")
    powerplant_factories = production_plan_service.calculate_production_plan(
        production_plan_request
    )
    response = [
        ProductionPlanResponseItem(name=p._name, p=p._generated_power_in_MWh)
        for p in powerplant_factories
    ]
    logging.info("End")
    return response

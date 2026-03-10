import logging

from fastapi import APIRouter, HTTPException

from app.data.request import ProductionPlanRequest
from app.data.response import ProductionPlanResponse
from app.services.production_planner import ProductionPlanner

logger = logging.getLogger(__name__)
router = APIRouter()
production_planner = ProductionPlanner()


@router.get("/")
async def root():
    return {"message": "Welcome to the Powerplan Planner API"}


@router.get("/healthcheck")
async def healthcheck():
    return {"status": "ok"}


@router.post("/productionplan", response_model=ProductionPlanResponse)
async def production_plan(payload: ProductionPlanRequest):
    try:
        return production_planner.create_plan(payload.load, payload.fuels, payload.powerplants)

    except ValueError as e:
        logger.exception("Bad request")
        return HTTPException(status_code=400, detail=str(e))

    except Exception:
        logger.exception("Internal error while creating production plan")
        return HTTPException(status_code=500, detail="Internal error while creating production plan")

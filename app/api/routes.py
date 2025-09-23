from fastapi import APIRouter, HTTPException

from app.schemas.payload import ProductionPlanRequest
from app.schemas.response import ProductionPlanResponseItem
from app.services.production_planner import plan_production

router = APIRouter()


@router.post("/productionplan", response_model=list[ProductionPlanResponseItem])
async def production_plan(payload: ProductionPlanRequest):
    try:
        plan = plan_production(payload)
    except ValueError as e:
        raise HTTPException(status_code=422, detail=str(e))
    return [ProductionPlanResponseItem(name=p.name, p=p.p) for p in plan]

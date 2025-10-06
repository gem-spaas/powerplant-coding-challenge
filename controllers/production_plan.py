from fastapi import APIRouter

from schemas.production_plan import ProductionPlanResponse, ProductionPlanRequest

router = APIRouter(prefix="/productionplan", tags=["productionplan"])


@router.get("/", response_model=list[ProductionPlanResponse])
async def get_production_plan(production_plan_request: ProductionPlanRequest):
    production_plan_service = ProductionPlanService()
    return production_plan_service.get_all()
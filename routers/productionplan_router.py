import logging
import uuid
from typing import List

from fastapi import APIRouter, HTTPException

from models import ProductionPlanInput, ProductionPlan
from services import generate_productionplan

logger = logging.getLogger(__name__)


# This is where I would put url prefix and version, but it is not part of the challenge
productionplan_router = APIRouter()


@productionplan_router.post(
    '/productionplan',
    responses={
        404: {"desription": "Error Id"}
    }
)
def get_production_plan(plan_input: ProductionPlanInput) -> List[ProductionPlan]:
    try:
        return (plan for plan in generate_productionplan(plan_input))
    except Exception:
        error_id = uuid.uuid4()
        logger.exception(f"Unhandled error happened, Error id: {error_id}")
        raise HTTPException(status_code=404, detail=f"Error happened, please check the logs, search for Error id:{error_id}")

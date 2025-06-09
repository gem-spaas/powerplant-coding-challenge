# Third-party imports
from fastapi import APIRouter, HTTPException
from typing import List
import logging

# Local application imports
from app.services.powerplant_calculator import ProductionPlanCalculator
from app.models.powerplant import ProductionPlanRequest, ProductionPlanResponse

# Set up logging for this module
logger = logging.getLogger(__name__)

# Create API router with specific prefix and metadata
router = APIRouter(
    prefix="/production_plan",
    tags=["production_plan"],
    responses={404: {"message": "Not Found"}},
)

# Instantiate the production plan calculator service
calculator = ProductionPlanCalculator()

# Endpoint to calculate the production plan
@router.post(
    "",
    response_model=List[ProductionPlanResponse],
    summary="Calculate production plan",
    description="Calculates the electricity production plan using the merit order method.",
)
async def production_plan(payload: ProductionPlanRequest):
    """
    Receives production plan request and returns the computed result using the
    merit order algorithm.
    """
    try:
        logger.info(f"Received request for load: {payload.load} MW")

        # Calculate production plan
        plan = calculator.generate_optimized_plan(payload)

        # Log total production for traceability
        logger.info(f"Total production: {sum(p['p'] for p in plan)} MW")

        return plan

    except Exception as e:
        logger.error(f"Error processing request: {str(e)}")
        raise HTTPException(status_code=500, detail="Internal server error")

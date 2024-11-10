from fastapi import FastAPI, HTTPException
from pydantic import BaseModel
from core.utils import format_payload, logger
from core.func import solve
from typing import Dict, Any
import os

# https://fastapi.tiangolo.com/#recap
productionApp = FastAPI()


class ProductionPlanRequest(BaseModel):
    load: float
    fuels: Dict[str, Any]
    powerplants: list[dict]


@productionApp.get("/")
async def default():
    endpoint = os.getenv('API_ENDPOINT', 'productionplan')
    logger.info("Root endpoint accessed")
    return {'message': f'do post request to /{endpoint}?with_co2=1 to include CO2 costs'}


@productionApp.post("/{endpoint}")
async def get_productionplan(endpoint: str, payload: ProductionPlanRequest, with_co2: bool = False):
    """Calculate production plan with optional CO2 cost consideration."""
    logger.info(f"Production plan request received for endpoint: {endpoint}")
    logger.debug(f"CO2 calculation: {'enabled' if with_co2 else 'disabled'}")

    if endpoint != os.getenv('API_ENDPOINT', 'productionplan'):
        logger.warning(f"Invalid endpoint requested: {endpoint}")
        raise HTTPException(status_code = 404, detail = "Endpoint not found")

    try:
        load, fuels_stat, powerplants_list = format_payload(payload)
        powerplants_list, total_cost = solve(powerplants_list = powerplants_list, load = load, fuels = fuels_stat, with_co2 = with_co2)

        formated_pwplants_dict = {pw.name: pw.injection for pw in powerplants_list}

        if not with_co2:
            # Asked Project Answer
            result = formated_pwplants_dict
        else:
            # Custom Answer no specification for Answer in the project README
            formated_pwplants_dict['cost_in_euro'] = round(total_cost, 1)
            result = formated_pwplants_dict

        logger.info("Successfully calculated production plan")
        logger.debug(f"Result: {formated_pwplants_dict}")
        return result
    except Exception as e:
        logger.error(f"Error processing request: {str(e)}")
        raise HTTPException(status_code = 500, detail = str(e))


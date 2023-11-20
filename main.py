import uvicorn
from fastapi import FastAPI
from typing import List
from energy.application.get import get_max_supply_by_plant
from energy.models.models import Payload

app = FastAPI()


@app.post("/productionplan/", status_code=201, response_model=List)
async def production_plan(payload: Payload):
    pwrplants = await get_max_supply_by_plant(payload=payload)
    return pwrplants


if __name__ == "__main__":
    uvicorn.run("main:app", host="0.0.0.0", port=8888)

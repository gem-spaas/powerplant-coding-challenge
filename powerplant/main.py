import typing

from fastapi import FastAPI

from powerplant.models import PlantLoadResponse, ProductionPlanRequest

app = FastAPI()


@app.post("/productionplan", response_model=typing.List[PlantLoadResponse])
async def root(request: ProductionPlanRequest) -> typing.List[PlantLoadResponse]:
    return request.optimize()

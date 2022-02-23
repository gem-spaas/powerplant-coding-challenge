import typing

from fastapi import FastAPI

from powerplant.models import PlantLoadResult, ProductionPlan

app = FastAPI()


@app.post("/productionplan", response_model=typing.List[PlantLoadResult])
async def root(request: ProductionPlan) -> typing.List[PlantLoadResult]:
    return request.optimize()

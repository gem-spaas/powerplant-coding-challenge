from fastapi import FastAPI
from controllers import production_plan
from settings import settings

description = """
## Production Plan
    Amount of electricity produced by given powerplants.
"""

app = FastAPI(
    title="powerplant-coding-challenge",
    description=description,
    summary="This API allows getting how much load given powerplants should generate based on key parameters.",
    version=str(settings.API_VERSION),
)

app.include_router(production_plan.router)


@app.get("/")
async def healthcheck():
    return {"message": "API is running!"}

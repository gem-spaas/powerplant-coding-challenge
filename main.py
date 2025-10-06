from fastapi import FastAPI
from controllers import cours, inscription, note
from settings import settings

description = """
## Production Plan
    Amount of electricity produced by given powerplants.
"""

app = FastAPI(
    title="powerplant-coding-challenge",
    description=description,
    summary="This API allows to get how much electricity given powerplants should generate based on fuels and load.",
    version=str(settings.API_VERSION),
)

VERSION_STRING = f"/v{settings.API_VERSION}"

app.include_router(powerplant.router, prefix=VERSION_STRING)


@app.get("/")
async def healthcheck():
    return {"message": "API is running!"}

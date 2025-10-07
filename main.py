import logging
from contextlib import asynccontextmanager

from fastapi import FastAPI
from fastapi.exceptions import RequestValidationError

from controllers import production_plan
from errors import server_error_handler, validation_exception_handler
from settings import settings


@asynccontextmanager
async def lifespan(app: FastAPI):
    # Configure logging at startup
    logging.basicConfig(
        level=logging.INFO,
        format="%(asctime)s - %(name)s - %(levelname)s - %(message)s",
    )
    logging.getLogger("uvicorn").setLevel(
        logging.INFO
    )  # optional: set Uvicorn log level

    logging.info("Logging initialized. Application starting up.")

    yield  # app runs here

    logging.info("Application shutting down.")


description = """
## Production Plan
    Amount of electricity produced by given powerplants.
"""

app = FastAPI(
    title="powerplant-coding-challenge",
    description=description,
    summary="This API allows getting how much load given powerplants should generate based on key parameters.",
    version=str(settings.API_VERSION),
    lifespan=lifespan,
)

# Exception handlers
app.add_exception_handler(Exception, server_error_handler) # 500 errors
app.add_exception_handler(RequestValidationError, validation_exception_handler)



# Routes
app.include_router(production_plan.router)


@app.get("/")
async def healthcheck():
    logging.info("Root endpoint called")
    return {"message": "API is running!"}

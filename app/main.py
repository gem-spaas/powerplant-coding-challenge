from fastapi import FastAPI, HTTPException
from fastapi.middleware.cors import CORSMiddleware
from app.models import EnergyDemand
from app.ucs import uniform_cost_search
from app.utils import categorize_powerplants
import logging
from datetime import datetime
from fastapi.responses import PlainTextResponse, JSONResponse
from fastapi.encoders import jsonable_encoder


app = FastAPI()

origins = [
    "http://localhost:8888",
]
app.add_middleware(
    CORSMiddleware,
    allow_origins=origins,
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

logging.basicConfig(filename="error.log", level=logging.ERROR)


@app.exception_handler(HTTPException)
def http_exception_handler(request, exc):
    logging.error(f"HTTPException: {exc.detail}, {datetime.now()}")
    return PlainTextResponse(str(exc.detail), status_code=exc.status_code)


@app.exception_handler(Exception)
async def general_exception_handler(request, exc):
    logging.error(f"Unhandled Exception: , {str(exc)}{datetime.now()}")
    return JSONResponse(
        status_code=exec.status_code if isinstance(exc, HTTPException) else 500,
        content=jsonable_encoder({"error:": str(exc)}),
    )


@app.get("/")
def welcome_index():
    return {"working_checking": "ok"}


@app.post("/productionplan")
def production_plan(energy_demand: EnergyDemand) -> list:
    powerplants = categorize_powerplants(energy_demand)
    min_cost_path = uniform_cost_search(powerplants, energy_demand.load)
    response = {}
    response["powerplants"] = []
    if min_cost_path is not None:
        for powerplant in powerplants:
            if powerplant.id not in min_cost_path.explored_ids:
                response["powerplants"].append({"name": powerplant.name, "p": 0})
            else:
                response["powerplants"].append(
                    {
                        "name": min_cost_path.powerplants[powerplant.id][
                            "powerplant_name"
                        ],
                        "p": min_cost_path.powerplants[powerplant.id]["load"],
                    }
                )
        if min_cost_path.enough_resource is False:
            response["error"] = "There is not sufficient resource"
            logging.error(
                f"Critical Issue: There is not sufficient resource, {datetime.now()}"
            )
    else:
        response = {"error": "There is an issue in API"}
    return response["powerplants"]


@app.get("/error")
def simulate_runtime_error():
    raise RuntimeError("This is a simulated runtime error.")

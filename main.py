
from fastapi import FastAPI
from models import Payload, Response
from power_calculator import PowerCalculator
import uvicorn


app = FastAPI()


@app.get("/")
def read_root():
    return {"Hello": "World"}


@app.post("/productionplan")
def productionplan(payload: Payload) -> list[Response]:
    service = PowerCalculator(
        load=payload.load,
        fuels=payload.fuels,
        powerplants=payload.powerplants
    )
    return service.get_powerplants_power()


if __name__ == "__main__":
    uvicorn.run(app, host="0.0.0.0", port=8888)

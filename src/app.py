from fastapi import FastAPI
from pydantic import BaseModel
from model.model import __version__ as model_version, energy_plan

app = FastAPI()

class DataIn(BaseModel):
    load: float
    fuels: dict
    powerplants: list

@app.get('/')
def index():
    status = {
        'health_check': 'OK',
        'model_version': model_version
    }
    return status

@app.post('/productionplan')
def productionplan(data: DataIn):
    return energy_plan(data)
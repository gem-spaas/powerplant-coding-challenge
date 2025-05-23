from fastapi import FastAPI, HTTPException
from app.schemas.request import ProductionPlanRequest
import app.service.merit_order as sm

app = FastAPI()

@app.get("/")
def root():
    return 'ENGIE PowerPlants API Ok'

@app.post("/productionplan")
def exec_production_plan(payload: ProductionPlanRequest):
    try:
        plan = sm.calculate_production_plan(payload)
        return plan
    except Exception as e:
        raise HTTPException(status_code=400, detail=str(e))

if __name__ == "__main__":
    uvicorn.run(app, host="0.0.0.0", port=8888)
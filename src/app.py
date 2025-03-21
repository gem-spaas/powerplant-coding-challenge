from fastapi import FastAPI, Request
from fastapi.responses import JSONResponse
import uvicorn

from src.planner import calculate_production_plan
from src.models import Payload
from src.validators import validate_payload

app = FastAPI()

@app.post("/productionplan")
async def production_plan(request: Request):
    try:
        payload_json = await request.json()
        validate_payload(payload_json)
        # 1. Transform payload JSON into a Payload object
        payload = Payload(
            load=payload_json["load"],
            fuels=payload_json["fuels"],
            powerplants=payload_json["powerplants"]
        )
        
        # 2. Calcuate production plan based on payload informaton
        plan = calculate_production_plan(payload.load, payload.fuels, payload.powerplants)

        # 3. Return in JSON format
        return JSONResponse(content=[p.to_dict() for p in plan])

    except Exception as e:
        return JSONResponse(status_code=500, content={"error": str(e)})

if __name__ == '__main__':
    uvicorn.run(app, host="0.0.0.0", port=8888)
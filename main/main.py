from fastapi import FastAPI, HTTPException
from pydantic import BaseModel
import logging
import merit_order

app = FastAPI()


# Pydantic model for the request payload
class ProductionPlanRequest(BaseModel):
    load: int
    fuels: dict
    powerplants: list


# Define the endpoint to handle POST requests
@app.post("/productionplan", response_model=list)
async def production_plan(payload: ProductionPlanRequest):
    # Access the payload data using Pydantic model
    load = payload.load
    fuels = payload.fuels
    powerplants = payload.powerplants

    # Process the production plan using my Merit Order function
    result_data, log = merit_order.mo_alg(load, fuels, powerplants)
    if log != 'All good':
        # Sending errors as response
        logging.error(f"An error occurred: {log}")
        logging.error(payload)
        raise HTTPException(status_code=500, detail=log)

    return result_data

if __name__ == '__main__':
    import uvicorn

    # Configure logging
    logging.basicConfig(filename='api.log', level=logging.ERROR, format='%(asctime)s - %(levelname)s - %(message)s')

    # Run the FastAPI application using Uvicorn
    uvicorn.run(app, host="0.0.0.0", port=8888)

# Third-party imports
from fastapi import FastAPI

# Local application imports
from app.routers import production_plan

# FastAPI application instance with metadata
app = FastAPI(
    title="Production Plan API",
    description="""Electricity production plan calculator based on merit order. \n
    Author: Henry Gerena - hagr27""",
    version="1.0.0"
)

# Include the router for production plan endpoints
app.include_router(production_plan.router)

@app.get("/", tags=["Root"])
async def root():
    """Health check endpoint to verify the API is running."""
    return {"message": "API is active"}

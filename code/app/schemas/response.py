from pydantic import BaseModel

class ProductionPlanResponse(BaseModel):
    name: str
    p: float
from pydantic import BaseModel


class ProductionPlanResponseItem(BaseModel):
    name: str
    p: float

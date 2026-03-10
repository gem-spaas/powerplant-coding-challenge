from typing import List

from pydantic import BaseModel, Field, field_validator


class ProductionPlanElement(BaseModel):
    name: str
    p: float = Field(ge=0)

    @field_validator("p")
    def p_multiple_of(cls, v):
        if round(v * 10) != v * 10:
            raise ValueError("p must be a multiple of 0.1")
        return v


ProductionPlanResponse = List[ProductionPlanElement]

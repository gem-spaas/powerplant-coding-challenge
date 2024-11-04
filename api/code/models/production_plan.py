"""
    This class provide the abstraction of the responses sent back to the client using pydantic library.



"""

from pydantic import BaseModel


class ProductionPlanItem(BaseModel):
    name: str
    p: float

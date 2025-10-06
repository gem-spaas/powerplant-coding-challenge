from schemas.production_plan import ProductionPlanRequest


class ProductionPlanService:
    _production_plan_request: ProductionPlanRequest
    def __init__(self, production_plan_request: ProductionPlanRequest):

    def calculate_production_plan(self, production_plan_request: ProductionPlanRequest):
        return NotImplementedError()


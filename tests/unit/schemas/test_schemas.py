import pytest
from polyfactory.factories.pydantic_factory import ModelFactory
from pydantic import ValidationError

from schemas.production_plan import ProductionPlanRequest


class ProductionPlanRequestFactory(ModelFactory[ProductionPlanRequest]):
    pass


def test_production_plan_schema__invalid_load__raises():
    with pytest.raises(ValidationError, match="Input should be greater than 0"):
        ProductionPlanRequestFactory.build(load=-1)

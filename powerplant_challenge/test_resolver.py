import os
import json

from unittest import TestCase
from .serializers import ProductionPlanSerializer
from .resolvers import ProductionPlanResolver

THIS_DIR = os.path.dirname(__file__)
FIXTURES_DIR = os.path.join(THIS_DIR, "..", "fixtures")


class ProductionPlanResolverTestCase(TestCase):
    def check_planner(self, planner, data):
        assert sum([p.power_supply for p in planner.plan]) == data["load"] == planner.load
        assert all(not p.power_supply or p.pmin <= p.power_supply <= p.pmax for p in planner.plan)

    def test_payloads(self):
        paths = [os.path.join(FIXTURES_DIR, f"payload{i}.json") for i in range(1, 4)]
        for path in paths:
            with open(path) as payload:
                data = json.loads(payload.read())
                serializer = ProductionPlanSerializer(data=data)
                assert serializer.is_valid()
                planner = ProductionPlanResolver(serializer.validated_data)
                self.check_planner(planner, data)

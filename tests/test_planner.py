import json
from pathlib import Path

from app.schemas.payload import ProductionPlanRequest
from app.services.production_planner import plan_production

BASE = Path(__file__).resolve().parents[1]


def load_payload(name: str):
    p = BASE / "example_payloads" / name
    return json.loads(p.read_text())


def test_plan_production_meets_load_and_constraints_payload1():
    payload = load_payload("payload1.json")
    req = ProductionPlanRequest(**payload)
    result = plan_production(req)

    # Sum of outputs should equal load within small tolerance
    total = sum(d.p for d in result)
    assert abs(total - req.load) < 1e-6

    # Each output within bounds
    for d, pp in zip(result, req.powerplants):
        # windturbine pmin considered 0 dynamically, but planner sets pmin=0 for wind
        if d.kind != "windturbine":
            assert pp.pmin - 1e-6 <= d.p <= pp.pmax + 1e-6
        else:
            assert 0.0 - 1e-6 <= d.p <= pp.pmax * req.fuels.wind / 100.0 + 1e-6


def test_plan_production_meets_load_and_constraints_payload2():
    payload = load_payload("payload2.json")
    req = ProductionPlanRequest(**payload)
    result = plan_production(req)

    total = sum(d.p for d in result)
    assert abs(total - req.load) < 1e-6

    for d, pp in zip(result, req.powerplants):
        if d.kind != "windturbine":
            assert pp.pmin - 1e-6 <= d.p <= pp.pmax + 1e-6
        else:
            assert d.p == 0.0  # no wind

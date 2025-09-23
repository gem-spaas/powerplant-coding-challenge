import json
from pathlib import Path
import pytest

from app.schemas.payload import ProductionPlanRequest, Fuels

BASE = Path(__file__).resolve().parents[1]


def load_payload(name: str):
    p = BASE / "example_payloads" / name
    return json.loads(p.read_text())


def test_fuels_aliases_population():
    data = {
        "gas(euro/MWh)": 10.0,
        "kerosine(euro/MWh)": 20.0,
        "co2(euro/ton)": 30.0,
        "wind(%)": 40.0,
    }
    fuels = Fuels(**data)
    # Attributes accessible by field name
    assert fuels.gas == 10.0
    assert fuels.kerosine == 20.0
    assert fuels.co2 == 30.0
    assert fuels.wind == 40.0


def test_production_plan_request_parses_example_payload():
    payload = load_payload("payload1.json")
    req = ProductionPlanRequest(**payload)
    assert req.load == payload["load"]
    assert len(req.powerplants) == len(payload["powerplants"])


def test_load_must_be_non_negative():
    with pytest.raises(ValueError):
        ProductionPlanRequest(
            load=-1,
            fuels=Fuels(
                **{
                    "gas(euro/MWh)": 10.0,
                    "kerosine(euro/MWh)": 20.0,
                    "co2(euro/ton)": 30.0,
                    "wind(%)": 40.0,
                }
            ),
            powerplants=[],
        )

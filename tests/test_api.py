import json
from pathlib import Path
from fastapi.testclient import TestClient


BASE = Path(__file__).resolve().parents[1]


def load_payload(name: str):
    p = BASE / "example_payloads" / name
    return json.loads(p.read_text())


def test_productionplan_endpoint_ok_payload1(application):
    client = TestClient(application)
    payload = load_payload("payload1.json")
    resp = client.post("/productionplan", json=payload)
    assert resp.status_code == 200
    data = resp.json()
    assert isinstance(data, list)
    # Length should match number of powerplants
    assert len(data) == len(payload["powerplants"])
    # Sum of p equals load
    total = sum(item["p"] for item in data)
    assert abs(total - payload["load"]) < 1e-6


def test_productionplan_rejects_negative_load(application):
    client = TestClient(application)
    payload = load_payload("payload1.json")
    payload["load"] = -10
    resp = client.post("/productionplan", json=payload)
    assert resp.status_code == 422

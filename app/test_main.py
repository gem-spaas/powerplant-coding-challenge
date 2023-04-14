import json
from fastapi.testclient import TestClient

from app.main import app

client = TestClient(app)

def load_fixture_payload(path: str):
    with open(f'fixtures/payloads/{path}.json') as f:
        payload = json.loads(''.join(f.readlines()))

    with open(f'fixtures/responses/response_{path}.json') as f:
        expected_response = json.loads(''.join(f.readlines()))

    return (payload, expected_response)

def test_productionplant():
    payload, expected_response = load_fixture_payload("payload1")
    response = client.post("/productionplant", json=payload)

    assert response.status_code == 200
    assert json.loads(response.json()) == expected_response

    payload, expected_response = load_fixture_payload("payload2")
    response = client.post("/productionplant", json=payload)

    assert response.status_code == 200
    assert json.loads(response.json()) == expected_response

    payload, expected_response = load_fixture_payload("payload3")
    response = client.post("/productionplant", json=payload)

    assert response.status_code == 200
    assert json.loads(response.json()) == expected_response
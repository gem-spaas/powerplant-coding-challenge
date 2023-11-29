import json

import pytest

from app import App, create_app


@pytest.fixture
def app():
    app = create_app()
    app.config.update(
        {
            "TESTING": True,
        }
    )
    yield app


@pytest.fixture
def client(app):
    return app.test_client()


@pytest.fixture
def payload1():
    with open("tests/example_payloads/payload1.json", "r") as file:
        return json.load(file)


@pytest.fixture
def payload3():
    with open("tests/example_payloads/payload3.json", "r") as file:
        return json.load(file)


@pytest.mark.happy
def test_method_get_production_plan(payload3):
    with open("tests/example_payloads/response3.json", "r") as file:
        expected_response = json.load(file)

    result = App().calculate_production_plan(payload3)

    assert [i for i in expected_response if i not in result] == []


@pytest.mark.happy
def test_endpoint_status(client):
    response = client.get("/status")
    assert response.status_code == 200


@pytest.mark.happy
def test_endpoint_productionplan(client, payload3):
    with open("tests/example_payloads/response3.json", "r") as file:
        expected_response = json.load(file)

    response = client.post("/productionplan", json=payload3)

    assert response.status_code == 200
    assert [i for i in expected_response if i not in response.json] == []


@pytest.mark.sad
def test_endpoint_productionplan_wrong_result(client, payload1):
    with open("tests/example_payloads/response3.json", "r") as file:
        expected_response = json.load(file)

    response = client.post("/productionplan", json=payload1)

    assert response.status_code == 200
    assert [i for i in expected_response if i not in response.json] != []


@pytest.mark.sad
def test_endpoint_productionplan_not_json(client, payload3):
    response = client.post("/productionplan", data=payload3)

    assert response.status_code == 400

import json

import pytest

from app import App


@pytest.fixture
def payload3():
    with open("tests/example_payloads/payload3.json", "r") as file:
        return json.load(file)


def test_payload3_production_plan(payload3):
    with open("tests/example_payloads/response3.json", "r") as file:
        expected_response = json.load(file)

    result = App().calculate_production_plan(payload3)

    assert [i for i in expected_response if i not in result] == []

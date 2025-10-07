import json

import pytest
from httpx import AsyncClient


@pytest.mark.anyio
async def test_get_production_plan__success(test_client: AsyncClient):
    # Load request body from JSON file
    with open("example_payloads/payload3.json") as f:
        payload = json.load(f)

    # Load request body from JSON file
    with open("example_payloads/response3.json") as f:
        expected_response = json.load(f)

    response = await test_client.post("/productionplan", json=payload)
    assert response.status_code == 200
    assert response.json() == expected_response

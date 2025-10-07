import pytest


@pytest.mark.anyio
async def test_healthcheck(test_client):
    response = await test_client.get("/")
    assert response.status_code == 200
    assert response.json() == {"message": "API is running!"}

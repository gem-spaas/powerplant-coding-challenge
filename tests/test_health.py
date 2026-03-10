from fastapi.testclient import TestClient

from app.main import app

client = TestClient(app)


def test_health():
    response = client.get("/healthcheck")

    assert response.status_code == 200
    assert response.json() == {"status": "ok"}

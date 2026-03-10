def test_health(client):
    response = client.get("/healthcheck")

    assert response.status_code == 200
    assert response.json() == {"status": "ok"}

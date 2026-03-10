import pytest
from fastapi.testclient import TestClient

from app.main import app


@pytest.fixture
def client():
    return TestClient(app)


@pytest.fixture
def valid_request():
    return {
        "load": 480,
        "fuels": {"gas(euro/MWh)": 13.4, "kerosine(euro/MWh)": 50.8, "co2(euro/ton)": 20, "wind(%)": 60},
        "powerplants": [{"name": "gasfiredbig1", "type": "gasfired", "efficiency": 0.53, "pmin": 100, "pmax": 460}],
    }

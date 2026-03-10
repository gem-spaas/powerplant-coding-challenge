import copy


def test_production_plan_valid(client, valid_request):
    response = client.post("/productionplan", json=valid_request)
    print("response", response.json())
    assert response.status_code == 200


def test_missing_load(client, valid_request):
    payload = copy.deepcopy(valid_request)
    payload.pop("load")

    response = client.post("/productionplan", json=payload)

    assert response.status_code == 422


def test_invalid_wind_percentage(client, valid_request):
    payload = copy.deepcopy(valid_request)
    payload["fuels"]["wind(%)"] = 150

    response = client.post("/productionplan", json=payload)

    assert response.status_code == 422


def test_negative_fuel_price(client, valid_request):
    payload = copy.deepcopy(valid_request)
    payload["fuels"]["gas(euro/MWh)"] = -10

    response = client.post("/productionplan", json=payload)

    assert response.status_code == 422


def test_invalid_efficiency(client, valid_request):
    payload = copy.deepcopy(valid_request)
    payload["powerplants"][0]["efficiency"] = 1.5

    response = client.post("/productionplan", json=payload)

    assert response.status_code == 422


def test_pmin_greater_than_pmax(client, valid_request):
    payload = copy.deepcopy(valid_request)
    payload["powerplants"][0]["pmin"] = 500
    payload["powerplants"][0]["pmax"] = 100

    response = client.post("/productionplan", json=payload)

    assert response.status_code == 422


def test_empty_powerplants_list(client, valid_request):
    payload = copy.deepcopy(valid_request)
    payload["powerplants"] = []

    response = client.post("/productionplan", json=payload)

    assert response.status_code == 422


def test_invalid_powerplant_type(client, valid_request):
    payload = copy.deepcopy(valid_request)
    payload["powerplants"][0]["type"] = "nuclear"

    response = client.post("/productionplan", json=payload)

    assert response.status_code == 422


def test_negative_load(client, valid_request):
    payload = copy.deepcopy(valid_request)
    payload["load"] = -100

    response = client.post("/productionplan", json=payload)

    assert response.status_code == 422

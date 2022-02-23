import json

import pytest
from fastapi.testclient import TestClient
from pytest import fixture

from powerplant.main import app
from powerplant.models import (
    PowerPlantLoad,
    PowerPlant,
    PowerPlantType,
    FuelsCosts,
    ProductionPlanRequest,
    PlantLoadResponse,
)


@fixture(scope="session")
def client():
    yield TestClient(app)


@fixture(scope="session")
def payload1():
    with open("tests/sample/payload1.json", "r") as f:
        data = json.load(f)
    return data


def test_read_main(client: TestClient, payload1: dict):
    response = client.post("/productionplan", json=payload1)
    assert response.status_code == 200


def test_power_plan_load_should_raise_value_error():
    plant = PowerPlant(
        name="gasfiredbig1", type="gasfired", efficiency=0.53, pmin=100, pmax=460
    )

    assert plant.power_plant_type == PowerPlantType.GAS_FIRED
    lower_value = 50
    with pytest.raises(ValueError):
        PowerPlantLoad(plant=plant, load=lower_value)
    upper_value = 500
    with pytest.raises(ValueError):
        PowerPlantLoad(plant=plant, load=upper_value)


def test_should_compute_cost():
    fuels = {
        "gas(euro/MWh)": 13.4,
        "kerosine(euro/MWh)": 50.8,
        "co2(euro/ton)": 20,
        "wind(%)": 60,
    }
    fuels_costs = FuelsCosts(**fuels)

    efficiency = 1
    plant = PowerPlant(
        name="gasfiredbig1",
        type=PowerPlantType.GAS_FIRED.value,
        efficiency=efficiency,
        pmin=100,
        pmax=460,
    )

    assert plant.compute_cost_per_mwh(fuels_costs) == 13.4 / efficiency

    efficiency = 1
    plant = PowerPlant(
        name="turbojet1",
        type=PowerPlantType.TURBO_JET.value,
        efficiency=efficiency,
        pmin=100,
        pmax=460,
    )

    assert plant.compute_cost_per_mwh(fuels_costs) == 50.8 / efficiency

    efficiency = 1
    plant = PowerPlant(
        name="wind1",
        type=PowerPlantType.WIND_TURBINE.value,
        efficiency=efficiency,
        pmin=100,
        pmax=460,
    )

    assert plant.compute_cost_per_mwh(fuels_costs) == 0


def test_should_optimize_load_with_low_consumption():
    fuels = {
        "gas(euro/MWh)": 13.4,
        "kerosine(euro/MWh)": 50.8,
        "co2(euro/ton)": 20,
        "wind(%)": 60,
    }
    fuels_costs = FuelsCosts(**fuels)
    efficiency = 1
    gas_plant = PowerPlant(
        name="gas1",
        type=PowerPlantType.GAS_FIRED.value,
        efficiency=efficiency,
        pmin=100,
        pmax=460,
    )
    efficiency = 1
    wind_plant = PowerPlant(
        name="wind1",
        type=PowerPlantType.WIND_TURBINE.value,
        efficiency=efficiency,
        pmin=0,
        pmax=30,
    )

    request = ProductionPlanRequest(
        load=10, fuels=fuels_costs, powerplants=[gas_plant, wind_plant]
    )

    assert request.optimize() == [
        PlantLoadResponse(name=wind_plant.name, p=10),
        PlantLoadResponse(name=gas_plant.name, p=0),
    ]


def test_should_optimize_combine_load():
    fuels = {
        "gas(euro/MWh)": 13.4,
        "kerosine(euro/MWh)": 50.8,
        "co2(euro/ton)": 20,
        "wind(%)": 100,
    }
    fuels_costs = FuelsCosts(**fuels)
    efficiency = 1
    gas_plant = PowerPlant(
        name="gas1",
        type=PowerPlantType.GAS_FIRED.value,
        efficiency=efficiency,
        pmin=100,
        pmax=460,
    )
    efficiency = 1
    wind_plant = PowerPlant(
        name="wind1",
        type=PowerPlantType.WIND_TURBINE.value,
        efficiency=efficiency,
        pmin=0,
        pmax=30,
    )

    request = ProductionPlanRequest(
        load=160, fuels=fuels_costs, powerplants=[gas_plant, wind_plant]
    )

    assert request.optimize() == [
        PlantLoadResponse(name=gas_plant.name, p=130),
        PlantLoadResponse(name=wind_plant.name, p=30),
    ]


def test_should_optimize_load_taking_into_account_pmin():
    fuels = {
        "gas(euro/MWh)": 13.4,
        "kerosine(euro/MWh)": 50.8,
        "co2(euro/ton)": 20,
        "wind(%)": 100,
    }
    fuels_costs = FuelsCosts(**fuels)
    efficiency = 1
    gas_plant = PowerPlant(
        name="gas1",
        type=PowerPlantType.GAS_FIRED.value,
        efficiency=efficiency,
        pmin=100,
        pmax=460,
    )
    efficiency = 1
    wind_plant = PowerPlant(
        name="wind1",
        type=PowerPlantType.WIND_TURBINE.value,
        efficiency=efficiency,
        pmin=0,
        pmax=30,
    )

    request = ProductionPlanRequest(
        load=110, fuels=fuels_costs, powerplants=[gas_plant, wind_plant]
    )

    assert request.optimize() == [
        PlantLoadResponse(name=gas_plant.name, p=100),
        PlantLoadResponse(name=wind_plant.name, p=10),
    ]


def test_should_optimize_taking_into_account_the_efficiency():
    fuels = {
        "gas(euro/MWh)": 13.4,
        "kerosine(euro/MWh)": 50.8,
        "co2(euro/ton)": 20,
        "wind(%)": 50,
    }
    fuels_costs = FuelsCosts(**fuels)
    efficiency = 0.5
    gas_plant = PowerPlant(
        name="gas1",
        type=PowerPlantType.GAS_FIRED.value,
        efficiency=efficiency,
        pmin=100,
        pmax=460,
    )
    efficiency = 1
    wind_plant = PowerPlant(
        name="wind1",
        type=PowerPlantType.WIND_TURBINE.value,
        efficiency=efficiency,
        pmin=0,
        pmax=30,
    )

    request = ProductionPlanRequest(
        load=110, fuels=fuels_costs, powerplants=[gas_plant, wind_plant]
    )

    assert request.optimize() == [
        PlantLoadResponse(name=gas_plant.name, p=95),
        PlantLoadResponse(name=wind_plant.name, p=15),
    ]


def test_should_optimize_when_having_three_plants_and_second_should_keep_min():
    fuels = {
        "gas(euro/MWh)": 20,
        "kerosine(euro/MWh)": 50,
        "co2(euro/ton)": 20,
        "wind(%)": 100,
    }
    fuels_costs = FuelsCosts(**fuels)
    efficiency = 1
    turbo_jet_plant = PowerPlant(
        name="turbo_jet1",
        type=PowerPlantType.TURBO_JET,
        efficiency=efficiency,
        pmin=200,
        pmax=300,
    )
    efficiency = 1
    gas_plant = PowerPlant(
        name="gas1",
        type=PowerPlantType.GAS_FIRED.value,
        efficiency=efficiency,
        pmin=200,
        pmax=300,
    )
    efficiency = 1
    wind_plant = PowerPlant(
        name="wind1",
        type=PowerPlantType.WIND_TURBINE.value,
        efficiency=efficiency,
        pmin=0,
        pmax=100,
    )

    request = ProductionPlanRequest(
        load=450,
        fuels=fuels_costs,
        powerplants=[gas_plant, wind_plant, turbo_jet_plant],
    )

    assert request.optimize() == [
        PlantLoadResponse(name=turbo_jet_plant.name, p=200),
        PlantLoadResponse(name=gas_plant.name, p=200),
        PlantLoadResponse(name=wind_plant.name, p=50),
    ]

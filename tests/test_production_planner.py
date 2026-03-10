import pytest

from app.data.power_plant_profiles import (
    DispatchEntry,
    GasFiredProfile,
    PowerPlantProfile,
    TurbojetProfile,
    WindTurbineProfile,
)
from app.data.request import Fuels, PowerPlant
from app.services.production_planner import ProductionPlanner


@pytest.fixture
def planner():
    return ProductionPlanner()


@pytest.fixture
def fuels():
    return Fuels.model_validate(
        {
            "gas(euro/MWh)": 13.4,
            "kerosine(euro/MWh)": 50.8,
            "co2(euro/ton)": 20.0,
            "wind(%)": 60.0,
        }
    )


def test_create_profile_gasfired(planner, fuels):
    plant = PowerPlant(
        name="gas1",
        type="gasfired",
        efficiency=0.5,
        pmin=100.0,
        pmax=300.0,
    )

    profile = planner._create_profile(plant, fuels)

    assert isinstance(profile, GasFiredProfile)
    assert profile.name == "gas1"
    assert profile.pmin == 100.0
    assert profile.pmax == 300.0
    assert profile.cost_per_MWh == pytest.approx((13.4 / 0.5) + (20.0 * 0.3))


def test_create_profile_turbojet(planner, fuels):
    plant = PowerPlant(
        name="jet1",
        type="turbojet",
        efficiency=0.3,
        pmin=0.0,
        pmax=50.0,
    )

    profile = planner._create_profile(plant, fuels)

    assert isinstance(profile, TurbojetProfile)
    assert profile.name == "jet1"
    assert profile.pmin == 0.0
    assert profile.pmax == 50.0
    assert profile.cost_per_MWh == pytest.approx(50.8 / 0.3)


def test_create_profile_windturbine(planner, fuels):
    plant = PowerPlant(
        name="wind1",
        type="windturbine",
        efficiency=1.0,
        pmin=0.0,
        pmax=150.0,
    )

    profile = planner._create_profile(plant, fuels)

    assert isinstance(profile, WindTurbineProfile)
    assert profile.name == "wind1"
    assert profile.pmin == 0.0
    assert profile.pmax == pytest.approx(150.0 * 0.6)
    assert profile.cost_per_MWh == 0


def test_create_profile_unknown_type_raises_value_error(planner, fuels):
    plant = PowerPlant.model_construct(
        name="unknown1",
        type="unknown",
        efficiency=1.0,
        pmin=0.0,
        pmax=100.0,
    )

    with pytest.raises(ValueError, match="Unknown power plant type"):
        planner._create_profile(plant, fuels)


def test_rebalance_prev_production_returns_true_when_excess_can_be_absorbed(planner):
    dispatch = [
        DispatchEntry(
            profile=PowerPlantProfile(
                name="plant1",
                cost_per_MWh=10.0,
                pmin=100.0,
                pmax=300.0,
            ),
            production=250.0,
        ),
        DispatchEntry(
            profile=PowerPlantProfile(
                name="plant2",
                cost_per_MWh=20.0,
                pmin=50.0,
                pmax=200.0,
            ),
            production=150.0,
        ),
    ]

    result = planner._rebalance_prev_production(dispatch, excess=80.0)

    assert result is True
    assert dispatch[0].production == 250.0
    assert dispatch[1].production == 70.0


def test_rebalance_prev_production_returns_false_when_excess_cannot_be_absorbed(planner):
    dispatch = [
        DispatchEntry(
            profile=PowerPlantProfile(
                name="plant1",
                cost_per_MWh=10.0,
                pmin=100.0,
                pmax=300.0,
            ),
            production=120.0,
        ),
        DispatchEntry(
            profile=PowerPlantProfile(
                name="plant2",
                cost_per_MWh=20.0,
                pmin=50.0,
                pmax=200.0,
            ),
            production=60.0,
        ),
    ]

    result = planner._rebalance_prev_production(dispatch, excess=40.0)

    assert result is False
    assert dispatch[0].production == 100.0
    assert dispatch[1].production == 50.0


def test_calculate_plan_simple_merit_order(planner):
    ordered_profiles = [
        PowerPlantProfile(name="wind1", cost_per_MWh=0.0, pmin=0.0, pmax=90.0),
        PowerPlantProfile(name="gas1", cost_per_MWh=10.0, pmin=100.0, pmax=300.0),
        PowerPlantProfile(name="jet1", cost_per_MWh=50.0, pmin=0.0, pmax=50.0),
    ]

    result = planner._calculate_plan(load=250.0, ordered_profiles=ordered_profiles)

    assert [item.name for item in result] == ["wind1", "gas1", "jet1"]
    assert [item.p for item in result] == [90.0, 160.0, 0.0]
    assert sum(item.p for item in result) == pytest.approx(250.0)


def test_calculate_plan_rebalances_when_remaining_load_is_below_pmin(planner):
    ordered_profiles = [
        PowerPlantProfile(name="wind1", cost_per_MWh=0.0, pmin=0.0, pmax=90.0),
        PowerPlantProfile(name="gas1", cost_per_MWh=10.0, pmin=100.0, pmax=300.0),
        PowerPlantProfile(name="gas2", cost_per_MWh=12.0, pmin=100.0, pmax=200.0),
    ]

    result = planner._calculate_plan(load=410.0, ordered_profiles=ordered_profiles)

    assert [item.name for item in result] == ["wind1", "gas1", "gas2"]
    assert [item.p for item in result] == [90.0, 220.0, 100.0]
    assert sum(item.p for item in result) == pytest.approx(410.0)


def test_calculate_plan_raises_value_error_when_load_cannot_be_satisfied(planner):
    ordered_profiles = [
        PowerPlantProfile(name="wind1", cost_per_MWh=0.0, pmin=0.0, pmax=50.0),
        PowerPlantProfile(name="gas1", cost_per_MWh=10.0, pmin=100.0, pmax=100.0),
    ]

    with pytest.raises(ValueError, match="Unable to satisfy load"):
        planner._calculate_plan(load=300.0, ordered_profiles=ordered_profiles)


def test_create_plan_builds_profiles_and_returns_plan(planner, fuels):
    powerplants = [
        PowerPlant(
            name="gas1",
            type="gasfired",
            efficiency=0.5,
            pmin=100.0,
            pmax=300.0,
        ),
        PowerPlant(
            name="wind1",
            type="windturbine",
            efficiency=1.0,
            pmin=0.0,
            pmax=150.0,
        ),
    ]

    result = planner.create_plan(load=190.0, fuels=fuels, powerplants=powerplants)

    assert len(result) == 2
    assert result[0].name == "wind1"
    assert result[0].p == pytest.approx(90.0)
    assert result[1].name == "gas1"
    assert result[1].p == pytest.approx(100.0)
    assert sum(item.p for item in result) == pytest.approx(190.0)

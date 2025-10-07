from models.gas_fired import GasFiredPowerPlant
from models.turbojet import TurboJetPowerPlant
from models.wind_turbine import WindTurbinePowerPlant
from schemas.production_plan import PowerPlantType, PowerPlant, Fuels


def powerplant_factory(
    powerplant: PowerPlant, fuels: Fuels
) -> GasFiredPowerPlant | TurboJetPowerPlant | WindTurbinePowerPlant:
    if powerplant.type == PowerPlantType.GAS_FIRED:
        return GasFiredPowerPlant(
            name=powerplant.name,
            co2_price_per_ton=fuels.co2_euro_per_ton,
            pmin=powerplant.pmin,
            pmax=powerplant.pmax,
            price_per_MWh=fuels.gas_euro_per_MWh,
            efficiency=powerplant.efficiency,
        )
    elif powerplant.type == PowerPlantType.TURBOJET:
        return TurboJetPowerPlant(
            name=powerplant.name,
            pmin=powerplant.pmin,
            pmax=powerplant.pmax,
            price_per_MWh=fuels.kerosine_euro_per_MWh,
            efficiency=powerplant.efficiency,
        )
    elif powerplant.type == PowerPlantType.WIND_TURBINE:
        return WindTurbinePowerPlant(
            name=powerplant.name,
            pmin=powerplant.pmin,
            pmax=powerplant.pmax,
            wind_percent=fuels.wind_percent,
            price_per_MWh=0.0,
            efficiency=powerplant.efficiency,
        )
    # Unsupported Powerplant type
    raise ValueError(f"Unknown powerplant type: {powerplant.type}")

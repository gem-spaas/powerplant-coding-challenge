from app.models import Gasfired, Turbojet, Windturbine, PowerPlantAsset, EnergyDemand


def categorize_powerplants(energy_demand: EnergyDemand) -> [PowerPlantAsset]:
    powerplants = []
    for id, power_plant_asset in enumerate(energy_demand.powerplants):
        power_plant_asset_type = power_plant_asset.type
        if power_plant_asset_type == "gasfired":
            powerplants.append(
                Gasfired(
                    id,
                    power_plant_asset.name,
                    power_plant_asset.efficiency,
                    power_plant_asset.pmin,
                    power_plant_asset.pmax,
                    energy_demand.fuels.gas_euro_mwh,
                    energy_demand.fuels.co2_euro_ton,
                )
            )
        elif power_plant_asset_type == "turbojet":
            powerplants.append(
                Turbojet(
                    id,
                    power_plant_asset.name,
                    power_plant_asset.pmax,
                    energy_demand.fuels.kerosine_euro_mwh,
                )
            )
        elif power_plant_asset_type == "windturbine":
            powerplants.append(
                Windturbine(
                    id,
                    power_plant_asset.name,
                    power_plant_asset.pmax,
                    energy_demand.fuels.wind_percentage,
                )
            )
        else:
            print(f"There is no power plant like {power_plant_asset_type}")
    powerplants.sort()
    return powerplants

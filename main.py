ifrom flask import Flask, request
import json

app = Flask(__name__)
# api = Api(app)


@app.route("/productionplan", methods=["POST"])
def api_run():
    payload = json.loads(request.data)
    result = compute_power_per_plant(payload)
    return json.dumps(result, indent=4)


def compute_power_per_plant(payload):
    list_windturbines = [plant for plant in payload["powerplants"] if plant["type"] == "windturbine"]
    list_gasplants = [plant for plant in payload["powerplants"] if plant["type"] == "gasfired"]
    list_turbojets = [plant for plant in payload["powerplants"] if plant["type"] == "turbojet"]
    WIND_PERCENTAGE = payload["fuels"]["wind(%)"] / 100
    GAS_PRICE = payload["fuels"]["gas(euro/MWh)"]
    KEROSINE_PRICE = payload["fuels"]["kerosine(euro/MWh)"]

    for plant in list_gasplants:
        plant["cost_per_Mwh"] = GAS_PRICE / plant["efficiency"]
    for plant in list_turbojets:
        plant["cost_per_Mwh"] = KEROSINE_PRICE / plant["efficiency"]

    list_windturbines.sort(key=lambda x: x["pmax"], reverse=True)
    list_gasplants.sort(key=lambda x: x["cost_per_Mwh"])
    list_turbojets.sort(key=lambda x: x["cost_per_Mwh"])
    power_per_plant = []
    load_left = payload["load"]

    ## first we use the windturbines
    for windturbine in list_windturbines:
        power_max_turbine = round(windturbine["pmax"] * WIND_PERCENTAGE, 1)
        if load_left >= power_max_turbine:
            power_per_plant.append({"name": windturbine["name"], "p": power_max_turbine})
            load_left -= power_max_turbine

    ## then the gas plants
    for gasplant in list_gasplants:
        if load_left >= gasplant["pmin"]:
            ## in this case we are sure not to skip a plant
            if load_left >= gasplant["pmax"]:
                power_per_plant.append({"name": gasplant["name"], "p": gasplant["pmax"]})
                load_left -= gasplant["pmax"]
            else:
                power_per_plant.append({"name": gasplant["name"], "p": load_left})
                previous_plant_power = load_left
                load_left = 0


        else:
            if load_left >= 0:
                ## in this case we might skip a plant because the load left is lower than the pmin
                ### so we need to retract some power from the previous plan to ensure at least pmin left for the new plant
                power_per_plant[-1]["p"] = power_per_plant[-1]["p"] - gasplant["pmin"] + load_left
                power_per_plant.append({"name": gasplant["name"], "p": gasplant["pmin"]})
                load_left = 0
            else:
                ### in this case load_left = 0 and it is ok to skip / put 0 power for the new plant
                power_per_plant.append({"name": gasplant["name"], "p": 0})

    ## then the turbojets
    for turbojet in list_turbojets:
        if load_left >= turbojet["pmax"]:
            power_per_plant.append({"name": turbojet["name"], "p": turbojet["pmax"]})
            load_left -= turbojet["pmax"]
        else:
            power_per_plant.append({"name": turbojet["name"], "p": load_left})
            load_left = 0

    return power_per_plant


if __name__ == "__main__":
    app.run('localhost', port=8888) # run our Flask app

from flask import Flask, request
import json
import pandas as pd

app = Flask(__name__)


@app.route("/productionplan", methods=['POST'])
def get_prod_plat():
    json_data = json.loads(request.data)
    output = process_input(json_data)
    return json.dumps(output, indent=4), 200


def process_input(data: dict):
    # get list of available powerplats :
    load = data.get('load')
    # Calculate cost
    potential_capacity_data = calculate_potential_capacity(data)
    sorted_potential_capacity_data = sorted(potential_capacity_data, key= lambda x : x['efficiency'], reverse=True)
    energy_plan_data = calculate_energy_play(load, sorted_potential_capacity_data)

    return energy_plan_data


def calculate_energy_play(load, newlist):
    target = 0
    end_result = []
    iter_data = iter(newlist)
    while target < load:
        d = next(iter_data)
        if (target + d.get('pmax')) <= load:
            target += d.get('pmax')
            d['p'] = d.get('pmax')
            end_result.append(d)
        else:
            remaining = load - target
            if remaining > d.get('pmin'):
                d['p'] = remaining
                target += remaining
                end_result.append(d)
            else:
                continue
    end_result = filter_keys(end_result)
    return end_result


def filter_keys(list_of_dict):
    return [{key: my_dict[key] for key in my_dict.keys() and {'p', 'name'}}  for my_dict in list_of_dict]


def calculate_potential_capacity(data):
    processed_data = []
    for powerplant in data.get("powerplants"):
        if powerplant.get('type') == "windturbine":
            fuel = data.get("fuels").get("wind(%)") / 100
            powerplant = calculate_metrics(fuel, powerplant)

        elif powerplant.get('type') == "gasfired":
            fuel = data.get("fuels").get("gas(euro/MWh)")
            powerplant = calculate_metrics(fuel, powerplant)

        elif powerplant.get('type') == "turbojet":
            fuel = data.get("fuels").get("kerosine(euro/MWh)")
            powerplant = calculate_metrics(fuel, powerplant)

        if powerplant['p/MWh'] > 0:
            processed_data.append(powerplant)

    return processed_data


def calculate_metrics(fuel, powerplant):
    powerplant['p/MWh'] = powerplant['efficiency'] * fuel
    return powerplant



if __name__ == "__main__":
    app.run(debug=True)
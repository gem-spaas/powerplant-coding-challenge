from flask import Flask, request
import json

from powerplant import Gasfire, Turbojet, Windplant

class_mapping = {'gasfired': Gasfire, 'turbojet': Turbojet, 'windturbine': Windplant}
app = Flask(__name__)


@app.route("/productionplan", methods=['POST'])
def get_prod_plat():
    try:
        json_data = json.loads(request.data)
    except json.decoder.JSONDecodeError as e:
        return f"You provided an invalid data format, please retry with a valid JSON", 401
    output = process_input(json_data)
    return json.dumps(output, indent=4), 200


def process_input(data: dict):
    # get list of available powerplats :
    load = data.get('load')
    # Calculate cost
    potential_capacity_data = calculate_potential_capacity(data)
    sorted_potential_capacity_data = sorted(potential_capacity_data, key=lambda x: x.efficiency, reverse=True)
    energy_plan_data, target = calculate_energy_plan(load, sorted_potential_capacity_data)
    return energy_plan_data


def calculate_energy_plan(load, newlist):
    target = 0
    end_result = []
    while target < load:
        for d in newlist:
            # first add windturbines if available
            remaining = load - target
            if d.type == 'windturbine' and d.pmax <= remaining:
                entry = {'name': d.name, 'p': d.pmax}
                target += d.pmax
                end_result.append(entry)

            # next add gasfire
            if d.type == "gasfired":
                remaining = load - target
                if remaining > 0:
                    # the remaining can be handled by on plant
                    if d.pmax > remaining:
                        entry = {'name': d.name, 'p': remaining}
                        target += remaining
                        end_result.append(entry)

                    else:
                        # can it be devided on two plants ?
                        plants = get_2_gasfired_plants(newlist)
                        sum_plants = sum([x.pmax for x in plants])
                        if sum_plants > remaining:
                            # each plant can take half of the load
                            for plant in plants:
                                gas_plant = {'name': plant.name, 'p': remaining / 2}
                                target += remaining / 2
                                end_result.append(gas_plant)
                        else:
                            for plant in plants:
                                gas_plant = {'name': plant.name, 'p': plant.pmax}
                                target += plant.pmax
                                end_result.append(gas_plant)

                            # use third gasfired plant
                            remaining = load - target
                            third = list(filter(lambda x: x.type == 'gasfired', newlist))[-1]
                            last_element = {'name': third.name, 'p': remaining}
                            end_result.append(last_element)
                            target += remaining

                    if d.type == 'turbojet' and remaining > 0:
                        if d.pmax > remaining > d.pmin:
                            turbojet = {'name': d.name, 'p': remaining}
                            target += remaining
                            end_result.append(turbojet)

    return end_result, target


def get_2_gasfired_plants(newlist):
    return list(filter(lambda x: x.type == 'gasfired', newlist))[:2]


def calculate_potential_capacity(data):
    processed_data = []
    fuel = 0
    for powerplant in data.get("powerplants"):
        plant = class_mapping.get(powerplant.get('type'))(*powerplant.values(), data)
        if plant.pmwh > 0:
            processed_data.append(plant)
    return processed_data


if __name__ == "__main__":
    app.run(debug=True)

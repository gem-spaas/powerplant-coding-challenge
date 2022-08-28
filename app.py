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
    print(energy_plan_data)
    print(target)
    if load > target:
        print("Target not reached")
        print("adjusting")
        energy_plan_data = adjust_remaining(sorted_potential_capacity_data, energy_plan_data, load, target)

    return energy_plan_data


def adjust_remaining(sorted_potential_capacity_data, energy_plan_data, load, target):
    entry = {}
    for d in sorted_potential_capacity_data:
        if d.name in [i['name'] for i in energy_plan_data]:
            print(f"skipping {d.name}")
            continue

        remaining = load - target
        if d.pmin <= remaining <= d.pmax:
            print("eligable")
            entry['p'] = remaining
            entry['name'] = d.name
            energy_plan_data.append(entry)
            print(energy_plan_data)
            return energy_plan_data


def calculate_energy_plan(load, newlist):
    target = 0
    end_result = []
    while target < load:
        for d in newlist:
            entry = {}
            # first add windturbines if available
            remaining = load - target
            if d.type == 'windturbine' and d.pmax <= remaining:
                target += d.pmax
                entry['name'] = d.name
                entry['p'] = d.pmax
                end_result.append(entry)

            # next add gasfire
            if d.type == "gasfired":
                remaining = load - target
                if remaining > 0:
                    # the remaining can be handled by on plant
                    if d.pmax > remaining:
                        target += remaining
                        entry['name'] = d.name
                        entry['p'] = remaining
                        end_result.append(entry)

                    else:
                        # can it be devided on two plants ?
                        plants = list(filter(lambda x: x.type == 'gasfired', newlist))[:2]

                        sum_plants = sum([x.pmax for x in plants])
                        if sum_plants > remaining:
                            # each plant can take half of the load
                            for plant in plants:
                                gas_plant = {}
                                target += remaining / 2
                                gas_plant['name'] = plant.name
                                gas_plant['p'] = int(remaining / 2)
                                end_result.append(gas_plant)
                        else:
                            # use third gasfired plant
                            for plant in plants:
                                gas_plant = {}
                                target += plant.pmax
                                gas_plant['name'] = plant.name
                                gas_plant['p'] = plant.pmax
                                end_result.append(gas_plant)

                            remaining = load - target
                            third = list(filter(lambda x: x.type == 'gasfired', newlist))[-1]
                            last_element = {'name': third.name, 'p': remaining}
                            end_result.append(last_element)
                            target += remaining




                    if d.type == 'turbojet' and remaining > 0:
                        if d.pmax > remaining > d.pmin:
                            target += remaining
                            entry['name'] = d.name
                            entry['p'] = remaining
                            end_result.append(entry)



    return end_result, target


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

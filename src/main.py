from flask import Flask, request, jsonify
from flasgger import Swagger, swag_from
import os
import yaml

app = Flask(__name__)
with open(os.path.join(os.path.dirname(__file__), '../swagger_spec.yml')) as f:
    swagger_spec = yaml.safe_load(f)
swagger = Swagger(app, template=swagger_spec)


@app.route('/productionplan', methods=['POST'])
@swag_from(os.path.join(os.path.dirname(__file__), '../swagger_spec.yml'))
def production_plan():
    data = request.json
    load = data['load']
    co2_cost = data.get('co2_cost', 0)
    fuels = data['fuels']
    powerplants = data['powerplants']

    # Calculate cost per MWh for each powerplant
    for plant in powerplants:
        if plant['type'] == 'windturbine':
            plant['cost'] = 0
            plant['pmax'] = plant['pmax'] * (fuels['wind(%)'] / 100)
        elif plant['type'] == 'gasfired':
            plant['cost'] = (fuels['gas(euro/MWh)'] + co2_cost) / plant['efficiency']
        elif plant['type'] == 'turbojet':
            plant['cost'] = fuels['kerosine(euro/MWh)'] / plant['efficiency']

    # Sort powerplants by cost
    powerplants = sorted(powerplants, key=lambda x: x['cost'])

    # Allocate load to powerplants
    remaining_load = load
    result = []
    for plant in powerplants:
        if remaining_load <= 0:
            power = 0
        else:
            power = min(plant['pmax'], remaining_load)
            power = max(power, plant['pmin']) if remaining_load >= plant['pmin'] else 0

        remaining_load -= power
        result.append({"name": plant['name'], "p": round(power, 1)})

    # Ensure no overproduction and correct allocation
    if remaining_load > 0:
        for plant in result:
            if plant['p'] < [p['pmax'] for p in powerplants if p['name'] == plant['name']][0]:
                plant['p'] = round(plant['p'] + remaining_load, 1)
                break

    return jsonify(result)


if __name__ == '__main__':
    app.run(host='0.0.0.0', port=8888)

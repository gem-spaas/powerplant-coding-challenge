from flask import Flask, request, jsonify
import numpy as np
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

    # Calculate cost per MWh for each power plant
    for plant in powerplants:
        if plant['type'] == 'windturbine':
            plant['cost'] = 0
            plant['pmax'] = plant['pmax'] * (fuels['wind(%)'] / 100)
        elif plant['type'] == 'gasfired':
            plant['cost'] = (fuels['gas(euro/MWh)'] + co2_cost) / plant['efficiency']
        elif plant['type'] == 'turbojet':
            plant['cost'] = fuels['kerosine(euro/MWh)'] / plant['efficiency']

    n = len(powerplants)
    inf = float('inf')
    precision = 10  # Use a precision of 0.1 MW

    # Scale the load to handle floating-point precision
    scaled_load = int(load * precision)
    dp = np.full((n + 1, scaled_load + 1), inf)
    dp[0][0] = 0
    choice = np.full((n + 1, scaled_load + 1), -1)

    for i in range(1, n + 1):
        plant = powerplants[i - 1]
        pmin_scaled = int(plant['pmin'] * precision)
        pmax_scaled = int(plant['pmax'] * precision)
        cost_per_unit = plant['cost'] / precision
        for j in range(scaled_load + 1):
            dp[i][j] = dp[i - 1][j]
            for p in range(pmin_scaled, pmax_scaled + 1):
                if j >= p and dp[i - 1][j - p] + cost_per_unit * p < dp[i][j]:
                    dp[i][j] = dp[i - 1][j - p] + cost_per_unit * p
                    choice[i][j] = p

    if dp[n][scaled_load] == inf:
        return jsonify({"error": "No solution found"}), 500

    result = []
    remaining_load = scaled_load
    for i in range(n, 0, -1):
        p = choice[i][remaining_load]
        if p == -1:
            result.append({"name": powerplants[i - 1]['name'], "p": 0})
        else:
            result.append({"name": powerplants[i - 1]['name'], "p": round(p / precision, 1)})
            remaining_load -= p

    result.reverse()

    return jsonify(result)


if __name__ == '__main__':
    app.run(host='0.0.0.0', port=8888)

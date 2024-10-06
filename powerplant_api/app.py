from flask import Flask, request, jsonify

from models import PowerPlantData,FuelData
from services.production_plan import PowerDistributionCalculator


app = Flask(__name__)

@app.route('/productionplan', methods=['POST'])
def production_plan():
    try:
        data = request.get_json()
        load_data = data["load"]
        fuels_data = data["fuels"]
        powerplants_data = data["powerplants"]

        fuels = FuelData.get_data(fuels_data)
        powerplants = [PowerPlantData.get_data(data) for data in powerplants_data]

        calculator = PowerDistributionCalculator(
            load=load_data,
            fuels=fuels,
            powerplants=powerplants
            )
        response = calculator.get_response()


        # Return the result as JSON
        return jsonify(response), 200

    except Exception as e:
        return jsonify({"error": str(e)}), 500

if __name__ == '__main__':
    app.run(debug=True, port=8888)

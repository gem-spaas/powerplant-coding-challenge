import logging
from flask import request, jsonify, current_app as app
from .services import calculate_power_distribution


@app.route("/productionplan", methods=["POST"])
def production_plan():
    """Calculate power distribution based on input payload."""

    try:
        # Parse JSON payload
        payload = request.get_json()
        load = payload["load"]
        fuels = payload["fuels"]
        powerplants = payload["powerplants"]

        # Calculate power distribution
        distribution = calculate_power_distribution(load, fuels, powerplants)

        return jsonify(distribution), 200
    except (KeyError, TypeError, ValueError) as e:
        logging.error("Error processing request: %s", e)
        return jsonify({"error": str(e)}), 400

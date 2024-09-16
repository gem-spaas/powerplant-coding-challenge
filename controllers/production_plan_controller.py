from flask import request, jsonify
from models.powerplants import (
    init_all_powerplant_units,
    get_merit_order_production_plan,
)


def get_production_plan():
    request_values = request.json
    required_load = request_values.get("load", 0)
    powerplant_values = request_values.get("powerplants", {})
    productivity_settings = request_values.get("fuels")
    try:
        assert (
            isinstance(required_load, float) or isinstance(required_load, int)
        ) and required_load > 0
        powerplants = init_all_powerplant_units(
            powerplant_values, productivity_settings
        )
        assert len(powerplants) != 0
        load_production = get_merit_order_production_plan(required_load, powerplants)
    except AssertionError:
        return jsonify(
            {"Error": "No valid load or powerplants missing in the given values."}
        )
    except Exception as e:
        return jsonify(
            {
                "Error": f"Some troubles append during processing. '{e}'. Please check the values"
            }
        )
    return jsonify(load_production)

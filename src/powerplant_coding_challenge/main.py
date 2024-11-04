from flask import Flask, request, jsonify

from powerplant_coding_challenge.data_processing.cost_computer import compute_powerplant_production
from powerplant_coding_challenge.data_retrieving.json_data_retriever import retrieve_data_from_json_payload
from powerplant_coding_challenge.utils.dataclasses import Payload
import logging

logging.basicConfig(format="%(asctime)s %(levelname)-8s %(message)s", level=logging.INFO, datefmt="%Y-%m-%d %H:%M:%S")
logger = logging.getLogger(__name__)
app = Flask(__name__)


@app.route('/productionplan', methods=['POST'])
def compute_production_plan():
    payload = request.get_json()
    if payload is None:
        return "No JSON received", 400
    try:
        payload_loaded: Payload = retrieve_data_from_json_payload(json_payload=payload)
        production_computed: list = compute_powerplant_production(payload_loaded=payload_loaded)
        return jsonify(production_computed), 200
    except KeyError:
        message = "the input json is incorrectly formatted, please follow the format given here https://github.com/gem-spaas/powerplant-coding-challenge/blob/master/example_payloads/payload1.json"
        logger.error(message)
        return jsonify({"key error in json input": message}), 422
    except TypeError:
        message = "the input json is incorrectly formatted, please follow the format given here https://github.com/gem-spaas/powerplant-coding-challenge/blob/master/example_payloads/payload1.json"
        logger.error(message)
        return jsonify({"value error in json input": message}), 422
    except Exception as e:
        return jsonify({"error": f"An error occurred: {str(e)}"}), 500


if __name__ == "__main__":
    app.run(debug=True, host="0.0.0.0", port=8888)

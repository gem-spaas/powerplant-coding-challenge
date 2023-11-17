import logging
import traceback
from flask import Flask, request, jsonify, make_response

from logger import get_logger
from utils import load_config
from production_plan import ProductionPlanGenerator


# the Flask application
app = Flask(__name__)


@app.route('/productionplan', methods=['POST'])
def create_production_plan():
    """
    API endpoint to request a production plan.
    """
    request_data = request.get_json()
    load = request_data.get('load', None)
    fuels = request_data.get('fuels', None)
    powerplants = request_data.get('powerplants', None)
    if not (load and fuels and powerplants):
        return make_response(jsonify({'message': 'Invalid or missing request parameters'}), 400)
    try:
        response_data = ProductionPlanGenerator(load=load,
                                                fuels=fuels,
                                                powerplants=powerplants).get_production_plan()
    except Exception:  # not nice
        logging.error('Unexpected error has occurred during the Production Plan generation. Request: {}'.format(
            request_data))
        logging.error(traceback.format_exc())
        return make_response(jsonify({'message': 'Unexpected error during the Production Plan generation'}), 500)
    return make_response(jsonify(response_data), 200)


if __name__ == '__main__':
    get_logger()
    app.run(host='0.0.0.0', port=int(load_config().get('api_server_port', 8888)))

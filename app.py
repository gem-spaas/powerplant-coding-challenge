#!/usr/bin/env python3

import sys
import json_logging, logging

from flask import Flask, request, jsonify

from gem.power import PowerSupply
from gem.payload import Payload


app = Flask(__name__)
json_logging.init_flask(enable_json=True)
json_logging.init_request_instrument(app)
logger = logging.getLogger("productionplan")
logger.setLevel(logging.DEBUG)
logger.addHandler(logging.StreamHandler(sys.stderr))


def log_error(msg):
    logger.error(msg)
    return msg

@app.route("/productionplan", methods=["POST"])
def production_plan():
    try:
        payload = request.get_json()
        if payload:
            logger.debug({ "payload" : payload})
            response = PowerSupply(Payload(payload)).calculate_production()
            logger.debug({ "response" : response })
        else:
            response = log_error({ "error": "The requested payload is not valid" })
    except Exception as e:
        response = log_error({ "error": { "msg": "Exception in 'productionplan' endpoint occured", "desc": str(e) } })
    
    return jsonify(response)


if __name__ == '__main__':
    app.run(debug=True, host='0.0.0.0', port=8888)
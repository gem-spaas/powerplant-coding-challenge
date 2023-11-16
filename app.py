#!/usr/bin/env python3

import sys
import json_logging, logging

from flask import Flask, request, jsonify


app = Flask(__name__)
json_logging.init_flask(enable_json=True)
json_logging.init_request_instrument(app)
logger = logging.getLogger("productionplan")
logger.setLevel(logging.DEBUG)
logger.addHandler(logging.StreamHandler(sys.stderr))


@app.route("/productionplan", methods=["POST"])
def production_plan():
    try:
        payload = request.get_json()
        request.get_json()
        if not payload:
            logger.error({"error": "Invalid payload"})
            response = {"error": "Invalid payload"}
        else:
            logger.debug({"payload" : payload})
            response = payload
    except Exception as e:
        logger.error({"error" : "Error in production plan endpoint"})
        response = { "error": str(e) }

    logger.debug({"response" : response})
    return jsonify(response)


if __name__ == '__main__':
    app.run(debug=True, host='0.0.0.0', port=8888)
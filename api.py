from flask import Blueprint, request, current_app, jsonify, Response
import json, flask
from test import solver

app = flask.Flask(__name__)
app.config["DEBUG"] = True

@app.route('/', methods=['GET'])
def note():
    return 'powerplant-coding-challenge-demo'

@app.route('/productionplan', methods=['POST'])
def productionplan():
    # payload = flask.request.json
    current_app.logger.info(f"Solver service requested")
    if request.json is None:
        current_app.logger.warning("No json file provided")
        return jsonify(message="No json file provided"), 400
    try:
    #regex to verify data here
        response = solver(flask.request.json)
        current_app.logger.warning("Problem solved")
        return jsonify(response), 200
    except IndexError as e:
        current_app.logger.error("Unsolvable request")
        return jsonify(message=f"Unsolvable request"), 202
    except Exception as e:
        current_app.logger.error("Solver service just crashed")
    return jsonify(message=f"Unfortunately, solver service just crashed"), 500

app.run('localhost', port=8888)

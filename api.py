from flask import Blueprint, request, current_app, jsonify, Response
import json, flask
from test import Network, solver
import pandas as pd


app = flask.Flask(__name__)
app.config["DEBUG"] = True

@app.route('/', methods=['GET'])
def home():
    #data = flask.request.form
    #print(data)
    print(solver())
    return solver()

@app.route('/productionplan', methods=['POST'])
def productionplan():
    data = flask.request.json
    #regex to verify data here
    LB1 = LoadBalancer(False, data)
    result = LB1.CalcCost()

    return result

app.run('localhost', port=8888)

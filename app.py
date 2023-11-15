from flask import Flask, Response, request

from service import Service


class App:
    def __init__(self):
        self.service = Service()

    def status(self):
        return Response(status=200)

    def productionplan(self):
        if request.content_type != "application/json":
            return {"msg": "Invalid body. Must be a JSON"}, 400

        # Retrieve the JSON body as a dict
        payload = request.get_json()

        if not payload:
            return {"msg": "Invalid body"}, 400

        try:
            fuels = payload["fuels"]
            load = payload["load"]
            powerplants = payload["powerplants"]
        except KeyError as e:
            return {"msg": f"Invalid payload format: '{e}' is missing"}, 400

        powerplants = self.service.create_powerplants(powerplants, fuels)

        production_plan = self.service.get_production_plan(load, powerplants, fuels)

        return production_plan, 200


def create_app():
    flask_app = Flask(__name__, instance_relative_config=True)
    app = App()

    @flask_app.route("/status", methods=["GET"])
    def status():
        return app.status()

    @flask_app.route("/productionplan", methods=["POST"])
    def productionplan():
        return app.productionplan()

    return flask_app

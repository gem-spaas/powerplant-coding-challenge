from flask import Flask, Response, request

from service import Service


class App:
    def __init__(self):
        self.service = Service()

    def status(self):
        """
        Checks that the app is up and running.

        Returns:
            Response: status 200 if ok.
        """
        return Response(status=200)

    def productionplan(self):
        """
        Method that checks that the body request and the payload are correct,
        and calls the specific services to create and order the given
        powerplants and calculate its production plan.

        Returns:
            dictionary: contains the resulting production plan, served as JSON.
        """
        # Check that the content type is correct
        if request.content_type != "application/json":
            return {"msg": "Invalid body. Must be a JSON"}, 400

        # Retrieve the JSON body as a dict
        payload = request.get_json()

        if not payload:
            return {"msg": "Invalid body"}, 400

        # Separate the payload, with a basic error handling
        try:
            fuels = payload["fuels"]
            load = payload["load"]
            powerplants_data = payload["powerplants"]
        except KeyError as e:
            return {"msg": f"Invalid payload format: '{e}' is missing"}, 400

        powerplants = self.service.create_powerplants(powerplants_data, fuels)
        powerplants = self.service.sort_by_power_cost(powerplants)

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

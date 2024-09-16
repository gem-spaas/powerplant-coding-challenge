from flask import Blueprint
from controllers.production_plan_controller import get_production_plan

production_plan_blue_print = Blueprint("production_plan_blue_print", __name__)
production_plan_blue_print.route("/production_plan", methods=["POST"])(
    get_production_plan
)

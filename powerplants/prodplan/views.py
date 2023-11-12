import json

from django.http import HttpResponse, JsonResponse
from django.utils.decorators import method_decorator
from django.views import View
from django.views.decorators.csrf import csrf_exempt

from prodplan.prodplan_calculator import ProductionPlanCalculator

from prodplan.prodplan_calculator import ManualInterventionNeeded


@method_decorator(csrf_exempt, name='dispatch')
class ProductionPlan(View):
    def get(self, request, *args, **kwargs):
        return HttpResponse("POST a correct payload to get the production plan.")

    def post(self, request, *args, **kwargs):
        data = json.loads(request.body)
        prodplan_calculator = ProductionPlanCalculator(**data)
        try:
            result = prodplan_calculator.get_production_plan()
        except ManualInterventionNeeded:
            result = {"error": "Failed to calculate production plan automatically. "
                      "Remaining load exceeds next powerplant min power. "
                      "Some cheaper plants might need to be switched off."}
        return JsonResponse(result, safe=False)

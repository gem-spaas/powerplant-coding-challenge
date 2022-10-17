from django.urls import path

from powerplant_challenge.views import ProductionPlanView, monitor


urlpatterns = [
    path("productionplan", ProductionPlanView.as_view(), name="production_plan"),
    path("monitor", monitor, name="monitor"),
]

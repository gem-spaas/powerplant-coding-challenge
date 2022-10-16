from django.urls import path

from powerplant_challenge.views import ProductionPlanView


urlpatterns = [path("productionplan", ProductionPlanView.as_view(), name="production_plan")]

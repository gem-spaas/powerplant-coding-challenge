from django.urls import path, include

from . import views

urlpatterns = [
    path('productionplan', views.ProductionPlan.as_view(), name="productionplan"),
]
from powerplant_challenge.consumers import PlanConsumer
from django.urls import re_path

websocket_urlpatterns = [
    re_path(r"websocket", PlanConsumer.as_asgi()),
]

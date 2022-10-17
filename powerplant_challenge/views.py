# -*- coding: utf-8 -*-

from rest_framework import status
from rest_framework.response import Response
from rest_framework.views import APIView
from asgiref.sync import async_to_sync
import json

from powerplant_challenge.exceptions import UnProcessableRequest
from powerplant_challenge.resolvers import ProductionPlanResolver
from powerplant_challenge.serializers import ProductionPlanSerializer
from django.shortcuts import render


def plan2dict(plan):
    return [{"name": plant.name, "p": plant.power_supply} for plant in plan]


import channels.layers

channel_layer = channels.layers.get_channel_layer()


class ProductionPlanView(APIView):
    def post(self, request):
        serializer = ProductionPlanSerializer(data=request.data)
        if serializer.is_valid():
            planner = ProductionPlanResolver(serializer.validated_data)
            if sum(p.power_supply for p in planner.plan) != planner.load:
                raise UnProcessableRequest()
            return Response(plan2dict(planner.plan), status=status.HTTP_200_OK)
        return Response(serializer.errors, status=status.HTTP_400_BAD_REQUEST)

    def finalize_response(self, request, response, *args, **kwargs):
        res = super(ProductionPlanView, self).finalize_response(request, response, *args, **kwargs)
        response.render()
        async_to_sync(channel_layer.group_send)(
            "test",
            {
                "type": "plan_event",
                "message": response.content,
            },
        )

        return res


def monitor(request):
    return render(request, "monitor.html")

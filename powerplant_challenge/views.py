# -*- coding: utf-8 -*-

from rest_framework import status
from rest_framework.response import Response
from rest_framework.views import APIView

from powerplant_challenge.exceptions import UnProcessableRequest
from powerplant_challenge.resolvers import ProductionPlanResolver
from powerplant_challenge.serializers import ProductionPlanSerializer


def plan2dict(plan):
    return [{"name": plant.name, "p": plant.power_supply} for plant in plan]


class ProductionPlanView(APIView):
    def post(self, request):
        serializer = ProductionPlanSerializer(data=request.data)
        if serializer.is_valid():
            planner = ProductionPlanResolver(serializer.validated_data)
            if sum(p.power_supply for p in planner.plan) != planner.load:
                raise UnProcessableRequest()
            return Response(plan2dict(planner.plan), status=status.HTTP_200_OK)
        return Response(serializer.errors, status=status.HTTP_400_BAD_REQUEST)

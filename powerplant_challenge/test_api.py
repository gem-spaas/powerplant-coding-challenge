# -*- coding: utf-8 -*-
import os
import json

from django.urls import reverse
from rest_framework.test import APISimpleTestCase

THIS_DIR = os.path.dirname(__file__)
FIXTURES_DIR = os.path.join(THIS_DIR, "..", "fixtures")


class PowerPlanTestCase(APISimpleTestCase):
    maxDiff = None

    def test_payload1(self):
        with open(os.path.join(FIXTURES_DIR, f"payload1.json")) as payload:
            res = self.client.post(reverse("production_plan"), data=json.loads(payload.read()), format="json")
            self.assertEquals(
                json.loads(res.content),
                [
                    {"name": "windpark2", "p": 21.6},
                    {"name": "windpark1", "p": 90.0},
                    {"name": "gasfiredbig1", "p": 368.4},
                    {"name": "gasfiredbig2", "p": 0.0},
                    {"name": "gasfiredsomewhatsmaller", "p": 0.0},
                    {"name": "tj1", "p": 0.0},
                ],
            )

    def test_payload2(self):
        with open(os.path.join(FIXTURES_DIR, f"payload2.json")) as payload:
            res = self.client.post(reverse("production_plan"), data=json.loads(payload.read()), format="json")
            self.assertEquals(
                json.loads(res.content),
                [
                    {"name": "windpark1", "p": 0.0},
                    {"name": "windpark2", "p": 0.0},
                    {"name": "gasfiredbig1", "p": 380.0},
                    {"name": "gasfiredbig2", "p": 100},
                    {"name": "gasfiredsomewhatsmaller", "p": 0.0},
                    {"name": "tj1", "p": 0.0},
                ],
            )

    def test_payload3(self):
        with open(os.path.join(FIXTURES_DIR, f"payload3.json")) as payload:
            res = self.client.post(reverse("production_plan"), data=json.loads(payload.read()), format="json")
            self.assertEquals(
                json.loads(res.content),
                [
                    {"name": "windpark2", "p": 21.6},
                    {"name": "windpark1", "p": 90.0},
                    {"name": "gasfiredbig1", "p": 460},
                    {"name": "gasfiredbig2", "p": 338.4},
                    {"name": "gasfiredsomewhatsmaller", "p": 0.0},
                    {"name": "tj1", "p": 0.0},
                ],
            )

    def test_invalid_payload(self):
        with open(os.path.join(FIXTURES_DIR, f"invalid_payload.json")) as payload:
            res = self.client.post(reverse("production_plan"), data=json.loads(payload.read()), format="json")
            self.assertEquals(
                json.loads(res.content),
                {
                    "load": ["A valid integer is required."],
                    "fuels": {
                        "gas(euro/MWh)": ["A valid number is required."],
                        "wind(%)": ["Ensure this value is less than or equal to 100."],
                    },
                    "powerplants": [
                        {"type": ["This field may not be null."]},
                        {"name": ["This field may not be null."]},
                        {"efficiency": ["A valid number is required."]},
                        {"pmin": ["A valid integer is required."]},
                        {"pmax": ["A valid integer is required."]},
                    ],
                },
            )

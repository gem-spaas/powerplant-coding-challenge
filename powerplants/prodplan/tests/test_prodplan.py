import json

from django.test import TestCase
from django.urls import reverse

from powerplants.settings import BASE_DIR


class ProductionPlanViewTestCase(TestCase):
    def test_prodplan(self):
        url = reverse('productionplan')
        with open(BASE_DIR / '../example_payloads/payload3.json', 'r') as f:
            payload = json.loads(f.read())
        response = self.client.post(url, json.dumps(payload), content_type="application/json")
        with open(BASE_DIR / '../example_payloads/response3.json', 'r') as f:
            expected_response = json.loads(f.read())
        self.assertEqual(response.json(), expected_response)

        with open(BASE_DIR / '../example_payloads/payload1.json', 'r') as f:
            payload = json.loads(f.read())
        response = self.client.post(url, json.dumps(payload), content_type="application/json")
        with open(BASE_DIR / '../example_payloads/response1.json', 'r') as f:
            expected_response = json.loads(f.read())
        self.assertEqual(response.json(), expected_response)

        with open(BASE_DIR / '../example_payloads/payload2.json', 'r') as f:
            payload = json.loads(f.read())
        response = self.client.post(url, json.dumps(payload), content_type="application/json")
        self.assertIn('error', response.json())

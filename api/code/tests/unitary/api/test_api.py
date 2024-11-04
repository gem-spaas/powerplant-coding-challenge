import random
import unittest

from fastapi.testclient import TestClient

from config import appconfig
from run_server import app

client = TestClient(app)


class ForwardToTestCase(unittest.TestCase):
    def test_update_job_not_matching_payload(self):
        job_id = random.randrange(1, 9999)
        code = random.randrange(1, 700)
        job = {
            "jobID": f"{job_id}",
            "code": code,
            "message": "Job Completed without errors or warnings",
            "status": {
                "success": 10,
                "errors": 0,
                "warnings": 0,
                "added": 0,
                "updated": 0,
            },
            "errors": [],
            "warnings": [],
        }
        response = client.post("/jobs/3333333", json=job)
        self.assertEqual(response.status_code, 400)

    def test_update_invalid_payload_schema(self):
        job_id = random.randrange(1, 9999)
        code = random.randrange(1, 700)
        job = {
            "asdasdas": f"{job_id}",
            "code": code,
            "message": "Job Completed without errors or warnings",
            "status": {
                "success": 10,
                "errors": 0,
                "warnings": 0,
                "added": 0,
                "updated": 0,
            },
            "errors": [],
            "warnings": [],
        }
        response = client.post(f"/jobs/{job_id}", json=job)
        self.assertEqual(response.status_code, 422)

    def test_update_job_one_success_post(self):
        job_id = random.randrange(1, 9999)
        code = random.randrange(1, 700)
        job = {
            "jobID": f"{job_id}",
            "code": code,
            "message": "Job Completed without errors or warnings",
            "status": {
                "success": 10,
                "errors": 0,
                "warnings": 0,
                "added": 0,
                "updated": 0,
            },
            "errors": [],
            "warnings": [],
        }
        response = client.post(f"/jobs/{job_id}", json=job)
        self.assertEqual(response.status_code, 201)

    def test_update_job_one_fail_on_put(self):
        response = client.put(
            "/jobs/1",
            json={
                "jobID": "1",
                "code": 3,
                "message": "Job Completed without errors or warnings",
                "status": {
                    "success": 10,
                    "errors": 0,
                    "warnings": 0,
                    "added": 0,
                    "updated": 0,
                },
                "errors": [],
                "warnings": [],
            },
        )
        self.assertEqual(response.status_code, 405)

    def test_health_invalid_request(self):
        response = client.put("/healthz")
        self.assertEqual(response.status_code, 405)

    def test_health_success(self):
        response = client.get("/healthz")
        self.assertEqual(response.status_code, 200)

    def test_version_invalid_request(self):
        response = client.put("/version")
        self.assertEqual(response.status_code, 405)

    def test_version_success(self):
        response = client.get("/version")
        self.assertEqual(response.status_code, 200)


if __name__ == "__main__":
    unittest.main()

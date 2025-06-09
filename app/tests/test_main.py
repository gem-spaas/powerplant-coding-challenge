# Third-party imports
from fastapi.testclient import TestClient
from app.main import app  # Adjust the import path according to your project structure

client = TestClient(app)

def test_root():
    """
    Test the root endpoint returns status 200 and expected JSON message.
    """
    response = client.get("/")
    assert response.status_code == 200
    assert response.json() == {"message": "API is active"}

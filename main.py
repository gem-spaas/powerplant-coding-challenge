from pathlib import Path
import json

from src.models import Payload
from src.planner import calculate_production_plan
from src.validators import validate_payload

# Get the absolute path of the JSON file
BASE_DIR = Path(__file__).resolve().parent
PAYLOAD_PATH = BASE_DIR / "example_payloads" / "payload1.json"

if __name__ == "__main__":
    with open(PAYLOAD_PATH, "r") as f:
        data = json.load(f)

    validate_payload(data)
    payload = Payload(
        load=data["load"],
        fuels=data["fuels"],
        powerplants=data["powerplants"]
    )

    result = calculate_production_plan(payload.load, payload.fuels, payload.powerplants)
    for r in result:
        print(r.to_dict())
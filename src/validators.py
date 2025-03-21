from src.models import PowerPlant
from fastapi import HTTPException

def validate_payload(payload_json: dict) -> None:
    required_keys = ["load", "fuels", "powerplants"]
    for key in required_keys:
        if key not in payload_json:
            raise HTTPException(status_code=400, detail=f"Missing key: {key}")

    if not isinstance(payload_json["load"], (int, float)) or payload_json["load"] <= 0:
        raise HTTPException(status_code=400, detail="'load' must be a positive number")

    if not isinstance(payload_json["fuels"], dict):
        raise HTTPException(status_code=400, detail="'fuels' must be a dictionary")

    if not isinstance(payload_json["powerplants"], list) or not payload_json["powerplants"]:
        raise HTTPException(status_code=400, detail="'powerplants' must be a non-empty list")

    for pp in payload_json["powerplants"]:
        try:
            plant = PowerPlant(**pp)
            if plant.pmin > plant.pmax:
                raise ValueError("pmin cannot be greater than pmax")
        except Exception as e:
            raise HTTPException(status_code=400, detail=f"Invalid powerplant: {str(e)}")

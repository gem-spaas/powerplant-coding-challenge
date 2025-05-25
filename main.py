from flask import Flask, request, jsonify

# ----------  LÓGICA DEL PRODUCTION PLAN  ---------- #
def merit_order(plants, fuels):
    def cost(plant):
        if plant["type"] == "gasfired":
            fuel_cost = fuels["gas(euro/MWh)"] / plant["efficiency"]
        elif plant["type"] == "turbojet":
            fuel_cost = fuels["kerosine(euro/MWh)"] / plant["efficiency"]
        else:  # windturbine
            return 0
        co2_cost = fuels["co2(euro/ton)"] * 0.3  # 0.3 t CO2 per MWh
        return fuel_cost + co2_cost

    return sorted(
        [p for p in plants if p["type"] != "windturbine"],
        key=cost
    )

def compute_plan(payload):
    # Validación simple
    if not all(k in payload for k in ("load", "fuels", "powerplants")):
        raise ValueError("JSON missing required keys")

    load = payload["load"]
    fuels = payload["fuels"]
    plants_in = payload["powerplants"]

    result = []
    remaining = load

    # 1) EÓLICAS – producen gratis: p = pmax × wind%
    wind_pct = fuels.get("wind(%)", 0) / 100
    for p in plants_in:
        if p["type"] == "windturbine":
            power = round(p["pmax"] * wind_pct, 1)
            result.append({"name": p["name"], "p": power})
            remaining -= power

    # 2) PLANTAS TÉRMICAS en orden de mérito (coste creciente)
    for p in merit_order(plants_in, fuels):
        if remaining <= 0:
            result.append({"name": p["name"], "p": 0})
            continue

        power = max(p["pmin"], min(p["pmax"], remaining))
        power = round(power, 1)
        result.append({"name": p["name"], "p": power})
        remaining -= power

    # 3) Ajuste final si sobra o falta muy poco
    if abs(remaining) >= 0.05:
        for entry in sorted(result, key=lambda x: x["p"]):
            if entry["p"] > 0.1:
                entry["p"] = round(entry["p"] + remaining, 1)
                break

    return result


# ----------  API FLASK  ---------- #
app = Flask(__name__)

@app.route("/productionplan", methods=["POST"])
def productionplan_endpoint():
    payload = request.get_json()
    try:
        plan = compute_plan(payload)
        return jsonify(plan)
    except Exception as e:
        return jsonify({"error": str(e)}), 500

if __name__ == "__main__":
    app.run(port=8888)

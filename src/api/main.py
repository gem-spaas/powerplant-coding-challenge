from flask import Flask, request, jsonify
import logging
from typing import List, Dict
from dataclasses import dataclass

# Set up logging
logging.basicConfig(level=logging.INFO)

app = Flask(__name__)

@dataclass
class Plant:
    #Represents each Powerplant payload and its datatype.
    type: str
    efficiency: float
    name: str
    pmin: float
    pmax: float

@dataclass
class ProductionPlanRequest:
    #Represents the input data for a production plan calculation and its datatype.
    load: float
    fuels: Dict[str, float]
    powerplants: List[Dict]  

def production_cost(plant_type: str, efficiency: float, fuel_prices: dict) -> float:
    #Calculate the production cost for a given PowerPlant type.
    if plant_type == "windturbine":
        return 0
    fuel_type = "gas(euro/MWh)" if plant_type == "gasfired" else "kerosine(euro/MWh)"
    return fuel_prices.get(fuel_type, 0) / efficiency

def calculate_power_output(plant: Plant, remaining_load: float, fuels: dict) -> float:
    #Calculate the PowerPlant capacity based on remaining load and fuel data.
    if plant.type == "windturbine":
        wind_percentage = fuels.get("wind(%)", 0) / 100
        return min(plant.pmax * wind_percentage, remaining_load)
    
    if remaining_load <= 0:
        return 0.0
        
    if plant.type in ["gasfired", "turbojet"]:
        return min(max(plant.pmin, remaining_load), plant.pmax)
        
    return 0.0

@app.route('/productionplan', methods=['POST'])
def calculate_costs():
    #API endpoint to handle POSTs requests and calculate the production plan.
    try:
        request_data = request.get_json()
        if not request_data:
            return jsonify({"error": "Invalid JSON input"}), 400

        production_plan_request = ProductionPlanRequest(**request_data)
        plants = [Plant(**plant_data) for plant_data in production_plan_request.powerplants]
        results = []
        remaining_load = production_plan_request.load
        
        # Process plants in merit-order: wind, gas, turbojet
        plant_types_order = ["windturbine", "gasfired", "turbojet"]
        plant_order = {plant.name: i for i, plant in enumerate(plants)}
        
        for plant_type in plant_types_order:
            for plant in (p for p in plants if p.type == plant_type):
                cost = production_cost(plant.type, plant.efficiency, production_plan_request.fuels)
                power = calculate_power_output(plant, remaining_load, production_plan_request.fuels)
                
                remaining_load = max(0, remaining_load - power)
                
                results.append({
                    "name": plant.name,
                    "p": round(power, 1)
                })
                
                logging.info(f"Plant: {plant.name}, Type: {plant.type}, Output: {power:.1f} MW, Cost: {cost:.2f} €/MWh")
        
        results.sort(key=lambda x: plant_order[x["name"]])
        
        return jsonify(results)

    #Error handling and Loggings.
    except (TypeError, KeyError) as e:
        logging.error(f"Invalid input data: {str(e)}")
        return jsonify({"error": "Invalid input data"}), 400
    except Exception as e:
        logging.error(f"Error processing request: {str(e)}")
        return jsonify({"error": "Internal server error"}), 500

#App run if payload ok
if __name__ == '__main__':
     app.run(host='0.0.0.0', port=8888, debug=True)
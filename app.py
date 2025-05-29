from flask import Flask, request, jsonify
import logging
from typing import List, Dict, Any
import math

# Configure logging
logging.basicConfig(level=logging.INFO)
logger = logging.getLogger(__name__)

app = Flask(__name__)

class PowerPlant:
    def __init__(self, name: str, type: str, efficiency: float, pmin: int, pmax: int):
        self.name = name
        self.type = type
        self.efficiency = efficiency
        self.pmin = pmin
        self.pmax = pmax
        self.cost_per_mwh = 0.0
        self.available_power = 0
    
    def calculate_cost(self, gas_price: float, kerosine_price: float, co2_price: float, wind_percentage: float):
        """Calculate the cost per MWh for this power plant"""
        if self.type == "windturbine":
            self.cost_per_mwh = 0.0
            self.available_power = int(self.pmax * wind_percentage / 100)
        elif self.type == "gasfired":
            # Cost = fuel_cost / efficiency + CO2_cost
            fuel_cost_per_mwh = gas_price / self.efficiency
            co2_cost_per_mwh = (0.3 * co2_price) / self.efficiency  # 0.3 ton CO2 per MWh
            self.cost_per_mwh = fuel_cost_per_mwh + co2_cost_per_mwh
            self.available_power = self.pmax
        elif self.type == "turbojet":
            fuel_cost_per_mwh = kerosine_price / self.efficiency
            self.cost_per_mwh = fuel_cost_per_mwh
            self.available_power = self.pmax
    
    def __repr__(self):
        return f"PowerPlant({self.name}, cost={self.cost_per_mwh:.2f}, available={self.available_power})"

class ProductionPlanCalculator:
    def __init__(self):
        self.plants = []
    
    def calculate_production_plan(self, payload: Dict[str, Any]) -> List[Dict[str, Any]]:
        """Calculate the production plan based on merit order optimization"""
        try:
            load = payload['load']
            fuels = payload['fuels']
            powerplants_data = payload['powerplants']
            
            # Create PowerPlant objects and calculate costs
            self.plants = []
            for plant_data in powerplants_data:
                plant = PowerPlant(
                    plant_data['name'],
                    plant_data['type'],
                    plant_data['efficiency'],
                    plant_data['pmin'],
                    plant_data['pmax']
                )
                plant.calculate_cost(
                    fuels['gas(euro/MWh)'],
                    fuels['kerosine(euro/MWh)'],
                    fuels['co2(euro/ton)'],
                    fuels['wind(%)']
                )
                self.plants.append(plant)
            
            # Sort plants by merit order (cost per MWh, wind first)
            self.plants.sort(key=lambda p: (p.cost_per_mwh, -p.available_power))
            
            logger.info(f"Merit order: {[f'{p.name}({p.cost_per_mwh:.2f})' for p in self.plants]}")
            
            # Calculate production plan
            production_plan = self._optimize_production(load)
            
            return production_plan
            
        except Exception as e:
            logger.error(f"Error calculating production plan: {str(e)}")
            raise
    
    def _optimize_production(self, target_load: int) -> List[Dict[str, Any]]:
        """Optimize production using a greedy approach with merit order"""
        remaining_load = target_load
        production_plan = []
        
        # Initialize all plants with 0 production
        for plant in self.plants:
            production_plan.append({"name": plant.name, "p": 0})
        
        # First pass: Use wind turbines to their full available capacity
        for i, plant in enumerate(self.plants):
            if plant.type == "windturbine" and remaining_load > 0:
                production = min(plant.available_power, remaining_load)
                # Round to nearest 0.1 MW
                production = round(production * 10) / 10
                production_plan[i]["p"] = production
                remaining_load -= production
                logger.info(f"Wind: {plant.name} produces {production} MW, remaining load: {remaining_load}")
        
        # Second pass: Use conventional plants in merit order
        plant_indices = list(range(len(self.plants)))
        
        while remaining_load > 0.1:  # Continue until load is satisfied (within 0.1 MW tolerance)
            made_progress = False
            
            for i in plant_indices:
                plant = self.plants[i]
                current_production = production_plan[i]["p"]
                
                # Skip wind turbines (already handled) and plants at max capacity
                if plant.type == "windturbine" or current_production >= plant.pmax:
                    continue
                
                # Calculate how much more this plant can produce
                if current_production == 0:
                    # Plant is off, need to consider pmin
                    min_increment = max(plant.pmin, 0.1)
                    max_possible = min(plant.pmax, remaining_load + current_production)
                else:
                    # Plant is already running, can increment by 0.1
                    min_increment = 0.1
                    max_possible = min(plant.pmax, remaining_load + current_production)
                
                if max_possible >= min_increment:
                    # Calculate optimal increment
                    if current_production == 0:
                        increment = min(remaining_load, plant.pmax)
                        increment = max(increment, plant.pmin)
                    else:
                        increment = min(remaining_load, plant.pmax - current_production)
                    
                    # Round to nearest 0.1 MW
                    increment = round(increment * 10) / 10
                    
                    if increment >= 0.1:
                        production_plan[i]["p"] = round((current_production + increment) * 10) / 10
                        remaining_load = round((remaining_load - increment) * 10) / 10
                        made_progress = True
                        logger.info(f"{plant.name} produces {production_plan[i]['p']} MW, remaining load: {remaining_load}")
                        
                        if remaining_load <= 0.1:
                            break
            
            if not made_progress:
                logger.warning(f"Could not satisfy remaining load of {remaining_load} MW")
                break
        
        # Final adjustment to exactly match the load
        total_production = sum(item["p"] for item in production_plan)
        difference = target_load - total_production
        
        if abs(difference) > 0.1:
            logger.warning(f"Production difference: {difference} MW")
            # Try to adjust the last active plant
            for i in reversed(range(len(production_plan))):
                if production_plan[i]["p"] > 0:
                    plant = self.plants[i]
                    new_production = production_plan[i]["p"] + difference
                    if plant.pmin <= new_production <= plant.pmax:
                        production_plan[i]["p"] = round(new_production * 10) / 10
                        break
        
        return production_plan

calculator = ProductionPlanCalculator()

@app.route('/productionplan', methods=['POST'])
def production_plan():
    """REST endpoint to calculate production plan"""
    try:
        if not request.is_json:
            return jsonify({"error": "Request must be JSON"}), 400
        
        payload = request.get_json()
        
        # Validate payload
        if not payload or 'load' not in payload or 'fuels' not in payload or 'powerplants' not in payload:
            return jsonify({"error": "Invalid payload structure"}), 400
        
        logger.info(f"Received request for load: {payload['load']} MW")
        
        # Calculate production plan
        production_plan = calculator.calculate_production_plan(payload)
        
        # Validate result
        total_production = sum(item["p"] for item in production_plan)
        logger.info(f"Total production: {total_production} MW, Target load: {payload['load']} MW")
        
        return jsonify(production_plan)
        
    except Exception as e:
        logger.error(f"Error processing request: {str(e)}")
        return jsonify({"error": "Internal server error"}), 500

@app.route('/health', methods=['GET'])
def health_check():
    """Health check endpoint"""
    return jsonify({"status": "healthy"})

@app.errorhandler(404)
def not_found(error):
    return jsonify({"error": "Endpoint not found"}), 404

@app.errorhandler(500)
def internal_error(error):
    logger.error(f"Internal server error: {str(error)}")
    return jsonify({"error": "Internal server error"}), 500

if __name__ == '__main__':
    logger.info("Starting Power Plant Production Plan API on port 8888")
    app.run(host='0.0.0.0', port=8888, debug=False)
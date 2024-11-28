import json
from flask import Flask, request, jsonify
import logging

app = Flask(__name__)
#Set up logging
logging.basicConfig(level=logging.INFO)

@app.route('/production_plan',methods=['POST'])

def production_plan():
    try:
        # Import Data inputs
        data = request.json
        complete_load = data.get('load')
        fuels = data.get('fuels')
        powerplants_list = data.get('powerplants')

        if complete_load is None or fuels is None or powerplants_list is None:
            return "Error : Load, fuels or powerplants is missing"

        logging.info(f"Calculating production plan for load = {complete_load} ")

        #Call the function to calculate the production plan
        res = execute_production_plan(complete_load, fuels, powerplants_list)

        #Save the results in a JSON File
        with open('production_plan_results.json', 'w') as json_file:
            json.dump(res, json_file,indent=4)

        return jsonify(res),200
    except Exception as error:
        logging.error(f"An error as occured : {str(error)}")
        return jsonify({"error": str(error)}),500



def execute_production_plan(complete_load, fuels, powerplants_list):
    # Go throught the powerplant list
    for powerplant in powerplants_list:
        print(powerplant)
        if powerplant['type'] == 'gasfired':
            fuel_cost = fuels['gas(euro/MWh)'] / powerplant['efficiency']
            cost_co2 = 0.3 * fuels['co2(euro/ton)']
            cost = fuel_cost + cost_co2
        elif powerplant['type'] == 'turbojet':
            cost = fuels['kerosine(euro/MWh)'] / powerplant['efficiency']
        elif powerplant['type'] == 'windturbine':
            cost = 0
        #We add the cost
        powerplant['cost'] = cost

    # Now lets see the merit-order sorting all the powerplants by cost
    powerplant_sorted = sorted(powerplants_list,key=lambda plant:plant['cost'])

    prod_plan = []
    current_load = complete_load

    #Wind allocation

    for powerplant in powerplant_sorted:
        print(current_load)
        if current_load <= 0:
            break
        max_ouput = powerplant['pmax']
        if powerplant['type'] == 'windturbine':
            wind_output = min(max_ouput * (fuels.get('wind(%)',0) / 100),current_load)
            produced_power = round(wind_output,1) #round to 0.1MW
            prod_plan.append({
                "name": powerplant['name'],
                "type": powerplant['type'],
                "efficiency": powerplant['efficiency'],
                "pmax": powerplant['pmax'],
                "pmin": powerplant['pmin'],
                "cost": powerplant['cost'],
                "p": produced_power
            })
            current_load = current_load - produced_power
        else:
            power_generate = min(current_load,max_ouput) # To be sure to respect the constraint of the powerplant

            #Make sure we use at least pmin and adjust for multiple of 0.1MW
            power_generate = max(powerplant['pmin'],round(power_generate/0.1)*0.1)
            power_generate = round(power_generate,1)

            #If the power generate is between pmin and pmax
            if power_generate >= powerplant['pmin'] and power_generate <= powerplant['pmax']:
                prod_plan.append({
                    "name": powerplant['name'],
                    "type": powerplant['type'],
                    "efficiency": powerplant['efficiency'],
                    "pmax": powerplant['pmax'],
                    "pmin": powerplant['pmin'],
                    "cost": powerplant['cost'],
                    "p": round(power_generate,1)
                })
                current_load = current_load - power_generate
            else:
                prod_plan.append({
                    "name": powerplant['name'],
                    "type": powerplant['type'],
                    "efficiency": powerplant['efficiency'],
                    "pmax": powerplant['pmax'],
                    "pmin": powerplant['pmin'],
                    "cost": powerplant['cost'],
                    "p": 0.0
                })
    if current_load < 0:
        for element in reversed(prod_plan):
            if current_load >=0:
                break

            #How much to reduce ?
            current_power_prod = element['p']
            reducible_power = current_power_prod - element['pmin']
            if reducible_power > 0 :
                #How much can be reduced ?
                to_reduce = min(reducible_power,-current_load)
                element['p'] = element['p'] - to_reduce
                current_load = current_load + to_reduce

    #Reactivate remaining powerplants if current_load > pmin

    for powerplant in powerplant_sorted:
        if current_load >= powerplant['pmin']:
            #Check if the powerplant can generate additionnal power
            for element in prod_plan:
                if element['name'] == powerplant['name'] and element['p'] < powerplant['pmax']:
                    add_power = min(powerplant['pmax'] - element['p'],current_load)
                    element['p'] = element['p'] + add_power
                    current_load = current_load - add_power
                    break

    for powerplant in powerplants_list:
        if not any(p['name'] == powerplant['name'] for p in prod_plan):
            prod_plan.append({
                "name": powerplant['name'],
                "type": powerplant['type'],
                "efficiency": powerplant['efficiency'],
                "pmax": powerplant['pmax'],
                "pmin": powerplant['pmin'],
                "cost": powerplant['cost'],
                "p": 0.0
            })
    return {"Production plan": prod_plan}


with open('payload3.json','r') as f:
    payload = json.load(f)
complete_load = payload.get('load')
fuels = payload.get('fuels')
power_plant_list = payload.get('powerplants')
results = execute_production_plan(complete_load,fuels,power_plant_list)
print(results)


if __name__ == '__main__':
    app.run(port=8888,debug=True)
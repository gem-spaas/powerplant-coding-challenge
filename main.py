from flask import Flask, jsonify, request

app = Flask(__name__)

@app.route('/productionplan', methods=['POST'])
def production_plan():

    data = request.get_json()

    loads = data['load']
    resp =[]
    power_by_wind = 0
    data['powerplants']
    for element in data['powerplants']:
        if element['type'] == 'windturbine':
            wind_moy_pow = element['pmax']/2 # due to pmin = 0 if no wind
            power_by_wind = element['efficiency']* wind_moy_pow * (data['fuels']['wind(%)']/100)
            sub_answ = {'name': element['name'], 'p': power_by_wind}
            resp.append(sub_answ)
        loads = loads -power_by_wind
        #free energies produced was removed from the total
    
    # calculate the order of priority for each energies
    merit_list = []
    for element in data['powerplants']:
        moy_pui = element['pmin']+element['pmax']/2
        if(element['type'] == 'gasfired'):
            merit = (data['fuels']['gas(euro/MWh)'] * moy_pui)/ element['efficiency']
            merit_list.append({'merit': merit, 'name':element['name']})
        elif(element['type'] == 'turbojet'):
            merit = (data['fuels']['kerosine(euro/MWh)'] * moy_pui)/ element['efficiency']
            merit_list.append({'merit': merit, 'name':element['name']})
    merit_order = sorted(merit_list, key=lambda k: k['merit'])
    
    #calcul power for each powerplant
    for powerplant in merit_order:
        if loads <= 0:
            break
        for element in data['powerplants']:
            if element['name'] == powerplant['name']:
                if loads >= element['pmax']:
                    loads -= element['pmax']
                    sub_answ = {'name': element['name'], 'p': element['pmax']}
                    resp.append(sub_answ)
                elif loads < element['pmax'] and loads >= element['pmin']:
                    sub_answ = {'name': element['name'], 'p': loads}
                    resp.append(sub_answ)
                    loads = 0
                else:
                    continue
    print(resp)
    #now calculate power for each one
    
    return jsonify(resp)

if __name__ == '__main__':
    app.run(port=8888)
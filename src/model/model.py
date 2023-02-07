__version__ = '0.1.0'

def energy_plan(data):

    load = load_left = data.load
    fuels = data.fuels
    powerplants = data.powerplants

    powerplant_type_cost = {'gasfired': fuels.get('gas(euro/MWh)'), 'turbojet': fuels.get('kerosine(euro/MWh)'), 'windturbine': fuels.get('wind(%)')}
    for powerplant in powerplants:
        if powerplant.get('type') == 'windturbine':
            powerplant['pmax'] *= powerplant_type_cost['windturbine']/100
        powerplant['MWH_cost'] = powerplant_type_cost.get(powerplant.get('type'))/powerplant.get('efficiency')
        powerplant['rank'] = powerplant.get('MWH_cost') * (powerplant.get('pmin') + 1) / powerplant.get('pmax')
        powerplant['p'] = 0

    powerplants.sort(key=lambda x: x.get('rank'))

    for i in range(len(powerplants)):
        
        if load_left < powerplants[i].get('pmin'):
            powerplants[i]['p'] = powerplants[i].get('pmin')
            load_left -= powerplants[i].get('p')
            load_over = abs(load_left)
            for j in range(i-1, -1, -1):
                if powerplants[j].get('p') - load_over >= powerplants[j].get('pmin'):
                    powerplants[j]['p'] -= load_over
                    break
                amount = powerplants[j].get('p') - powerplants[j].get('pmin')
                powerplants[j]['p'] = powerplants[j].get('pmin')
                load_over -= amount
            break
            
        if powerplants[i].get('pmin') <= load_left <= powerplants[i].get('pmax'):
            powerplants[i]['p'] = load_left
            break
            
        powerplants[i]['p'] = powerplants[i].get('pmax')
        load_left -= powerplants[i].get('p')

    return [{'name': powerplant.get('name'), 'p': powerplant.get('p')} for powerplant in powerplants]
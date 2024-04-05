from Models.PowerPlantModel import PowerPlant
import logging

logger = logging.getLogger(__name__)
logging.basicConfig(filename='engie_coding_challenge.log', encoding='utf-8', level=logging.DEBUG)

def createPowerPlant(payload):
    try:
        listPlant = []
        for item in payload:
            powerplant = PowerPlant(item["name"], item["type"], item["efficiency"], item["pmin"], item["pmax"])
            listPlant.append(powerplant)
        return listPlant
    except Exception:
        logger.error(f"createPowerPlant : {Exception}")
        return(f"Services/PowerPlantService.py/createPowerPlant : {Exception}")

def sortList(list):
    return sorted(list, key=lambda pw_plant: (pw_plant.efficiency, pw_plant.pmax), reverse=True)

def handleLoad(load, listPlant):
    for powerplant in listPlant:
        if(powerplant.pmax < load):
            load = load - powerplant.pmax
        else:
            powerplant.pmax = load
            load = 0
    return load

def createResponseJson(listPlant):
    responseJson = []
    for powerplant in listPlant:
        responseJson.append({
            "name": powerplant.name,
            "p": powerplant.pmax
        })
    return responseJson

def handlePowerPlant(listPlant, load, gasPrice, kerosinePrice, windPourcent):
    try:
        listWindTurbine = []
        listGasFired = []
        listTurboJet = []

        for powerplant in listPlant:
            if(powerplant.type == "windturbine"):
                powerplant.pmax = round(powerplant.pmax * (windPourcent/100), 1)
                listWindTurbine.append(powerplant)
            if(powerplant.type == "turbojet"):
                listTurboJet.append(powerplant)
            if(powerplant.type == "gasfired"):
                listGasFired.append(powerplant)

        listWindTurbine = sortList(listWindTurbine)
        load = handleLoad(load, listWindTurbine)

        if(gasPrice <= kerosinePrice):
            listGasFired = sortList(listGasFired)
            load = handleLoad(load, listGasFired)

            listTurboJet = sortList(listTurboJet)
            handleLoad(load, listTurboJet)
        else:
            listTurboJet = sortList(listTurboJet)
            load = handleLoad(load, listTurboJet)

            listGasFired = sortList(listGasFired)
            handleLoad(load, listGasFired)

        allPlants = listWindTurbine
        allPlants.extend(listGasFired)
        allPlants.extend(listTurboJet)

        return createResponseJson(allPlants)
    except Exception:
        logger.error(f"handlePowerPlant : {Exception}")
        return(f"Services/PowerPlantService.py/handlePowerPlant : {Exception}")
    
def handleEnterJson(data):
    try:
        load = data["load"]
        gasPrice = data["fuels"]["gas(euro/MWh)"]
        kerosinePrice = data["fuels"]["kerosine(euro/MWh)"]
        windPourcent = data["fuels"]["wind(%)"]
        listPlants = createPowerPlant(data["powerplants"])
        return handlePowerPlant(listPlants, load, gasPrice, kerosinePrice, windPourcent)
    except Exception:
        logger.error(f"handleEnterJson : {Exception}")
        return(f"Services/PowerPlantService.py/handleEnterJson : {Exception}")




import json 

filename1 = "example_payloads/payload1.json"
filename2 = "example_payloads/payload2.json"
filename3 = "example_payloads/payload3.json"

total_load = 0
total_production = 0
final_price = 0

plant_plan = {}

def fusion(liste_a, liste_b) :
    if len(liste_a) == 0 :
        return liste_b
    if len(liste_b) == 0 :
        return liste_a
    if liste_a[0][1] >= liste_b[0][1] :
        return [liste_a[0]] + fusion(liste_a[1:],liste_b)
    else :
        return [liste_b[0]] + fusion(liste_a, liste_b[1:])

def tri_fusion(liste) :
    if len(liste) <= 1 :
        return liste
    else :
        middle = int(len(liste)/2)
        return fusion(tri_fusion(liste[:middle]),tri_fusion(liste[middle:]))

def init(file) :
    #datas = json.load(file)
    datas = file

    plant_info = {}
    global total_load
    global total_production
    global plant_plan
    plant_prod = {}
    plant_prod_max = {}
    
    total_load = datas["load"]

    fuel_prices = {}
    for fuel in datas["fuels"] :
        tmp = fuel.strip().split('(')[0]
        fuel_prices[tmp] = datas["fuels"][fuel]
    plant_info = datas["powerplants"]

    for (i,plant) in enumerate(datas["powerplants"]) :
        tmp = plant["type"]
        if tmp == "gasfired" :
            tmp = "gas"
        elif tmp == "windturbine" :
            tmp = "wind"
        elif tmp == "turbojet" :
            tmp = "kerosine"
        else :
            tmp = None
        plant_info[i]["type"] = tmp
        plant_info[i]["price_unit"] = fuel_prices[tmp]/plant_info[i]["efficiency"]
        plant_info[i]["prod"] = plant_info[i]["pmax"]
        if tmp == "wind" :
            plant_info[i]["price_unit"] = 0
            plant_info[i]["prod"] = (plant_info[i]["pmax"]*fuel_prices[tmp])/100
        plant_info[i]["price_total"] = plant_info[i]["price_unit"]*plant_info[i]["prod"]
        
        plant_plan[plant_info[i]["name"]] = {}
        plant_plan[plant_info[i]["name"]]["type"] = plant_info[i]["type"]
        plant_plan[plant_info[i]["name"]]["efficiency"] = plant_info[i]["efficiency"]
        plant_plan[plant_info[i]["name"]]["pmin"] = plant_info[i]["pmin"]
        plant_plan[plant_info[i]["name"]]["pmax"] = plant_info[i]["pmax"]
        plant_plan[plant_info[i]["name"]]["price_unit"] = plant_info[i]["price_unit"]
        plant_plan[plant_info[i]["name"]]["prod"] = plant_info[i]["prod"]
        plant_plan[plant_info[i]["name"]]["price_total"] = plant_info[i]["price_total"]

        total_production += plant_info[i]["prod"]
        
def plantToList(plant_list) :
    plant_list_ = []
    for info in plant_list :
        plant_list_.append([info, plant_list[info]['price_unit']])
    return plant_list_

def update(plant_list) :
    global total_production
    global plant_plan
    global final_price
    
    for (i,plant) in enumerate(plant_list) :
        diff = total_production - total_load
        name = plant[0]
        if diff <= 0 :
            assert(diff==0)
            if diff < 0 :
                print(", rejeter une erreur")
            break
            
        else :
            pmin = plant_plan[name]["pmin"]
            pdiff = plant_plan[name]["prod"] - plant_plan[name]["pmin"]

            if pdiff < diff :
                plant_plan[name]["prod"] = plant_plan[name]["pmin"]
                total_production -= pdiff

            else : 
                plant_plan[name]["prod"] -= diff
                total_production -= diff

            plant_plan[name]["price_total"] = plant_plan[name]["price_unit"]*plant_plan[name]["prod"]
            
    for name in plant_plan :
        plant_plan[name]["prod"] = round(plant_plan[name]["prod"],1)
        final_price += plant_plan[name]["price_total"]
        
            
def checkSum() :
    total_prod = 0
    for plant in plant_plan :
        total_prod += plant_plan[plant]["prod"]
    if total_prod == total_load :
        return True
    else :
        return False 

def visu() :
	print("<br/>")
	print("************ VISUALIZATION ***********")
	print("<br/>")
	print("<br/>")
	print("Total Load : " + str(total_load))
	print("<br/>")
	print("Total Production : " + str(total_production))
	print("<br/>")
	print("Total Price : " + str(final_price))
	print("<br/>")
	print("<br/>")
	print()
	print(plant_plan)
	print()
	print("<br/>")
	print("**************************************")


def resolve(file) : 
    init(file)
    #visu()
    plant_list = plantToList(plant_plan)
    plant_list_tri = tri_fusion(plant_list)
    update(plant_list_tri)
    visu()
    print("<br/>")
    print("checksum : " + str(checkSum()))
    print("<br/>")        
    return plant_plan
    

if __name__ == '__main__' :
    file1 = open(filename1)
    file2 = open(filename2)
    file3 = open(filename3)
    resolve(file1)
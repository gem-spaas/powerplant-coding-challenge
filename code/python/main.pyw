#-----------------------------------
# Imports
#-----------------------------------

from os import path

from typing import Optional
from http.server import BaseHTTPRequestHandler, HTTPServer
import json

from powerplant import PowerPlant
from iphysicfactor import IPhysicFactor
from windfactor import WindFactor

#-----------------------------------
# Constants
#-----------------------------------

INPUT_FILE: str = "./payload1.json"
"""This is the file that is supposed to be read."""

OUTPUT_FILE: str = "./example_response.json"
"""This is the the output file of the API."""

#-----------------------------------
# Functions
#-----------------------------------

def read_data (file_name: str) -> Optional[object] :
    """This method is used to retrieve data from a json file. Take the path to the file and returns the nameless object contained in the json file."""
    
    # If the file does exists
    if path.exists(file_name) :

        file = open(file_name)
        data = json.load(file)
        file.close()

        return data

    # Otherwise the functions return none.
    else :
        return None

def parse_data (data: object) -> Optional[object] :
    """This method is used to parse the raw data of the json file into power plants, physic factor and load. Take the raw data as parameter."""

    try: 
        power_plants = []

        load: float = data['load']
        gas_price: float = data['fuels']['gas(euro/MWh)']
        kerosine_price: float = data['fuels']['kerosine(euro/MWh)']
        co2_price: float = data['fuels']['co2(euro/ton)']
        wind_power: float = data['fuels']['wind(%)']
        
        wind_physic_factor = WindFactor(wind_power/100)
        pp_index = 0

        for power_plant_data in data['powerplants']: 

            pp_index += 1
            pp_name: str = power_plant_data['name']
            pp_type: str = power_plant_data['type']
            pp_efficiency: float = power_plant_data['efficiency']
            pp_pmin: float = power_plant_data['pmin']
            pp_pmax: float = power_plant_data['pmax']
            pp_price: float = 0.0
            pp_physic_factors: list[IPhysicFactor] = list()
            pp_aon: bool = False

            if pp_type == "gasfired" :
                pp_physic_factors = None
                pp_price = gas_price
            
            elif pp_type == "turbojet" :
                pp_physic_factors = None
                pp_price = kerosine_price
            
            elif pp_type == "windturbine":
                pp_physic_factors.append(wind_physic_factor)
                pp_aon = True

            power_plants.append(PowerPlant(pp_index, pp_name, pp_type, pp_efficiency, pp_pmin, pp_pmax, pp_price, pp_aon, pp_physic_factors))
            del pp_physic_factors

        return [load, power_plants]

    except:
        return None

#TODO create power plant class
def compute_powerplants (load: float, power_plants: list[PowerPlant]) -> None :
    """This method is used to edit the power plant activation. Take the load aimed and the power plants and returns nothing."""

    load_left: float = load

    # Sort by computed price
    power_plants.sort(key= lambda power_plant: (power_plant.compute_price_rate(), power_plant.get_index()))

    # Activate the lower price firsts
    for power_plant in power_plants:

        if load_left == 0 :
            break
        
        if power_plant.compute_price_rate() == 0 :

            load_left -= power_plant.compute_activation(load_left)

        else:
            break
        
    print("load left after no cost power plant : ", load_left)
    
    # If power left -> activate higher price
    if load_left > 0 :

        for power_plant in power_plants :

            if load_left == 0 :
                break
            
            if power_plant.compute_price_rate() == 0 :
                continue

            load_left -= power_plant.compute_activation(load_left)
            
        print("load left after low price linear not priceless : ", load_left)

    # If power left -> it's due to pmin, deactivate last AON (expensive one, or just index wise) and activate low price linear producer
    if load_left > 0 :
        
        nothing_changed = False

        while load_left > 0 and not(nothing_changed):

            nothing_changed = True

            power_plants.reverse()
            
            for power_plant in power_plants :

                if power_plant.is_all_or_nothing() and power_plant.get_activation() == 1 :
                    load_left += power_plant.compute_output_power()
                    power_plant.set_activation(0)
                    break

            power_plants.reverse()

            for power_plant in power_plants :
                
                if load_left == 0 :
                    break

                if power_plant.is_all_or_nothing() :
                    continue

                if power_plant.get_activation() == 1 :
                    continue

                else :
                    nothing_changed = True
                    load_left -= power_plant.compute_activation(load_left)

        print("load left after expensive linear power plants and deactivation of aon : ", load_left)

    # If power left -> reactivate aon until negative power left
    if load_left > 0 :

        for power_plant in power_plants :
            
            if load_left <= 0 :
                break

            if power_plant.is_all_or_nothing() and power_plant.get_activation() == 0 :
                load_left -= power_plant.compute_activation(load_left)

        print("load left after activation of every power plant", load_left)

    # Sort by indices
    power_plants.sort(key= lambda power_plant: power_plant.get_index())
    

def create_exportable_data (power_plants: list[PowerPlant]) -> Optional[list[object]] :
    """This method is used to parse the power plant into a writable nameless object."""
    
    data: list[object] = []

    for power_plant in power_plants:
        pp_data = {'name': power_plant.get_name(), 'p': "{:.1f}".format(power_plant.compute_output_power())}
        data.append(pp_data)
    
    return data

def save_data (file_name: str, data: any) -> None :
    """This method is used to save the data into a json file. Take the path to the file and the nameless object and returns nothing."""
      
    with open(file_name, 'w') as file:
        json.dump(data, file)


#-----------------------------------
# Class
#-----------------------------------

class handler (BaseHTTPRequestHandler) :
    """This is the handler class for the http requests"""

    def do_GET (self) :
        """This is the method called when the user do a get request."""
        
        # Only the /productionplan route is accepted. 
        if self.path == "/productionplan" :

            # Accept the connection by sending 200.
            self.send_response(200)
            self.send_header('content-type', 'text/html')
            self.end_headers()

            # Create a message for explaining to the user what happened.
            message = "Job finished."

            # Read the data stored in the file.
            data = read_data(INPUT_FILE)
            
            # If the file has returned something.
            if not(data is None) :

                # Parsing the data from the json file.
                parsed_data = parse_data(data)

                # If the parsing was succesful
                if not(parsed_data is None) :
                    
                    power_plants: list[PowerPlant] = parsed_data[1]
                    """Are the power plants."""

                    load: float = parsed_data[0]
                    """Is the load of the network."""

                    # Compute the ouput power of each power plants.
                    compute_powerplants(load, power_plants)

                    # Export the data
                    export_data = create_exportable_data(power_plants)

                    print('export data : ', export_data)

                    # If the export data has been created.
                    if not(export_data is None) :

                        save_data(OUTPUT_FILE, export_data)

                        del export_data

                    # Otherwise an error occured while creating a parseable object.
                    else:
                        message = "An error occured while creating a parseable object."

                    del power_plants
                    del load
                    del parsed_data

                # Otherwise an error occured while parsing.
                else:
                    message = "An error occured while parsing the data."

                del data

            # Otherwise an error occured during the reading of the file.
            else:
                message = "An error occured while opening the file."


            # Send a small message to show the API ran correctly.
            self.wfile.write(bytes(message, "utf-8"))

        # Otherwise the http server send back a forbidden access.
        else:
            self.send_response(403)
            self.end_headers()


    def do_POST (self) :
        """This is the method called when the user do a post request."""

        # Any post request is forbidden
        self.send_response(403)
        self.end_headers()




#-----------------------------------
# Main method
#-----------------------------------


with HTTPServer(('', 8888), handler) as server:
    server.serve_forever()
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
    pass

#TODO create power plant class
def compute_powerplants (load: float, power_plants: list[PowerPlant]) -> None :
    """This method is used to edit the power plant activation. Take the load aimed and the power plants and returns nothing."""
    pass

def create_exportable_data (power_plants: list[PowerPlant]) -> Optional[list[object]] :
    """This method is used to parse the power plant into a writable nameless object."""
    pass

def save_data (file_name: str, data: any) -> None :
    """This method is used to save the data into a json file. Take the path to the file and the nameless object and returns nothing."""
    pass

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
            if not(data == None) :

                # Parsing the data from the json file.
                parsed_data = parse_data(data)

                # If the parsing was succesful
                if not(parsed_data == None) :
                    
                    power_plants: list[PowerPlant] = parsed_data.power_plants
                    """Are the power plants."""

                    load: float = parsed_data.load
                    """Is the load of the network."""

                    # Compute the ouput power of each power plants.
                    compute_powerplants(load, power_plants)

                    # Export the data
                    export_data = create_exportable_data(power_plants)

                    # If the export data has been created.
                    if not(export_data == None) :

                        save_data(OUTPUT_FILE, export_data)

                    # Otherwise an error occured while creating a parseable object.
                    else:
                        message = "An error occured while creating a parseable object."

                # Otherwise an error occured while parsing.
                else:
                    message = "An error occured while parsing the data."

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
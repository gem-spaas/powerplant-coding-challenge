#!/usr/bin/env python3

import json

class Payload:
    """Class definition for Payload."""
    def __init__(self, payload):
        self.payload_dict = payload
        
    def load(self):
        if (len(self.payload_dict)):
            return self.payload_dict["load"]
    
    def powerPlants(self):
        if (len(self.payload_dict)):
            return self.payload_dict["powerplants"]

    def fuels(self):
        if (len(self.payload_dict)):
            return self.payload_dict["fuels"]
    
    def dump(self):
        if (len(self.payload_dict)):
            return self.payload_dict

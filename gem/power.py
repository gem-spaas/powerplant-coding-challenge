#!/usr/bin/env python3

class PowerSupply:
    """Class definition for PowerSupply."""
    def __init__(self, payload=None):
        self.payload = payload if payload is not None else None

    def setPayload(self, payload=None):
        if payload is not None:
            self.payload = payload

    def production_plan(self):
        payload = self.payload
        response = { "response": payload }
        return response

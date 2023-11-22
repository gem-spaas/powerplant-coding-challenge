class InvalidPayload(Exception):
    def __init__(self, message):
        self.message = f"Invalid payload: {message} missing'"
        super().__init__(self.message)


class InvalidPayloadPowerplants(Exception):
    def __init__(self, message):
        self.message = f"Invalid payload: {message} missing within 'powerplants'"
        super().__init__(self.message)


class InvalidPayloadFuels(Exception):
    def __init__(self, message):
        self.message = f"Invalid payload: {message} missing within 'fuels'"
        super().__init__(self.message)

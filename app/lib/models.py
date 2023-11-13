from .pydantic_models import PowerPlant, Fuels

# Define constants
CO2_EMISSION_FACTOR = 0.3  # tons of CO2 per MWh


class PowerPlantBase:

    def __init__(self, powerplant_data: PowerPlant):
        self.name = powerplant_data.name
        self.pmin = powerplant_data.pmin
        self.pmax = powerplant_data.pmax
        self.efficiency = powerplant_data.efficiency
        self.type = powerplant_data.type


class WindTurbine(PowerPlantBase):

    def __init__(self, plant_data: PowerPlant, wind_percentage: int):
        super().__init__(plant_data)
        self.wind_percentage = wind_percentage

    def calculate_allocated_power(self, remaining_load):
        wind_power = (self.wind_percentage / 100) * self.pmax
        wind_power = round(wind_power, 2)
        return min(wind_power, remaining_load)

    
    def calculate_co2_cost(self):
        return 0
    
    def calculate_fuel_cost(self):
        return 0

class GasFiredPowerPlant(PowerPlantBase):

    def __init__(self, plant_data: PowerPlant, gas_cost: float, co2_cost: float):

        super().__init__(plant_data)
        self.gas_cost = gas_cost
        self.co2_cost = co2_cost

    def calculate_allocated_power(self, remaining_load):
        gas_power = min(self.pmax, max(self.pmin, remaining_load))
        return round(gas_power, 2)
    
    def calculate_fuel_cost(self, allocated_power):
        return (allocated_power / self.efficiency) * self.gas_cost

class TurboJetPowerPlant(PowerPlantBase):
    def __init__(self, plant_data: PowerPlant, kerosine_cost: float, co2_cost: float):
        super().__init__(plant_data)
        self.kerosine_cost = kerosine_cost
        self.co2_cost = co2_cost

    def calculate_allocated_power(self, remaining_load):
        turbo_power = min(self.pmax, max(self.pmin, remaining_load))
        return round(turbo_power, 2)

    def calculate_fuel_cost(self, allocated_power):
        return (allocated_power / self.efficiency) * self.kerosine_cost

class PowerPlantFactory:
    def create_powerplant(self, plant_data: PowerPlant, fuels: Fuels):
        if plant_data.type == 'windturbine':
            return WindTurbine(plant_data, fuels.wind_percentage)
        elif plant_data.type == 'gasfired':
            return GasFiredPowerPlant(plant_data, fuels.gas_euro_mwh, fuels.co2_euro_ton)
        elif plant_data.type == 'turbojet':
            return TurboJetPowerPlant(plant_data, fuels.kerosine_euro_mwh, fuels.co2_euro_ton)


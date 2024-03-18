from constants import *


class PowerPlant:
    """
    A PowerPlant object represents a power plant, it has its attributes, and it can calculate its own cost.
    """

    def __init__(self, name, type, efficiency, pmin, pmax, price=0, wind_strength=0):
        self.name = name
        self.type = type
        self.efficiency = efficiency
        self.pmin = pmin * 1.0
        self.pmax = pmax * 1.0
        self.price = price
        if type == 'windturbine':  # could be in a new child class
            self.pmax = round(self.pmax * (wind_strength / 100), 1)
        self.cost = None
        self.__set_cost()

    def __set_cost(self):
        """
        Sets the cost of the power plant.
        """
        try:
            self.cost = self.price / self.efficiency
        except ZeroDivisionError:
            self.cost = 0


class ProductionPlanGenerator:
    """
    A ProductionPlanGenerator object receives the required load, the fuel costs and the list of available power plants,
    and generates the production plan accordingly.
    """

    def __init__(self, load, fuels, powerplants):
        self.load = load
        self.prices = {}
        self.wind_strength = 0
        self.power_plants = []

        self.__load_fuel_details(fuels)
        self.__create_power_plants(powerplants)
        self.__set_power_plant_order()

    def __load_fuel_details(self, fuel_prices):
        """
        Loads the fuel details from the data received from the request.
        """
        for fuel, value in fuel_prices.items():
            if fuel not in FUEL_POWERPLANT_MAPPING.keys():
                continue
            if fuel == WIND:
                self.wind_strength = value
            self.prices[FUEL_POWERPLANT_MAPPING[fuel]] = value
        self.prices[FUEL_POWERPLANT_MAPPING[WIND]] = 0

    def __create_power_plants(self, powerplants):
        """
        Creates the list power plant objects, based on the power plant list that we receive from the request data.
        """
        for powerplant_type in FUEL_POWERPLANT_MAPPING.values():
            self.power_plants += [PowerPlant(name=powerplant.get('name'),
                                             type=powerplant.get('type'),
                                             efficiency=powerplant.get('efficiency'),
                                             pmin=powerplant.get('pmin'),
                                             pmax=powerplant.get('pmax'),
                                             price=self.prices[powerplant_type],
                                             wind_strength=self.wind_strength)
                                  for powerplant in powerplants if powerplant.get('type') == powerplant_type]

    def __set_power_plant_order(self):
        """
        Sets the order of the power plants based on the merit order.
        """
        self.power_plants.sort(key=lambda x: (x.cost, x.pmax * -1), reverse=False)

    def get_production_plan(self):
        """
        Creates and returns the production plan.

        For sure there is room to improve and most probably there are edge cases where it will not work well ...
        """
        production_plan = []
        remaining_load = self.load

        # looping through all the power plan objects in the list
        for power_plant in self.power_plants:
            # if the remaining load is zero, we just add the rest of the power plants with p=0.0
            if remaining_load <= 0:
                production_plan.append({'name': power_plant.name, 'p': 0.0})
                continue
            # the energy output has to be greater than 0.1
            if power_plant.pmax < 0.1:
                continue
            # if the pmin of the current power plant is greater than the remaining load (and we have other power plants
            # already in the list), we need to check if we can reduce the output of the previous power plants
            if power_plant.pmin > remaining_load and len(production_plan) > 0:
                energy_excess = power_plant.pmin - remaining_load
                # looping through the power plants already in the production plan
                for x in production_plan[::-1]:
                    # finding the power plant based on the name
                    cur_powerplant = [p for p in self.power_plants if p.name == x.get('name')][0]
                    cur_p = x.get('p')
                    # the room by which we can reduce p (till the pmin)
                    room_to_reduce = cur_p - cur_powerplant.pmin
                    if room_to_reduce > 0.0:  # if we have room to reduce
                        # if we can reduce the output of this power plant more than the excess, then all is fine,
                        # we break out of the loop after
                        if room_to_reduce >= energy_excess:
                            x['p'] = cur_p - energy_excess
                            remaining_load += energy_excess
                            break
                        # if we can reduce the output of this power plan less than the excess, then we do as much as
                        # we can, then break out of the loop anyway, because the remaining_load value changes
                        # and the certain conditions may work differently for the current powerplant (with the
                        # excess of pmin)
                        x['p'] = cur_p - room_to_reduce
                        remaining_load += energy_excess
                        energy_excess -= room_to_reduce
                        break
                    else:
                        continue
            # if the pmax is greater than the remaining load, then done
            if power_plant.pmax >= remaining_load:
                production_plan.append({'name': power_plant.name, 'p': remaining_load})
                remaining_load = 0
                continue
            # if none of the above conditions are satisfied, we add the current power plan with its maximum output
            production_plan.append({'name': power_plant.name, 'p': power_plant.pmax})
            remaining_load -= power_plant.pmax
        return production_plan

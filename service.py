from models.powerplant import Windturbine, Gasfired, Turbojet
from services.exceptions import (
    InvalidPayloadPowerplants,
    InvalidPayloadFuels,
)

# Constants
CO2_EMMISION_FACTOR = 0.3


class Service:
    # List of objects containing the given powerplants
    powerplants = []

    def create_powerplants(self, powerplants_data, fuels):
        """
        Method that given a dictionary of powerplants data, creates a
        Powerplant object for each one, based on their type and taking
        into account their extra attributes.

        Args:
            powerplants_data (dictionary): contains the powerplants data
            fuels (dictionary): contains the current fuels info

        Returns:
            list: created Powerplant objects
        """
        self.powerplants = []

        # Create an object for each given powerplant and add it to the list
        for data in powerplants_data:
            # Take into account the type of powerplant and its extra attributes
            try:
                type_ = data["type"]
            except KeyError as e:
                raise InvalidPayloadPowerplants(e)

            if type_ == "windturbine":
                powerplant = Windturbine(**data)

            if type_ == "gasfired":
                gasfired_data = data.copy()
                try:
                    gasfired_data.update(
                        {
                            "gas_price": fuels["gas(euro/MWh)"],
                            "co2_price": fuels["co2(euro/ton)"],
                        }
                    )
                except KeyError as e:
                    raise InvalidPayloadFuels(e)

                powerplant = Gasfired(**gasfired_data)

            if type_ == "turbojet":
                turbojet_data = data.copy()
                try:
                    turbojet_data.update(
                        {"kerosine_price": fuels["kerosine(euro/MWh)"]}
                    )
                except KeyError as e:
                    raise InvalidPayloadFuels(e)

                powerplant = Turbojet(**turbojet_data)

            # Add the powerplant to the list
            self.powerplants.append(powerplant)

        return self.powerplants

    def sort_by_power_cost(self, powerplants):
        """
        Method that sorts the powerplants based on their real cost of
        generating electrical power, which will be the merit-order.

        Args:
            powerplants (list): PowerPlant objects

        Returns:
            list: sorted PowerPlant objects by cost
        """
        return sorted(powerplants, key=lambda x: x.power_cost)

    def get_production_plan(self, load, powerplants, fuels):
        """
        Method that calculates the whole production plan, deciding which
        powerplants to activate depending on the merit-order, and taking
        into account their pmin and pmax.

        Args:
            load (int): amount of energy (MWh) that needs to be generated
            powerplants (dictionary): sorted powerplants by merit-order
            fuels (dictionary): contains the current fuels info

        Returns:
            dictionary: complete production plan
        """
        # To calculate the needed power from each plant, we will take the
        # initial load and substract an specific amount of the sorted
        # powerplants, taking into account their pmin and pmax
        load_remaining = load

        production_plan = []

        for i, plant in enumerate(powerplants):
            # Initialize the generated power from a plant
            generated_power = 0

            # Wind turbines pmax depend on the percentage of wind
            if plant.type == "windturbine":
                plant.pmax = plant.pmax * fuels.get("wind(%)") / 100

            # In case we reached the needed load, we could do a break, but
            # we want to log the unused plant as well
            if load_remaining == 0:
                pass

            elif plant.pmax == 0:
                pass

            # If the pmin of a plant is bigger than the remaining load, we
            # shouldn't turn it on, so we pass to the next one
            elif plant.pmin > load_remaining:
                pass

            # If the pmax is higher than the remaining load, we take all the
            # generated power, which will result in load_remaining=0
            elif plant.pmax > load_remaining:
                generated_power += load_remaining
                load_remaining -= generated_power

            # If the pmax is lower, we will take as much power as possible,
            # taking into account the next powerplant pmin, to avoid extra
            # cost because of not turning it on
            elif plant.pmax < load_remaining:
                temp_remaining = load_remaining - plant.pmax
                try:
                    if temp_remaining < powerplants[i + 1].pmin:
                        generated_power += load_remaining - (
                            plant.pmax - (powerplants[i + 1].pmin - temp_remaining)
                        )

                    else:
                        generated_power += plant.pmax

                except IndexError:
                    generated_power += plant.pmax

                load_remaining -= generated_power

            production_plan.append({"name": plant.name, "p": generated_power})

        return production_plan

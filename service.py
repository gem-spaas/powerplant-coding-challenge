from model import PowerPlant

# Constants
CO2_EMMISION_FACTOR = 0.3


class Service:
    # List of objects containing the given powerplants
    powerplants = []

    def create_powerplants(self, powerplants_data, fuels):
        """
        Method that given a dictionary of powerplants data, creates a
        Powerplant object for each one, calculating their real cost
        of generated electric power, and sorts them in merit-order.

        Args:
            powerplants_data (dictionary): contains the powerplants data
            fuels (dictionary): contains the current fuels info

        Returns:
            list: sorted Powerplant objects by cost
        """
        self.powerplants = []

        # Create an object for each given powerplant and add it to the list
        for data in powerplants_data:
            # Create a new Powerplant object
            powerplant = PowerPlant(**data)
            # Calculate the real cost of a generated MWh
            powerplant.cost = self.calculate_cost(
                powerplant.type, powerplant.efficiency, fuels
            )
            # Add the powerplant to the list
            self.powerplants.append(powerplant)

        # Now sort the list by its real cost, but keep the original
        sorted_plants = sorted(self.powerplants, key=lambda x: x.cost)

        return sorted_plants

    def calculate_cost(self, type, efficiency, fuels):
        """
        Method that calculates the real cost of generating electrical power
        based on the powerplant type and efficiency.

        Args:
            type (str): type of powerplant
            efficiency (float): thermal efficiency of a plant
            fuels (dictionary): contains the current fuels info

        Returns:
            float: real cost of the generated power
        """
        if type == "windturbine":
            return 0

        elif type == "gasfired":
            # gas(euro/MWh) / efficiency x (0.3 * co2(euro/ton))
            return (
                fuels["gas(euro/MWh)"]
                / efficiency
                * (CO2_EMMISION_FACTOR * fuels["co2(euro/ton)"])
            )

        elif type == "turbojet":
            # kerosine(euro/MWh) / efficiency
            return fuels["kerosine(euro/MWh)"] / efficiency

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

        for plant in powerplants:
            # Initialize the generated power from a plant
            generated_power = 0

            # In case ww reached the needed load, we could do a break, but
            # we want to log the unused plant as well
            if load_remaining <= 0:
                production_plan.append({"name": plant.name, "p": generated_power})
                continue

            # Wind turbines pmax depend on the percentage of wind
            if plant.type == "windturbine":
                plant.pmax = plant.pmax * fuels.get("wind(%)") / 100

            # If the pmin of a plant is bigger than the remaining load, we
            # shouldn't turn it on, so we pass to the next one
            if plant.pmin > load_remaining:
                continue

            # If the pmax is higher than the remaining load, we take as much
            # load as needed to reach the remaining, which will result in 0
            if plant.pmax > load_remaining:
                generated_power += load_remaining
                load_remaining -= generated_power

            # Else, we take all the available energy
            else:
                generated_power += plant.pmax
                load_remaining -= generated_power

            production_plan.append({"name": plant.name, "p": generated_power})

        return production_plan

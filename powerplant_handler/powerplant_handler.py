class PowerplantHandler:

    @staticmethod
    def calculate_production_plan(payload, config):
        """
        Calculates the production plan for the given payload
        :param payload: The input data from the request
        :param config: Configuration variables to be used in the cost_per_mwh calculations
        :return: The production plan
        """
        # Extract necessary data from payload
        load = payload['load']
        fuels = payload['fuels']
        powerplants = payload['powerplants']
        include_co2_emissions = config.getboolean('SETTINGS', 'INCLUDE_CO2_EMISSIONS')
        gasfired_emissions = config.getfloat('SETTINGS', 'GASFIRED_CO2_TONS')
        turbojet_emissions = config.getfloat('SETTINGS', 'TURBOJET_CO2_TONS')

        # Calculate the cost of producing electricity for each powerplant
        for plant in powerplants:
            plant_cost_per_mwh = PowerplantHandler.calculate_cost_per_mwh(plant, fuels, include_co2_emissions,
                                                                          gasfired_emissions, turbojet_emissions)

            plant["cost"] = plant_cost_per_mwh
            if plant["type"] == "windturbine":
                plant['pmax'] = plant['pmax'] * fuels['wind(%)'] / 100

        # Sort powerplants by cost
        powerplants = sorted(powerplants, key=lambda x: x['cost'])

        # Calculate the power distribution
        response = PowerplantHandler.calculate_power_distribution(powerplants, load)

        # Set the other plants to zero
        response = response + [{'name': plant['name'], 'p': 0} for plant in powerplants
                               if plant['name'] not in [item['name'] for item in response]]

        return response

    @staticmethod
    def calculate_power_distribution(powerplants, load):
        """
        Calculates the power distribution for the given powerplants and load
        :param powerplants:  The powerplants to be used in the calculation which have already had their cost calculated
        :param load: The load to be met
        :return: The power distribution
        """
        response = []
        remaining_load = load

        for i, plant in enumerate(powerplants):
            power = 0

            if plant['pmax'] > 0:

                if plant['type'] == 'windturbine':
                    power = plant['pmax']
                elif remaining_load > 0 and remaining_load >= plant['pmin']:
                    power = min(remaining_load, plant['pmax'])
                    power = max(power, plant['pmin'])

                # EDGE CASE: Check that we can distribute power to the next plant.
                # There must be more remaining load than the next plant's pmin
                # This must only occur if we will actually need the next plant!
                if i < len(powerplants) - 1:
                    next_plant = powerplants[i + 1]
                    if next_plant['pmin'] > remaining_load - power > 0:
                        power = remaining_load - next_plant['pmin']

                remaining_load -= power
                power = round(power, 1)

                response.append({"name": plant['name'], "p": power})

                if remaining_load <= 0:
                    break

        # Ensure the total power matches the load
        total_power = sum(item['p'] for item in response)
        if total_power < load:
            raise ValueError('Not enough power to meet the load')

        return response

    @staticmethod
    def calculate_cost_per_mwh(plant, fuels, include_co2_emissions, gasfired_emissions, turbojet_emissions):
        """
        Calculates the cost of producing electricity for a given power plant
        :param plant: The current power plant
        :param fuels: The fuels data for the corresponding power plant
        :param include_co2_emissions: Boolean to use or not use the CO2 emissions for the calculations
        :param gasfired_emissions: The CO2 emissions for gasfired power plants
        :param turbojet_emissions: The CO2 emissions for turbojet power plants
        :return: The cost of producing electricity for the given power plant
        """
        if plant["type"] == "gasfired":
            fuel_cost_per_mwh = fuels["gas(euro/MWh)"] / plant["efficiency"]
            co2_emissions_cost_per_mwh = gasfired_emissions * fuels["co2(euro/ton)"]
        elif plant["type"] == "turbojet":
            fuel_cost_per_mwh = fuels["kerosine(euro/MWh)"] / plant["efficiency"]
            co2_emissions_cost_per_mwh = turbojet_emissions * fuels["co2(euro/ton)"]
        elif plant["type"] == "windturbine":
            fuel_cost_per_mwh = 0
            co2_emissions_cost_per_mwh = 0
        else:
            raise ValueError(f"Invalid power plant type provided: {plant['type']}")

        if include_co2_emissions:
            total_cost_per_mwh = fuel_cost_per_mwh + co2_emissions_cost_per_mwh
        else:
            total_cost_per_mwh = fuel_cost_per_mwh

        return total_cost_per_mwh

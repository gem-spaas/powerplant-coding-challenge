from powerplant_coding_challenge.utils.dataclasses import Payload, Powerplant
import operator
from typing import Tuple
from dataclasses import replace
import logging

logging.basicConfig(format="%(asctime)s %(levelname)-8s %(message)s", level=logging.INFO, datefmt="%Y-%m-%d %H:%M:%S")
logger = logging.getLogger(__name__)


def compute_powerplant_production(payload_loaded: Payload) -> list:
    f"""
    compute the optimal powerplant production depending on a given payload
    - format all powerplants at Powerplant dataclass type
    - split powerplant by type
    - compute needed production per powerplant type
    - decide best method to use to have optimal results 
    - format output to have a correct json format
    
    :param payload_loaded : Payload object that contains load, fuels and powerplants
     
    :return: a list containing output at a json format of optimal powerplant consumptions for a given payload
    """
    logger.info("compute powerplant production")
    list_powerplants: list = []

    # format all powerplants at Powerplant dataclass type
    for powerplant in payload_loaded.powerplants:
        encoded_powerplant = Powerplant(name=powerplant['name'], type=powerplant['type'],
                                        efficiency=powerplant['efficiency'],
                                        pmin=powerplant['pmin'], pmax=powerplant['pmax'])

        encoded_powerplant_cost = replace(encoded_powerplant,
                                          cost=compute_cost_of_powerplant(powerplant=encoded_powerplant,
                                                                          payload_loaded=payload_loaded))
        encoded_powerplant_min_cost = replace(encoded_powerplant_cost, min_cost=encoded_powerplant_cost.cost *
                                                                                encoded_powerplant_cost.pmin)
        list_powerplants.append(encoded_powerplant_min_cost)

    # split powerplant by type
    windturbine_powerplants = sorted(
        [powerplant for powerplant in list_powerplants if powerplant.type == "windturbine"],
        key=operator.attrgetter('pmax'), reverse=True)
    turbojet_powerplants = sorted([powerplant for powerplant in list_powerplants if powerplant.type == "turbojet"],
                                  key=operator.attrgetter('pmax'))
    gasfired_powerplants = sorted([powerplant for powerplant in list_powerplants if powerplant.type == "gasfired"],
                                  key=operator.attrgetter('min_cost'))
    fossil_fuel_powerplants = sorted(
        [powerplant for powerplant in list_powerplants if powerplant.type in ("gasfired", "turbojet")],
        key=operator.attrgetter('pmax'), reverse=True)

    # compute needed production per powerplant type
    windturbine_powerplants, remaining_load_after_wind = compute_needed_load_windturbine(load=payload_loaded.load,
                                                                                         windturbine_powerplants=windturbine_powerplants,
                                                                                         payload_loaded=payload_loaded)

    turbojet_powerplants, remaining_load_after_turbo = compute_needed_load_fossil_fuel(load=remaining_load_after_wind,
                                                                                    powerplants_list=turbojet_powerplants)
    gasfired_powerplants, remaining_load_after_gas = compute_needed_load_fossil_fuel(load=remaining_load_after_turbo,
                                                                                  powerplants_list=gasfired_powerplants)

    fossil_fuel_powerplants, remaining_load_after_fossil = compute_needed_load_fossil_fuel(
        load=remaining_load_after_wind,
        powerplants_list=fossil_fuel_powerplants)

    # decide best method to use to have optimal results
    unused_powerplants_method_1: int = count_number_unused_powerplants(powerplants_list=turbojet_powerplants+windturbine_powerplants+gasfired_powerplants)
    unused_powerplants_method_2: int = count_number_unused_powerplants(
        powerplants_list= windturbine_powerplants + fossil_fuel_powerplants)
    all_powerplants_method_1: list = gasfired_powerplants+windturbine_powerplants+turbojet_powerplants
    all_powerplants_method_2: list = fossil_fuel_powerplants + windturbine_powerplants
    choosen_method: list = all_powerplants_method_2 if unused_powerplants_method_2 > unused_powerplants_method_1 else all_powerplants_method_1

    # format output to have a correct json format
    final_output = format_output(all_powerplants=choosen_method)
    logger.info("powerplant production computed successfully!")
    return final_output


def count_number_unused_powerplants(powerplants_list: list) -> int:
    f"""
    given a list of powerplant having their own consumption already computed, count the number of powerplant that would 
    be unused

    :param powerplants_list : list of powerplant with consumption already computed

    :return: an integer that indicated the number of unused powerplants in the list of powerplants
    """
    unused_powerplants = 0
    for powerplant in powerplants_list:
        if powerplant.consumption == 0.0:
            unused_powerplants += 1
    return unused_powerplants


def format_output(all_powerplants: list) -> list:
    f"""
    given a list of powerplant, format them at a correct json for the output

    :param all_powerplants : list of all powerplants

    :return: a list that could be interpreted at a json
    """
    final_output: list = []
    for powerplant in all_powerplants:
        final_output.append({"name": f"{powerplant.name}", "p": powerplant.consumption})
    return final_output


def compute_cost_of_powerplant(powerplant: Powerplant, payload_loaded: Payload) -> float:
    f"""
    given a powerplant, compute the cost of it

    :param powerplant :     a Powerplant object 
    :param payload_loaded : loaded Payload object

    :return: a float that indicates the cost of a given powerplant
    """
    if powerplant.type == 'gasfired':
        co2_emission = 0.3
        return (payload_loaded.fuels.gas + co2_emission * payload_loaded.fuels.co2) / powerplant.efficiency
    elif powerplant.type == 'turbojet':
        return payload_loaded.fuels.kerosine / powerplant.efficiency
    elif powerplant.type == 'windturbine':
        return 0.0


def compute_needed_load_windturbine(load: float, windturbine_powerplants: list, payload_loaded: Payload) -> Tuple[
    list, float]:
    f"""
    compute the load needed for all wind turbine powerplant for a given load

    :param load :                       a Powerplant object 
    :param windturbine_powerplants :    list of all wind turbine power plants
    :param payload_loaded :             loaded Payload object

    :return: a tuple containing the list of wind turbine power plants with their needed consumption and the remaining load as a float format
    """
    powerplants_with_consumption: list = []
    wind_power = (payload_loaded.fuels.wind / 100)
    for powerplant in windturbine_powerplants:
        load_left_after_powerplant: float = max(0, round(load - powerplant.pmax * wind_power, 2))
        powerplant_consumption = round(load - load_left_after_powerplant, 2)
        powerplant_with_consumption = replace(powerplant, consumption=round(powerplant_consumption, 2))
        powerplants_with_consumption.append(powerplant_with_consumption)
        load = load_left_after_powerplant
    return powerplants_with_consumption, load


def compute_needed_load_fossil_fuel(load: float, powerplants_list: list) -> Tuple[list, float]:
    f"""
    compute the load needed for all fossil fuel type powerplant for a given load

    :param load :                       a Powerplant object 
    :param powerplants_list :           list of all fossil fuel power plants

    :return: a tuple containing the list of fossil fuel type power plants with their needed consumption and the remaining load as a float format
    """
    powerplants_with_consumption: list = []
    load_left_after_powerplant = load
    for powerplant in powerplants_list:
        if load_left_after_powerplant > 0:
            load_left_after_powerplant: float = max(0, round(load - powerplant.pmax, 2))
            powerplant_consumption = max(round(load - load_left_after_powerplant, 2), powerplant.pmin)
            powerplant_with_consumption = replace(powerplant, consumption=powerplant_consumption)
            powerplants_with_consumption.append(powerplant_with_consumption)
            load = load_left_after_powerplant
        else:
            powerplant_with_consumption = replace(powerplant, consumption=0.0)
            powerplants_with_consumption.append(powerplant_with_consumption)
            load = load_left_after_powerplant
    return powerplants_with_consumption, load

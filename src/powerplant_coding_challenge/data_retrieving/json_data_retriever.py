from powerplant_coding_challenge.utils.dataclasses import Payload, Fuels
import logging

logging.basicConfig(format="%(asctime)s %(levelname)-8s %(message)s", level=logging.INFO, datefmt="%Y-%m-%d %H:%M:%S")
logger = logging.getLogger(__name__)


def retrieve_data_from_json_payload(json_payload: dict) -> Payload:
    f"""
    encode json payload data at a Payload dataclass format

    :param json_payload:    payload given by a POST request at a json format

    :return: a Payload dataclass
    """
    logger.info("retrieve payload from json")
    load: int = json_payload['load']
    fuels: Fuels = Fuels(gas=json_payload['fuels']['kerosine(euro/MWh)'],
                         kerosine=json_payload['fuels']['gas(euro/MWh)'],
                         co2=json_payload['fuels']['co2(euro/ton)'],
                         wind=json_payload['fuels']['wind(%)'])
    powerplants: list = json_payload['powerplants']

    payload = Payload(load=load, fuels=fuels, powerplants=powerplants)
    logger.info("payload retrieved successfully!")

    return payload

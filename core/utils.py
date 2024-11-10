from core.classes import Fuels, Powerplant
import yaml
import sys
from os import getenv, path, makedirs
import logging
from datetime import datetime

class Logger:
    # my default logger config
    _instance = None
    _initialized = False

    def __new__(cls):
        if cls._instance is None:
            cls._instance = super(Logger, cls).__new__(cls)
        return cls._instance

    def __init__(self):
        if not Logger._initialized:
            log_dir = 'logs'
            if not path.exists(log_dir):
                makedirs(log_dir)
            self.logger = logging.getLogger('PowerPlantAPI')
            self.logger.setLevel(logging.DEBUG)
            file_formatter = logging.Formatter(
                '%(asctime)s - %(name)s - %(levelname)s - %(message)s'
            )
            console_formatter = logging.Formatter(
                '%(levelname)s: %(message)s'
            )
            log_file = path.join(log_dir, f'powerplant_{datetime.now().strftime("%Y%m%d")}.log')
            file_handler = logging.FileHandler(log_file)
            file_handler.setLevel(logging.DEBUG)
            file_handler.setFormatter(file_formatter)
            console_handler = logging.StreamHandler(sys.stdout)
            console_handler.setLevel(logging.INFO)
            console_handler.setFormatter(console_formatter)
            self.logger.addHandler(file_handler)
            self.logger.addHandler(console_handler)
            Logger._initialized = True

    def debug(self, message): self.logger.debug(message)
    def info(self, message): self.logger.info(message)
    def warning(self, message): self.logger.warning(message)
    def error(self, message): self.logger.error(message)
    def critical(self, message): self.logger.critical(message)


DEFAULT_CONFIG = {
    'API': {
        'protocol': 'http',
        'host': '0.0.0.0',
        'endpoint': 'productionplan',
        'port': 8888
    }
}
logger = Logger()


def get_config():
    """Get configuration from environment variables or config file"""
    # Load from config file first
    config = DEFAULT_CONFIG
    # Override with environment variables when it run in docker
    if getenv('DOCKER_ENV'):
        logger.info("Docker environment detected, overriding with environment variables")
        if getenv('CONFIG'):
            config = load_config(getenv('CONFIG'))
        else:
            config['API'].update({
                'protocol': getenv('API_PROTOCOL', config['API']['protocol']),
                'host':     getenv('API_HOST', config['API']['host']),
                'port':     int(getenv('API_PORT', config['API']['port'])),
                'endpoint': getenv('API_ENDPOINT', config['API']['endpoint'])
                })
    else:
        logger.info(f"Loading configuration from config.yml")
        config = load_config('config.yml')
    return config


def load_config(default_path: str = 'config.yml'):
    """Load configuration from YAML file"""
    with open(default_path) as stream:
        try:
            config = yaml.safe_load(stream)
            logger.debug(f"Successfully loaded config: {config}")
        except yaml.YAMLError as exc:
            logger.error(f"Error loading config file: {exc}")
            logger.info(f"Loading default integrated config : {DEFAULT_CONFIG}")
            config = DEFAULT_CONFIG
        except FileNotFoundError:
            logger.critical(f"Config file not found: {default_path}")
            logger.info(f"Loading default integrated config : {DEFAULT_CONFIG}")
            config = DEFAULT_CONFIG
    return config


def format_payload(payload):
    # Assume Payload format won't change / order won't change (fuels and powerplant struct)
    try:
        logger.info("Processing payload")
        load = payload.load
        fuels_stat = Fuels(*payload.fuels.values())
        powerplants_list = [Powerplant(i + 1, **pw) for i, pw in enumerate(payload.powerplants)]
        return load, fuels_stat, powerplants_list
    except Exception as e:
        logger.error(f"Error formatting payload: {str(e)} \nBe sure format/order is correct")
        raise



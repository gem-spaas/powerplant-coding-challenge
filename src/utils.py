import yaml
from pathlib import Path
import os


CONFIG_FILE = os.path.join(Path(__file__).resolve().parent.parent, 'config.yml')


def load_config():
    """
    Returns the content of the config file.
    """
    try:
        with open(CONFIG_FILE) as config_file:
            config = yaml.safe_load(config_file)
            if not config:
                return {}
    except FileNotFoundError:
        return {}
    return config

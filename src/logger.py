import logging
import datetime


logging.basicConfig(filename='./logs/log_api_server_{}.log'.format(datetime.datetime.strftime(datetime.datetime.now(),
                                                                 '%Y%m%d%H%M%S')),  # could be in config file
                    level=logging.ERROR,
                    datefmt='%Y.%m.%d-%H:%M:%S',
                    format='%(asctime)s - %(levelname)s - %(message)s')


def get_logger():
    """Returns the logger."""
    logging.getLogger()

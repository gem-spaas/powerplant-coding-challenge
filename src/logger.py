import logging

def setup_logger():
    """
    Sets up the logger with predefined formats for info, error, and warning messages.
    """
    # Create custom logger
    logger = logging.getLogger("production_plan_logger")
    logger.setLevel(logging.DEBUG)  # Allow all levels of logging

    # Create console handler and set level to debug
    ch = logging.StreamHandler()
    ch.setLevel(logging.DEBUG)

    # Create a formatter that includes the log level and the message
    formatter = logging.Formatter('%(asctime)s - %(levelname)s - %(message)s')

    # Add formatter to the console handler
    ch.setFormatter(formatter)

    # Add console handler to logger
    logger.addHandler(ch)

    # Example: Predefined log messages
    logger.info("Logger initialized successfully.")
    logger.warning("This is a predefined warning message.")
    logger.error("This is a predefined error message.")

    return logger



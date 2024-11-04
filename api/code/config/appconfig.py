"""
    This is the Configuration System for the Powerplan  API

    All these Variables should be exported in the Container and need to be readable
    by the Process.

    If there is no needed configuration, the Container will End with ValueError.

"""

import logging
import os
from logging.config import dictConfig
from config.constants import BS_LOGGER, BS_CORRELATION_LOGGER
from config.traceability import CorrelationIdFilter

LOG_LEVEL = os.getenv("LOG_LEVEL", "")
BS_LOG_LEVEL = os.getenv("BS_LOG_LEVEL", "")

if LOG_LEVEL is None or LOG_LEVEL == "":
    LOG_LEVEL = "WARNING"

if BS_LOG_LEVEL is None or BS_LOG_LEVEL == "":
    BS_LOG_LEVEL = LOG_LEVEL

dictConfig(
    {
        "version": 1,
        "formatters": {
            "data-format": {
                # DATE | BS LOGGER NAME | LEVEL | FILENAME | LINE | FUNCTION | MESSAGE
                "format": "%(asctime)s - %(name)s - %(levelname)s - %(filename)s:%(lineno)d - %(funcName)s - %(message)s",
                "datefmt": "%Y%m%d%H%M%S",
            },
            "correlation-format": {
                # DATE | BS LOGGER NAME | LEVEL | FILENAME | LINE | FUNCTION | CORRELATION ID | MESSAGE
                "format": "%(asctime)s - %(name)s - %(levelname)s - %(filename)s:%(lineno)d - %(funcName)s - [%(correlation_id)s] - %(message)s",
                "datefmt": "%Y%m%d%H%M%S",
            },
        },
        "filters": {
            "correlation_id_filter": {
                "()": CorrelationIdFilter,
            }
        },
        "handlers": {
            "default": {
                "formatter": "data-format",
                "class": "logging.StreamHandler",
                "stream": "ext://sys.stdout",  # Default is stderr
            },
            "correlation": {
                "formatter": "correlation-format",
                "class": "logging.StreamHandler",
                "filters": ["correlation_id_filter"],
                "stream": "ext://sys.stdout",  # Default is stderr
            },
        },
        "root": {"level": LOG_LEVEL, "handlers": ["default", "correlation"]},
        "loggers": {
            BS_LOGGER: {"level": BS_LOG_LEVEL, "propagate": 0, "handlers": ["default"]},
            BS_CORRELATION_LOGGER: {
                "level": BS_LOG_LEVEL,
                "propagate": 0,
                "handlers": ["correlation"],
            },
        },
    }
)

logger = logging.getLogger(BS_LOGGER)
logger.debug("After logger initialization")

# Only for Log
APP_NAME = "powerplant-code-challenge"
APP_VERSION = os.getenv("APP_VERSION", "1.1.0")
SWAGGER_URL_PREFIX = os.getenv("SWAGGER_URL_PREFIX", "")

# Feature List
FEATURE_ENABLE_SWAGGER_UI = os.getenv("FEATURE_ENABLE_SWAGGER_UI", "NO")

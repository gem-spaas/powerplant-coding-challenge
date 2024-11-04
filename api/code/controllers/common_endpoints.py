"""
    This module defines common utility API endpoints for the Powerplant Coding Challenge API.
    It includes functions to check the application's version, root, and health status.

    Endpoints:
    - GET /: Root endpoint returning appname and version
    - GET /version: Retrieve the current version of the application
    - GET /healthz: Perform a health check of the application

"""

from datetime import datetime

import logging

from fastapi import HTTPException

from config import appconfig
from config.constants import BS_LOGGER
from models.responses import HealthResponse
from fastapi import APIRouter

commons_router = APIRouter()


@commons_router.get("/", include_in_schema=False)
def get_root():
    logging.getLogger(BS_LOGGER).info("Request received in /")
    return {"appname": appconfig.APP_NAME, "version": appconfig.APP_VERSION}


@commons_router.get("/version", include_in_schema=False)
def get_version():
    logging.getLogger(BS_LOGGER).info("Request received in /version")
    return appconfig.APP_VERSION


@commons_router.get("/healthz", response_model=HealthResponse, include_in_schema=False)
def check_health():
    logging.getLogger(BS_LOGGER).info("Request received in /healthz")
    try:
        current_time = datetime.now().strftime("%#m/%#d/%Y %#I:%M:%S %p")
        # TODO run some heath checks
        return HealthResponse(status="healthy", current_time=current_time)
    except Exception as e:
        raise HTTPException(status_code=500, detail=f"System Error: {e}")

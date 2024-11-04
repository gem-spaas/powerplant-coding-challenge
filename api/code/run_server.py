"""
    This is the Entry Point of the API. By default, we always disabled the rdocs, but
    using FEATURE_ENABLE_SWAGGER_UI we can enable the UI for DEV/TESTING Environments.

    As any Docker Style API we want to be Environment Agnostic, so, all the configuration
    must be done using ENV.

    The configuration for system and API will be on the appconfig module and we want to prevent
    any start without these configuration files.

    To Enable a feature you must use YES (all in capitals) and NO or not set to Disable., i.e,
    to enable the feature FEATURE_ENABLE_SWAGGER_UI, you must export as:

        $ export FEATURE_ENABLE_SWAGGER_UI=YES

"""

import uvicorn
import logging

from fastapi import FastAPI

from controllers.common_endpoints import commons_router
from controllers.productionplan_endpoints import productionplan_router
from config import appconfig
from config.constants import BS_LOGGER

if appconfig.FEATURE_ENABLE_SWAGGER_UI == "YES":
    if "" == appconfig.SWAGGER_URL_PREFIX:
        root_path_value = ""
    else:
        root_path_value = f"/{appconfig.SWAGGER_URL_PREFIX}"
    app = FastAPI(
        root_path=root_path_value,
        openapi_url="/spec",
        docs_url="/swagger/index.html",
        redoc_url=None,
    )
else:
    app = FastAPI(openapi_url="/spec", docs_url=None, redoc_url=None)

app.include_router(productionplan_router)
app.include_router(commons_router)

if __name__ == "__main__":
    uvicorn.run(app, host="0.0.0.0", port=8888)
    logging.getLogger(BS_LOGGER).info("Powerplan Coding Challenge API is running!")

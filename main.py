import logging

import uvicorn
from fastapi import FastAPI

import routers

logging.basicConfig(level=logging.WARNING)


def main():
    app = FastAPI(
        title="Engie's Powerplant challenge",
        description="Zoltan Bak's solution for Engie's challenge",
        version="0.1.0"
    )

    # The "routers.productionplan_router" - I import and use it for readability reasons. Earlier conventions I used,
    # happy to change it if needed
    app.include_router(routers.productionplan_router)

    host, port = "0.0.0.0", 8888
    logging.info(f"Starting up server on {host}:{port}, check out: http://{host}:{port}/docs")

    uvicorn.run(app, host=host, port=port)


if __name__ == '__main__':
    main()

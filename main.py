import uvicorn
from core.api import productionApp
from core.utils import logger,get_config


def main():
    logger.info("Starting Power Plant Production Planning API")
    config = get_config()
    logger.info(f"Server configuration:")
    logger.info(f"\tHost: {config['API']['host']}")
    logger.info(f"\tPort: {config['API']['port']}")
    logger.info(f"\tProtocol: {config['API']['protocol']}")
    logger.info(f"\tEndpoint: {config['API']['endpoint']}")

    try:
        # Run the server
        uvicorn.run(
                productionApp,
                host = config['API']['host'],
                port = config['API']['port'],
                )
    except Exception as e:
        logger.critical(f"Failed to start server: {str(e)}")
        raise

if __name__ == "__main__":
    main()



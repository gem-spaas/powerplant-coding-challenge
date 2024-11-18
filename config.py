class Config:
    """Set Flask configuration vars."""

    DEBUG = False


class DevelopmentConfig(Config):
    """Development configuration.

    Args:
        Config: The base configuration class.
    """

    DEBUG = True
    ENV = "development"


class ProductionConfig(Config):
    """Production configuration.

    Args:
        Config: The base configuration class.
    """

    DEBUG = False
    ENV = "production"

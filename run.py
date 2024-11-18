from app import create_app
from config import ProductionConfig, DevelopmentConfig

app = create_app(config_class=ProductionConfig)
# For development tests we should change ProductionConfig to DevelopmentConfig
# app = create_app(config_class=DevelopmentConfig)


if __name__ == "__main__":
    app.run(host="0.0.0.0", port=8888)

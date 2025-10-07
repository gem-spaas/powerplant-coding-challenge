from pydantic_settings import BaseSettings


class Settings(BaseSettings):
    API_VERSION: int = 1


settings = Settings()

import pytest

from app.main import create_app


@pytest.fixture
def application():
    return create_app()

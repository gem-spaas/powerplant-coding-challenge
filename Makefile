run:
	poetry run uvicorn app.main:app --reload --port 8888

test:
	poetry run pytest

lint:
	poetry run ruff check .

format:
	poetry run ruff format .
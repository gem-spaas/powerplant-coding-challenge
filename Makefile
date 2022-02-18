start:
	uvicorn powerplant.main:app --reload --port 8888

lint:
	black .

tests:
	pytest

dc:
	docker-compose up

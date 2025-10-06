# Variables

DOCKER_COMPOSE=docker-compose

help:
	@echo "Usage:"
	@echo "make [command]"
	@echo ""
	@echo "Commands:"
	@echo "	dev			Run the development environment"
	@echo "	lint		Lint & correct errors"
	@echo "	build		Build docker images"
	@echo "	run			Run the API"
	@echo "	stop		Stop the API"
	@echo "	test		Run unit & integration tests"

# Targets
.PHONY: dev ruff test run build stop

dev:
	uv run uvicorn main:app --reload

lint:
	black .
	uv run ruff check . --fix

build:
	$(DOCKER_COMPOSE) build

run:
	$(DOCKER_COMPOSE) up

stop:
	$(DOCKER_COMPOSE) down

test:
	pytest -v
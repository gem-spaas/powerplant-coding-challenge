.PHONY: help
help: # Show help for each of the Makefile target.
	@grep -E '^[a-zA-Z0-9 _]+:.*#'  Makefile | sort | while read -r l; do printf "\033[1;32m$$(echo $$l | cut -f 1 -d':')\033[00m:$$(echo $$l | cut -f 2- -d'#')\n"; done

install_requirements: # Installl packages requirements for run main code
	python -m pip install -r requirements.txt

install_requirements_dev: # Installl packages requirements for dev/test and linter
	python -m pip install -r requirements_dev.txt

linter: # Flake8 checks
	flake8 .

run_dev: # Run fastapi app in development mode
	fastapi dev main.py --port 8888

run: # Run fastapi app in production mode
	fastapi run main.py --port 8888

build_image: # Build docker image
	docker build -t powerplant:latest .

run_image: # Run fastapi using docker image
	docker container run -p 8888:8888 powerplant:latest
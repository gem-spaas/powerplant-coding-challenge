# Powerplant Coding Challenge

## Table of Contents 
1. [Challenge Summary](#challenge-summary)
2. [About the Team](#about-the-team)
3. [Solution Overview](#solution-overview)
4. [Repository Setup](#repository-setup)
5. [Run Locally (with Poetry)](#run-locally-with-poetry)
6. [Run with Docker](#run-with-docker)
7. [Make a Test Request with cURL](#make-a-test-request-with-curl)
8. [JSON Payload Validation](#json-payload-validation)
9. [Technical Highlights](#technical-highlights)
10. [Handling Unsolvable Load Scenearios](#unsolvable-scenarios)

---

## Challenge Summary
This project solves the SPaaS coding challenge which consists of building a REST API that calculates the optimal production plan for a set of powerplants. Given the total required electrical load and the cost of fuels, the goal is to allocate production across powerplants in the most cost-efficient way while respecting constraints such as Pmin, Pmax and efficiency.

## About the Team
This challenge was proposed by the Short-term Power as-a-Service (SPaaS) team within GEM (Global Energy Management) at ENGIE. The team operates on day-ahead and intraday energy markets, ensuring optimal production and grid balancing in real-time conditions.

---

## Solution Overview
The API is developed in Python using FastAPI. It exposes a single POST endpoint `/productionplan`, which receives a JSON payload describing the load, available fuels, and a list of powerplants. The API computes the optimal production plan by prioritizing cheaper power sources and respecting technical constraints.

Key aspects of the implementation include:
- Custom algorithm to compute power allocation (no external solvers used).
- Validation of the input payload structure and powerplant constraints.
- Test coverage for edge cases and invalid inputs.

## Repository Setup
To get started, first clone the repository:

```bash
git clone https://github.com/gem-spaas/powerplant-coding-challenge.git
cd powerplant-coding-challenge
```

Then, make sure you are on the correct branch:

```bash
git checkout cc_anafernandez
```

Then follow the steps below to install dependencies and run the app.

## Run Locally (with Poetry)

1. **Install Poetry** (if not already installed):
```bash
curl -sSL https://install.python-poetry.org | python3 -
```

2. **Install dependencies**:
```bash
poetry install
```

3. **Run the main script manually**:
```bash
poetry run python -m main
```

4. **Start the FastAPI server**:
```bash
poetry run uvicorn src.app:app --reload --port 8888
```

Access the API docs at: [http://127.0.0.1:8888/docs](http://127.0.0.1:8888/docs)

## Run with Docker

1. **Build the Docker image**:
```bash
docker build -t production-plan .
```

2. **Run the container**:
```bash
docker run -p 8888:8888 production-plan
```

API will be available at: [http://localhost:8888/docs](http://localhost:8888/docs)

## Make a Test Request with cURL
To test the API with a sample payload, run:
```bash
curl -X POST "http://127.0.0.1:8888/productionplan"      -H "Content-Type: application/json"      -d @example_payloads/payload1.json
```
Ensure the file `payload1.json` exists inside the `example_payloads` directory at the root of the project.

## JSON Payload Validation
A custom validation function (`validate_payload`) ensures that the incoming JSON has the correct structure and valid values:
- Required keys: `load`, `fuels`, `powerplants`
- `load` must be a positive number
- `fuels` must be a dictionary
- `powerplants` must be a non-empty list with valid fields (`name`, `type`, `efficiency`, `pmin`, `pmax`)
- Additional checks: `pmin <= pmax`, `0 < efficiency <= 1`

## Technical Highlights

### Tools and Technologies
- Python 3.10
- FastAPI
- Poetry (dependency management)
- Uvicorn (ASGI server)
- Docker

### Best Practices Applied
- Input and output types explicitly defined in function signatures.
- Code separated into logical modules (`models`, `planner`, `validators`, `app`).
- Validations are decoupled from business logic.
- Errors are handled gracefully using FastAPI's `HTTPException`.
- Dockerized for portability and deployment.
- API automatically documented via FastAPI at `/docs`.

## Handling Unsolvable Load Scenearios

In some edge cases, it may be impossible to satisfy the requested load using the available powerplants. This typically happens when the remaining load is lower than the minimum production (`pmin`) of all remaining available powerplants. Since powerplants cannot be partially activated below their `pmin`, no valid combination can fulfill the load.

In such cases, the algorithm raises an `HTTP 400` error to indicate that the production plan is infeasible.

### Example: `payload4.json`

An example of this scenario is included in `example_payloads/payload4.json`. In this case, the requested load is very low and all available powerplants have a `pmin` greater than the remaining load, with no wind contribution. This triggers the error condition.

Example response:

```bash
{
  "detail": "Unable to meet the required load with given powerplant constraints."
}
```

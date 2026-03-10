# Powerplant Coding Challenge

REST API built with FastAPI to compute a production plan for a set of powerplants based on the required load, fuel prices, and plant constraints.

The API exposes a `POST /productionplan` endpoint that returns how much power each plant should produce while following a merit-order dispatch approach and respecting operational limits.

---

## Technologies used

- Python 3.12
- FastAPI
- Pydantic
- Uvicorn
- Pytest
- Ruff
- Poetry
- Docker

---

## Project structure

```text
app/
├── api/                # FastAPI routes
├── data/               # Request/response schemas and internal profiles
├── services/           # Business logic (production planner)
├── utils/              # Shared app configuration
└── main.py             # FastAPI application entrypoint

tests/                  # Unit and API tests
Dockerfile              # Container image definition
Makefile                # Common development commands
README.md
```

---

## Requirements

To run the project locally you need:
- Python 3.12
- Poetry

To run with containers you need:
- Docker

## Run locally

### 1. Install dependencies

```bash
poetry install
```

### 2. Run the API

```bash
make run
```

### 3. Open the API documentation

Swagger UI:

```text
http://localhost:8888/docs
```

---

## Run with Docker

### 1. Build the image

```bash
docker build -t powerplant-planner .
```

### 2. Run the container

```bash
docker run --rm -p 8888:8888 powerplant-planner
```

### 3. Open the API documentation

```text
http://localhost:8888/docs
```

---

## How the solution works

The planning logic is implemented with a merit-order dispatch approach:

1. Each powerplant is transformed into an internal profile with:

   * marginal production cost
   * `pmin`
   * `pmax`

2. Plants are sorted by marginal cost:

   * wind turbines are treated as zero-cost plants
   * gas-fired plants include fuel and CO2 costs
   * turbojets use kerosine cost

3. Production is allocated from the cheapest plant to the most expensive one.

4. When needed, previously assigned production can be rebalanced to satisfy `pmin` constraints of later plants.

5. The final result is returned as a list of `{name, p}` elements.

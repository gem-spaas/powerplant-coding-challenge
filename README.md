# Powerplant Coding Challenge

This project is a FastAPI application that calculates the optimal production plan to meet a given load with available power plants. The application is containerized using Poetry & Docker.

## Table of Contents

- [Installation](#installation)
- [Poetry](#poetry)
- [Docker](#docker)
- [Project Structure](#project-structure)

## Installation

### Prerequisites

- Docker
- Docker Compose (optional)
- Python 3.11 (if running locally without Docker)

### Clone the Repository

```sh
git clone https://github.com/your-username/powerplant-coding-challenge.git
cd powerplant-coding-challenge
```

## Poetry

### Setup with Poetry

1. Install Poetry:

    Follow the instructions on the [Poetry Installation](https://python-poetry.org/docs/#installation) page. For example, using the recommended installation script:

    ```sh
    curl -sSL https://install.python-poetry.org | python3 -
    ```

2. Install Dependencies:

    ```sh
    poetry install
    ```

3. Activate the Virtual Environment:
    ```sh
    poetry env activate
    ```

4. Run the API:
    ```sh
    uvicorn src.api:app --host 0.0.0.0 --port 8888
    ```

5. Run the tests (pytest):
    ```sh
    pytest src/tests/test_api.py
    ```

6. Example request:

    Use `curl` or any API client to send a POST request to the `/productionplan` endpoint:

    ```sh
    curl -X POST "http://localhost:8888/productionplan" -H "Content-Type: application/json" -d @payload.json
    ```


## Docker

### Setup with Docker

1. Build the Docker image:

    ```sh
    docker build -t powerplant-coding-challenge .
    ```

2. Run the Docker container:

    ```sh
    docker run -d -p 8888:8888 powerplant-coding-challenge
    ```

3. Example request:

    Use `curl` or any API client to send a POST request to the `/productionplan` endpoint:

    ```sh
    curl -X POST "http://localhost:8888/productionplan" -H "Content-Type: application/json" -d @payload.json
    ```

## Project Structure
```
project-root/
├── Dockerfile
├── README.md
├── requirements.txt
├── pyproject.toml
├── src/
│   ├── api.py
│   ├── tests/
│   │   ├── example_payloads/
│   │   │   ├── payload1.json
│   │   │   ├── response1.json
│   │   │   ├── payload2.json
│   │   │   ├── response2.json
│   │   │   ├── payload3.json
│   │   │   ├── response3.json
│   └── └── test_api.py
└── payload.json
```

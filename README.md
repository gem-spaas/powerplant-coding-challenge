## Info

powerplant-coding-challenge
Submission by Irvin Heslan

## Prerequisites

- Python 3.8 or higher
- Docker (optional)

## Installation
### Local Installation

1. Clone the repository:
```bash
git clone 
cd powerplant-coding-challenge
```

2. Create a virtual environment (optional but recommended):
```bash
python -m venv venv
source venv/bin/activate  # On Windows: venv\Scripts\activate
```

3. Install dependencies:
```bash
pip install -r requirements.txt


### Docker Installation

1. Clone the repository:
```bash
git clone 
cd powerplant-coding-challenge
```

2. Build the Docker image:
```bash
docker build -t powerplant-api .
```

## Configuration

The API configuration is stored by default in `config.yml`. Default settings:

```yaml
API:
  protocol: http
  host: 0.0.0.0
  endpoint: productionplan
  port: 8888
```

These settings can be overrided with Docker env variables.
Env variables will ALWAYS override setting file.

```
ENV DOCKER_ENV=1
ENV API_PROTOCOL=http
ENV API_HOST=0.0.0.0
ENV API_PORT=8888
ENV API_ENDPOINT=productionplan
EXPOSE 8888
```

or by setting up a custom config_file
```
ENV DOCKER_ENV=1
ENV CONFIG=mycustom_config.yml
```





## Usage

### Running Locally

1. Start the API server:
```bash
python main.py
```

The API will be available at `http://localhost:8888`

### Running with Docker

1. Run the container:
```bash
docker run -p 8888:8888 powerplant-api
```

The API will be available at `http://localhost:8888`


## API Endpoints

### GET /
Returns a simple message directing users to use the POST endpoint.

### POST /productionplan
Calculate the optimal production plan for power plants.

**Request Format:**
```json
{
    "load": float,
    "fuels": {
        "gas": float,
        "kerosine": float,
        "co2": float,
        "wind": float
    },
    "powerplants": [
        {
            "name": string,
            "type": string,
            "efficiency": float,
            "pmin": int,
            "pmax": int
        }
    ]
}
```

**Response Format:**
```json
{
    "powerplant-1": float,
    "powerplant-2": float,
    ...
}
```

## Logging

All the logs file are in logs/folder if the folder is not existing it will create the folder logs.
One log per day of run.


## Testing

To test the API with example payloads:

```bash
python ./tests/test.py
```
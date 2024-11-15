# Power Plant Coding Challenge

This project is based on a Fast API python code that calculates the optimal production plan for a set of Power Plants based on Load provisions and Energy type prices.

## Project Structure
powerplant-coding-challenge/
├── src/
│ ├── api/
│ │ └── main.py
│ └── example_payloads/
├── tests/
│ └── test.py
├── Dockerfile
├── README.md
└── requirements.txt


### Prerequisites

- Docker
- Python 3.8+
- Postman (for testing)

### Installation
## Build the Docker image:

$docker build -t powerplant-api .

## Run the Docker container exposing the referenced API Port 8888:

$docker run -p 8888:8888 powerplant-api


The API should be available at `http://localhost:8888`.

### Usage

Use Postman to send a POST request to `http://localhost:8888/productionplan`  using the example payloads that can be found in the `src/example_payloads/` directory.

Or optionally use Curl to send the Post Request (on the example below there is payload1 but there is also payload2 and payload3 to be used):

$curl -X POST -H "Content-Type: application/json" -d @src/example_payloads/payload1.json http://localhost:8888/productionplan

### Running Tests
To run the unit tests execute the following command on root directory:

$python3 -m unittest discover tests


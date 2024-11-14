# Production Plan API

This API calculates an optimal production plan for a set of power plants to meet a specified energy load, based on various input parameters like fuel costs, plant efficiency, and power limits. The resulting production plan is saved as a JSON file in the project directory.

## Project Overview

The Production Plan API is designed to receive energy load requirements and various power plant parameters. It calculates the most cost-effective distribution of energy across these plants to meet the requested load. This API returns a downloadable JSON file with the production plan, specifying the energy output for each plant.

### Project Structure

```bash
  project/
├── requirement.txt             
├── src/               # Folder containing source code
│   ├── production_plan.py  # Production planning algorithm
│   └── app.py             # Flask server for the application interface
└── test/              # Folder containing tests
    └── test_app.py       # Unit tests for the Flask application
```

- **`src/`**: This folder contains the source code of the application:
  - `production_plan.py`: This module implements the production algorithm. It includes calculation functions.
  - `app.py`: This module sets up the Flask server for the application. It exposes an API.

- **`test/`**: This folder contains unit tests for the application.
  - `test_app.py`: This file contains tests to verify the functionality of the Flask application. The tests check that the API routes work as expected and that the planning algorithm responds correctly to various requests.
- **requirements.txt**: Lists the required dependencies for the project.


## Getting Started

### Prerequisites

Before starting, ensure you have:
- **Python 3.8** or higher installed on your system.
- **pip** (Python package manager) installed for managing dependencies.

### Installation

1. **Clone or Download** the project files to your local machine.
2. **Install dependencies** by navigating to the project directory in your terminal and running:

   ```bash
   pip install -r requirements.txt

## Running the API

This section provides step-by-step instructions to run the Production Plan API, send a request, and verify the output.

### 1. Start the API

- Open a terminal in the project directory where `app.py` and `production_plan.py` are located.
- Run the following command to start the Flask API:

  ```bash
  python app.py

- If the server starts successfully, you should see output in the terminal indicating that the API is running, with a message similar to:

    ```bash
  Running on http://127.0.0.1:8888 (Press CTRL+C to quit)

### 2. Prepare the Input JSON File
- Create a JSON file named `payload.json` in the same directory as `app.py`. This file should contain the input data for the API, including details like the energy load, fuel costs, and power plant specifications.

### 3. Send a POST Request to the API
- Open another terminal window in the same directory where `payload.json` is located.

- Use the following `curl` command to send a `POST` request to the `/productionplan` endpoint:
    ```bash
    curl -X POST http://localhost:8888/productionplan -H "Content-Type: application/json" -d @payload.json

- This command sends the contents of `payload.json` as a POST request to the API.

### 4. Verify the Output

- After the request completes successfully, check the project directory. You should see a file named `Merouane_Hadouch_HEADMIND_production_plan_result.json`.

### 5. Tests

- To run the tests:
  ```bash 
  python -m unittest discover -s test


## Explanation of How the API Works

### API Endpoint 
- The API has one endpoint, `/productionplan`, which only accepts POST requests with JSON data as input.

### Calculation

- The `calculate_production_plan` function in `production_plan.py` processes this input data to calculate the optimal production levels for each plant. The goal is to distribute the energy output to meet the specified load in the most cost-effective way, taking into account each plant’s efficiency, minimum and maximum output limits, and fuel costs.

### Output

- The API saves the calculated production plan in a JSON file named `Merouane_Hadouch_HEADMIND_production_plan_result.json`.
- This file will specify each plant's production level in MWh

### Confirmation Response

- Once the calculation is complete and the file is generated, the API returns a confirmation message in JSON format, indicating that the output file has been successfully created.

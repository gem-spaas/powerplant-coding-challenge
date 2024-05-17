# Powerplant Coding Challenge

**Author:** Charles Frère  
**Company:** LittleBigCode

## Overview

This project aims to develop an API that calculates the optimal power output for each power plant to meet a given load while minimizing costs. The algorithm considers various factors such as fuel prices, CO2 costs, and wind availability.

## Algorithm Explanation

### First Algorithm

This algorithm uses a merit-order dispatch approach to minimize costs:
1. **Cost Calculation:** Each power plant's cost per MWh is calculated based on its fuel type and efficiency.
2. **Sorting:** Power plants are sorted by cost, prioritizing cheaper sources.
3. **Load Distribution:** The load is distributed among the power plants starting with the cheapest until the load is met or all plants are utilized.
4. **Constraints Handling:** Minimum and maximum generation constraints for each plant are respected during load distribution.

### Second Algorithm

To improve upon the first algorithm, especially in cases like `payload2.json`, a dynamic programming approach is used:
1. **Precision Handling:** The load and power values are scaled to integers to manage decimal precision.
2. **DP Table Initialization:** The DP table is initialized with infinity, and `dp[0][0]` is set to 0.
3. **Filling the DP Table:** Iterate through each power plant and update the DP table by considering the cost of producing each power value within the plant's range.
4. **Backtracking:** Determine the optimal power distribution by backtracking through the DP table, ensuring the power values are scaled back to their original precision.

The second algorithm provides a more precise solution but is slower compared to the first algorithm.


## Deployment Instructions

You can deploy this application either locally or using Docker. Docker is recommended for ease of deployment.

### Local Deployment 

1. Make sure you python3.11 installed
```bash
python --version
```

2. Create and activate a virtual environment:
```bash
python -m venv .venv
source .venv/bin/activate
```

3. Install necessary packages:
```bash
pip install -r requirements.txt
```

4. Run the application locally:
```bash
python src/main.py
```

For the dynamic programming approach:
```bash
python src/main_dynamic_programming.py
```

To test the API:
```bash
curl -X POST -H "Content-Type: application/json" -d @example_payloads/payload3.json http://0.0.0.0:8888/productionplan
```

### Docker Deployment

1. Build the Docker image:
```bash
docker build -t powerplant-api .
```

2. Run the application in a container
```bash
docker run -p 8888:8888 powerplant-api
```

### Test the API

Once the container is up and running, you can test the API with curl:
```bash
curl -X POST -H "Content-Type: application/json" -d @example_payloads/payload3.json http://0.0.0.0:8888/productionplan
```

or via the Swagger UI
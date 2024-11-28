# Production Plan Calculator

This Flask application calculates the production plan for power plants based on provided load and fuel costs.

## Prerequisites

- Python 3.x installed
- pip (Python package installer)

## Installation
Install Dependencies using pip from the directory requirements.txt is located:

pip install -r requirements.txt

## Running the Application
Start the Flask Application: Run the following command in your terminal from the directory where app.py is located :

python app.py

This will start the Flask server, which listens on http://127.0.0.1:8888.

## Sending a POST Request
To calculate the production plan, you need to send a POST request with a JSON payload.
You can use the curl command to send a POST request with the JSON payload

For exemple, if you have a json file named payload3.json and containing all your inputs :

curl -X POST http://127.0.0.1:8888/production_plan -H "Content-Type: application/json" -d @payload3.json

## Viewing Results
The results are saved in a "production_plan_results.json" file.

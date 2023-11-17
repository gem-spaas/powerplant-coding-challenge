# powerplant-coding-challenge


## Description
Solution by Roland Takacs.

## Run With Docker

Download the source code and go to the root of the repository.<br>
```
docker build -t production_plan_api_server .
docker run -d -p 8888:8888 production_plan_api_server
```
It will start a container in the background.

## Run Locally

Download the source code and go to the root of the repository.<br><br>
Create the virtual environment and install the requirements:
```
python3 -m venv env
source env/bin/activate
pip3 install -r requirements.txt
```
Run the app (by default it will run on port 8888):
```
python3 src/api_server.py
```

## Run Tests

Run all the tests from the repository root:
```
python3 -m unittest tests/tests.py
```

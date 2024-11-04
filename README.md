# powerplant-coding-challenge

## Goal 
The goal of this project is to create a REST API that could return a json containing list of power plants
predicted production for a given predicted load, and the given resources of each power plants. 

### Algorithm strategy
Here, the strategy was quite simple.
We compute two strategies

First strategy : 
1. We use the wind turbines in priority
2. We use turbo jet power plants in second priority ranked by ascending pmax
3. We compute gas power plants in third priority ranked by ascending min_cost


Second strategy : 
1. We use the wind turbines in priority
2. We use fossil fuel power plants in second priority ranked by descending pmax

Then, we see which one use the less power plants. If the Second strategy use less power plants (most likely to happen because we rank by highest pmax first), 
we are choosing this strategy. If there is an equal amount of power plant used, we tend to choose the First stragégy because it will use in priority turbo jets which don't have a pmin and have less carbon impact.


## Pre-Requisites
### OS
This template is optimized for UNIX environments. 

If you are using a windows environment, you should use WSL (>=2).

### Software
[Poetry](https://python-poetry.org/docs/): virtual environment and package manager

### Run locally 
1. Install packages with the following command
```bash
poetry install
```

2. run the script with the following command
```bash
poetry run python3 src/powerplant_coding_challenge/main.py
```

3. test the route in a diffrent environment (e.g. a separate notebook)
```python
import json
import requests

url = "http://localhost:8888/productionplan"
with open('path_to_example_payload/payload3.json') as f:
    d = json.load(f)

# Send as JSON payload
req = requests.post(url, json=d)
print(req.status_code, req.json())
```
You should find example of payloads in the folder [example_payloads](example_payloads)


### Run with Docker
1. build the docker image from the Dockerfile

`````bash
docker build -t powerplant-coding-challenge-resolution .
`````
2. run the image and bind it to the port 8888
`````bash
docker run -p 8888:8888 powerplant-coding-challenge-resolution:latest
`````
3. test the route in a diffrent environment (e.g. a separate notebook)
```python
import json
import requests

url = "http://localhost:8888/productionplan"
with open('path_to_example_payload/payload3.json') as f:
    d = json.load(f)

# Send as JSON payload
req = requests.post(url, json=d)
print(req.status_code, req.json())
```
You should find example of payloads in the folder [example_payloads](example_payloads)


## TO DO 
. adding unit tests


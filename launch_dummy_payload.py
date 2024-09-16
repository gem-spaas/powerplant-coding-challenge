
import requests
import json
from json import JSONDecodeError

file_path = '' # Your filepath to payload.json

data = {}

try:
    with open(file_path, 'r') as file:
        data = json.load(file)
    headers = {'Content-Type': 'application/json'}

    res = requests.post(
        'http://127.0.0.0:8888/production_plan',
        json.dumps(data), headers=headers)
    if res.ok:
        print(res.json())
        
except JSONDecodeError:
    print("Given Json file is not valid.")


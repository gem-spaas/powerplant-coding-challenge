import requests
import json
import sys
import os



project_root = os.path.abspath(os.path.join(os.path.dirname(__file__), '..'))
sys.path.insert(0, project_root)
from core.utils import load_config

def prepare_payload(path:str  = ''):
    # Get config or Env var
    config = load_config(os.path.join(project_root, 'config.yml'))
    protocol = os.getenv('API_PROTOCOL', config['API']['protocol'])
    host = os.getenv('API_HOST', config['API']['host'])
    port = int(os.getenv('API_PORT', config['API']['port']))
    endpoint = os.getenv('API_ENDPOINT', config['API']['endpoint'])
    url = f"{protocol}://{host}:{port}/{endpoint}"

    with open(path, 'r') as file:
        payload = json.load(file)
    return url,payload

def send_payload(url,payload):
    try:
        req = requests.post(url, json=payload)
        req.raise_for_status()
        return req
    except requests.exceptions.RequestException as e:
        print(f"Error making request: {e}")
        if hasattr(e, 'response') and e.response is not None:
            print(f"Response text: {e.response.text}")



if __name__ == '__main__':
    payload_path = os.path.join(project_root, 'example_payloads', 'payload3.json')
    api_url,json_payload = prepare_payload(path=payload_path)

    # Send classic payload
    resp = send_payload(api_url,json_payload).text
    print("Response :")
    print(resp,end="\n"*2)

    # Send payload with co2
    resp_with_co2 = send_payload(f"{api_url}?with_co2=1",json_payload).text
    print("Response with CO2 at 0.3:")
    print(resp_with_co2)
<a name="main"></a>
# __GEM Powerplant Challenge solution by Pablo Cazallas Gonzalez__

This is a Python microservice to achieve the power supply goals described in the challenge.  
Following are the instructions to get the service up and running:

Python only:
```
$ python3 -m venv venv
$ source venv/bin/activate
$ python3 -m pip install -r requirements.txt
$ ./app.py
```

Python inside Docker:
```
$ docker compose up -d
$ curl -X POST -H 'Content-Type: application/json' -d @/path/to/payload.json http://localhost:8888/productionplan 
```

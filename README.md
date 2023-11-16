<a name="main"></a>
# __GEM Powerplant Challenge solution by Pablo Cazallas Gonzalez__

This is a Python microservice to achieve the power supply goals described in the challenge.  


<a name="howto"></a>
## Following are the instructions to get the service up and running.
You should use one of them at a time only.

- Start the service (standalone Python):
```
$ python3 -m venv venv
$ source venv/bin/activate
$ python3 -m pip install -r requirements.txt
$ ./app.py
```

- Start the service (inside Docker):
```
$ docker compose up -d
$ curl -X POST -H 'Content-Type: application/json' -d @/path/to/payload.json http://localhost:8888/productionplan 
```

Test the service:
```
$ curl -X POST -H 'Content-Type: application/json' -d @/path/to/payload.json http://localhost:8888/productionplan 
```

Stop the service (Python):
```
$ Ctrl+C
```

Stop the service (Docker):
```
$ docker compose down
```

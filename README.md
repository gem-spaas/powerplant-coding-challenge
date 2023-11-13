# powerplant-coding-challenge

## Author: Beto Cerutti

## Install via docker
To build the image cd into the project root and the type:
```docker build -t power-planner```.
Then run it with:
```docker run -p 8888:8888 power-planner```.
Available at:
```http://localhost:8888/docs```.

## Install manually
Change directory into project root then:

```python3 -m venv venv```.
```source venv/bin/activate```.
```pip install -r requirements.txt```.
```cd app```
Then run it with:
```uvicorn main:app --port 8888```.
Available at:
```http://localhost:8888/docs```.


## Comments

The reporting in costs and Co2 emmissions was intentionally 
left as print statements so it desn't impact the returned data 
structure.

A LIP approach was intentionally not used as pointed in the requirements.

I am aware that in a more realistic scenario other variables will take place
and a more robust solution will be needed.

The solution is accomplish using OOP in order to improve readability, testability
and maintainability.
# powerplant-coding-challenge


## Welcome !

Thank you very much for your time and consideration.

This is a very easy to deploy Django project that exposes an API endpoint to calculate the production plan. There are no models or database. Just create a virtualenv, install the requirements and run the server:

```commandline
virtualenv venv -p `which python3.8`
. venv/bin/activate
cd powerplants
pip install -r requirements.txt
python manage.py runserver 0.0.0.0:8888
```

To run the tests:

```commandline
python manage.py test prodplan/tests
```

You can also use the provided Dockerfile:

```commandline
docker build -t powerplants .
docker run -p 8.8.8.8:8.8.8.8 powerplants
```

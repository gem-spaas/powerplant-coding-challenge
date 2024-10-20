# Python version

- Python 3.11

# Requirements

Before running the app, it is necessary to install dependencies:

```
python -m pip install -r requirements.txt
```

Plus: for dev packages, it is necessary to install dev packages

```
python -m pip install -r requirements_dev.txt
```

# Running the app

After install the requirements, run the app with one of the following commands:

Fast API DEV mode
```
fastapi dev main.py --port 8888
```

Fast API Production mode
```
fastapi run main.py --port 8888
```

Starting app manually
```
python main.py
```

# Interactive API docs

Accessing the path `/docs` (ex `http://localhost:8888/docs`) it is possible to access the interactive API docs

# Docker image

It is possible to execute app using Docker.

Build image:

```
docker build -t powerplant:latest .
```

Run image
```
docker container run -p 8888:8888 powerplant:latest
```

# Makefile

This repository contains a Makefile to help it with some commands
# ⚡ PowerPlant Coding Challenge – API

This repository contains a FastAPI-based backend built as part of the PowerPlant coding challenge. The API receives powerplant data (according to specifications) and calculates optimal production levels based on constraints.

---

## Tech Stack

- Python ^3.11
- FastAPI
- Poetry
- Docker

---

## 🚀 Quickstart

### Launch with Docker

First, make sure you’re in the project root which is ..code/ where the `Dockerfile` is located:

```bash
# Clone repository
git clone https://github.com/your-username/powerplant-coding-challenge.git
cd powerplant-coding-challenge/code

# Build image
docker build -t powerplant-api .

# Start API
docker run -p 8888:8888 powerplant-api
```

### 📦 Launch without Docker

Requirements:
- Poetry
- Python ^3.11

```bash
# Clone repository
git clone https://github.com/your-username/powerplant-coding-challenge.git
cd powerplant-coding-challenge/code

# Install dependencies 
poetry install

# Run API
poetry run uvicorn "app.main:app" --host 0.0.0.0 --port 8888 --reload
```

The API will now be running at:  [http://localhost:8888](http://localhost:8888)




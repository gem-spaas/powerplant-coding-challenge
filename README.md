# 🚀 powerplant-coding-challenge

This API allows getting how much load given powerplants should generate based on key parameters.

## ✨ Tools & Libraries

- **FastAPI** for asynchronous API implementation and documentation
- **Pydantic** for input validation of requests and responses and environment settings
- **Pytest** for unit and integration tests
- **Ruff & Black** for code quality, error detection and PEP8 standard adherence
- **Docker** for containerization
- **Makefile** for automating common and useful commands
- **DeepSeek AI** for documentation and best practices

## 🏗️ Project Structure

```bash
├── powerplant-coding-challenge/
│ ├── controllers/ --> API endpoints and request handling logic
│ ├── schemas/ --> Request and response validation DTOs
│ ├── factories/ --> Factory method to abstract entity creations based on powerplant type
│ ├── services/ --> Business logic
│ ├── models/ --> Models definition
├── settings.py --> Constants and environment variables
├── main.py --> Entrypoint
```

## 🚀 Getting started

## Running with Docker

### Prerequisites

- [Docker](https://docs.docker.com/get-docker/)
- [Make](https://www.gnu.org/software/make/)
- [Docker Compose](https://docs.docker.com/compose/install/)

### Installation

1. **Clone the repository**
   ```bash
   git clone git@github.com:gem-spaas/powerplant-coding-challenge.git
   git checkout aboutat-solution
   ```

2. **Build**
    ```bash
   make build
   ```
3. **Run**
    ```bash
   make run
   ```

## Running locally

### Prerequisites
- [UV](https://docs.astral.sh/uv/)
- [Python 3.13](https://www.python.org/downloads/release/python-3130/)

1. **Clone the repository**
   ```bash
   git clone git@github.com:gem-spaas/powerplant-coding-challenge.git
   git checkout aboutat-solution
   ```
   
2. **Install dependencies**
   ```bash
    uv sync --frozen
   ```
   
3. **Run the development server**
   ```bash
    make dev
   ```

## 📚 API Documentation

- **Swagger UI**: http://localhost:8888/docs

- **ReDoc**: http://localhost:8888/redoc

## 🧪 Testing

**Run tests**

```bash
   make test
   ```



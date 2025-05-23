FROM python:3.11.5-slim
RUN apt-get update && apt-get install -y curl build-essential && apt-get clean

# Install Poetry
ENV POETRY_VERSION=1.8.5
RUN curl -sSL https://install.python-poetry.org | POETRY_VERSION=$POETRY_VERSION python3 -

# Poetry to PATH
ENV PATH="/root/.local/bin:$PATH"

# Run App on port 8888
WORKDIR /app
COPY pyproject.toml poetry.lock* ./
RUN poetry install --no-root --no-interaction
COPY . .
EXPOSE 8888

# Start app
CMD ["poetry", "run", "uvicorn", "app.main:app", "--host", "0.0.0.0", "--port", "8888", "--reload"]
FROM python:3.10-slim

WORKDIR /app

COPY pyproject.toml poetry.lock* /app/
RUN pip install poetry && poetry install --no-root

COPY ./src /app/src
COPY example_payloads /app/example_payloads

EXPOSE 8888

CMD ["poetry", "run", "uvicorn", "src.app:app", "--host", "0.0.0.0", "--port", "8888"]

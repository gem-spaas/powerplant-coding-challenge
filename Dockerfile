# Dockerfile to run the API on port 8888 using an application factory
FROM python:3.13-slim

WORKDIR /app
COPY requirements.txt ./
RUN pip install --no-cache-dir -r requirements.txt

COPY app ./app
COPY example_payloads ./example_payloads
COPY README.md ./README.md

EXPOSE 8888
# Run FastAPI using the factory function create_app
CMD ["uvicorn", "app.main:create_app", "--factory", "--host", "0.0.0.0", "--port", "8888"]

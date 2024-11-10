# Use Python 3.8 slim image as base
FROM python:3.12-slim
WORKDIR /app
COPY requirements.txt .

RUN pip install --no-cache-dir -r requirements.txt
COPY . .

ENV DOCKER_ENV=1
ENV CONFIG=config.yml
ENV API_PROTOCOL=http
ENV API_HOST=0.0.0.0
ENV API_PORT=8888
ENV API_ENDPOINT=productionplan
EXPOSE 8888

CMD ["python", "main.py"]
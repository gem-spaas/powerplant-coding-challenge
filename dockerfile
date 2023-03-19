FROM python:3.11-slim
ENV PYTHONUNBUFFERED=true

WORKDIR /app
COPY requirements.txt /app/

RUN pip3 install -r requirements.txt

COPY . /app

EXPOSE 8888

CMD python3 main.py
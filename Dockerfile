FROM python:3.9.6

ENV PYTHONUNBUFFERED 1

RUN mkdir /app
WORKDIR /app

ADD requirements.txt requirements.txt
RUN pip install -r requirements.txt

ADD . /app/

ENV PYTHONPATH "${PYTHONPATH}:/app"

CMD ["python", "src/api_server.py"]
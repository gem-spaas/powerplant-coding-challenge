FROM python:3.9

ARG DOCKER_IP
ENV DOCKER_IP=${DOCKER_IP:-0.0.0.0}

WORKDIR /code

COPY ./*.py /code/
COPY ./requirements.txt /code/requirements.txt

RUN pip install --no-cache-dir --upgrade -r /code/requirements.txt

CMD uvicorn main:app --host ${DOCKER_IP} --port 8888

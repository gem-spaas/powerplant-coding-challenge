from python:3.9-slim-buster

ARG ROOT=/source
WORKDIR ${ROOT}

RUN pip install poetry==1.2.0

COPY pyproject.toml poetry.lock ${ROOT}/

RUN poetry config virtualenvs.create false
RUN poetry install 

COPY . .

ARG USER=engie
RUN useradd -s /bin/bash -m ${USER} 

RUN chown -R ${USER}:${USER} ${ROOT}

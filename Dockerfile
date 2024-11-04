FROM python:3.8

RUN apt update && apt install -y python3-pip

RUN pip install poetry
WORKDIR /run


COPY pyproject.toml ./
RUN poetry install

COPY . .
ENV PYTHONPATH="src"
EXPOSE 8888

CMD ["poetry", "run", "python3", "src/powerplant_coding_challenge/main.py"]
FROM python:3.11-alpine

WORKDIR /src

COPY requirements.txt main.py power_calculator.py models.py /src/

RUN python -m pip install -r requirements.txt

CMD ["fastapi", "run", "--port", "8888"]


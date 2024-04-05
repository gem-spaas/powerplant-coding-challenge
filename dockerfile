FROM python:3.9

COPY . .

RUN pip install -r requirements.txt

WORKDIR ./src

CMD ["python", "-m", "main"]

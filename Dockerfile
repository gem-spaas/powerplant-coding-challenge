FROM python:3.8-slim

WORKDIR /usr/src/app

COPY . .

WORKDIR /usr/src/app/powerplants

RUN pip install --no-cache-dir -r requirements.txt

EXPOSE 8888

CMD ["python", "manage.py", "runserver", "0.0.0.0:8888"]

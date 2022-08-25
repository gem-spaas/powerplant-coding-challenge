FROM python:3.9-slim-buster

COPY . .
RUN pip install -r requirements.txt
CMD [ "python3", "-m" , "flask", "run", "--port=8888", "--host=0.0.0.0"]

FROM python:3.11-slim

COPY . .
RUN pip install --upgrade pip
RUN pip install -r app/requirements.txt

EXPOSE 8888

CMD ["uvicorn", "app.main:app", "--port", "8888", "--reload"]

FROM python:3.10-slim

# Just for Python Execution
ENV PYTHONDONTWRITEBYTECODE 1
ENV PYTHONUNBUFFERED 1
ENV PYTHONPATH "${PYTHONPATH}:/powerplanner"


WORKDIR /powerplanner
COPY code/ /powerplanner/
RUN pip install --no-cache-dir -r requirements.txt


EXPOSE 8888

CMD ["uvicorn", "run_server:app", "--host", "0.0.0.0", "--port", "8888"]

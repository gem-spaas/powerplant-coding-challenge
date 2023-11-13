# Use an official Python image as a base image
FROM python:3.10-slim

# Install dependencies
RUN pip install fastapi uvicorn

# Set the working directory in the container
WORKDIR /app

# Copy app dir into app dir in container
COPY ./app /app

# Make port 8888 available to the world outside this container
EXPOSE 8888

# Run the command to start uvicorn
CMD ["uvicorn", "main:app", "--host", "0.0.0.0", "--port", "8888"]

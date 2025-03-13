# Use the official Python 3.11 image from the Docker Hub
FROM python:3.11-slim

# Install curl and other necessary dependencies
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Copy the requirements file into the container
COPY requirements.txt /usr/src/app/requirements.txt

# Set the working directory to /usr/src/app
WORKDIR /usr/src/app

# Install required packages
RUN pip install --no-cache-dir -r requirements.txt

# Copy the contents of the src directory into the container's /usr/src/app directory
COPY src/ .

# Expose the port the app runs on
EXPOSE 8888

# Command to run the FastAPI application
CMD ["uvicorn", "api:app", "--host", "0.0.0.0", "--port", "8888"]
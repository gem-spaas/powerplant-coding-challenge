# Use a specific, stable Python version with a lightweight base image for faster builds and smaller image size
FROM python:3.9-slim

# Prevent Python from writing .pyc files and enable unbuffered output to ensure logs appear in real-time
ENV PYTHONDONTWRITEBYTECODE=1 \
    PYTHONUNBUFFERED=1

# Define the working directory inside the container where the app code will reside
WORKDIR /app

# Copy only the requirements file first to utilize Docker cache efficiently for dependency installation
COPY requirements.txt .

# Install Python dependencies without cache to reduce image size
RUN pip install --upgrade pip && \
    pip install --no-cache-dir -r requirements.txt

# Copy the rest of the application code into the container
COPY . .

# Expose the port that the application will listen on (should match the port used by the app server)
EXPOSE 8888

# Define the default command to run the app with Uvicorn server, listening on all interfaces and the specified port
CMD ["uvicorn", "app.main:app", "--host", "0.0.0.0", "--port", "8888"]

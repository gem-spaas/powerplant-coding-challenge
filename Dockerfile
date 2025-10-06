FROM python:3.13-slim

WORKDIR /app

# Install uv.
COPY --from=ghcr.io/astral-sh/uv:latest /uv /uvx /bin/

# Copy the requirements file and install dependencies
COPY pyproject.toml uv.lock ./
RUN uv sync --no-dev --frozen --no-cache

# Copy the application files into the container.
COPY . .

# make sure Python knows about /app
ENV PYTHONPATH=/app

# Run the application.
CMD [".venv/bin/fastapi", "run", "--host", "0.0.0.0"]
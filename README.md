# powerplant-coding-challenge

## Run with docker

Clone the repository

Navigate to the solution folder

Build the image docker (docker desktop should be previously installed) cmd: docker build -t challenge .

Run build image : docker run -p 8888:8080 challenge

## Run with visual studio and dotnet

Open the solution file GEM.PowerPlant.sln (visual studio and dotnet core 8 sdk must be installed previusly)

Build and run the solution over http

## What is missing
Logging: Serilog can be installed and configured to log to the console, to a file or to an Elasticsearch index

Healthcheck: healthCheck middleware can be configured

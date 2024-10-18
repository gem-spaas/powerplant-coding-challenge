# Power Plant Production Plan API (.NET)

This is a REST API built with ASP.NET Core that calculates the production plan for power plants based on the given load and fuel costs.

## Requirements

- .NET 8.0 SDK 
- Docker (optional, for containerized deployment)

## Getting Started

### Clone and Setup

1. Open a terminal and clone the repository:
   ```
   git clone https://github.com/tomvedewe/powerplant-coding-challenge.git
   ```
2. Navigate to the project directory:
   ```
   cd powerplant-coding-challenge
   ```
## Building and Running the Application

### Using .NET CLI

1. Build the project:
   ```
   dotnet build
   ```
2. Navigate to the folder with the csproj file
   ```
   cd GemPowerplant.CodingChallenge
   ```
3. Run the application:
   ```
   dotnet run
   ```
3. By default, the API will be available at `http://localhost:8888`

### Using Docker

1. Clone this repository
2. Navigate to the project directory
   ```
   cd powerplant-coding-challenge
   ```
3. Build the Docker image:
   ```
   docker build -t powerplant-api .
   ```
4. Run the Docker container:
   ```
   docker run -d -p 8888:8888 --name powerplant-api-container powerplant-api
   ```
5. The API will be available at `http://localhost:8888`

#### Disclaimer:
It is possible the build fails and returns the following error: 

ERROR: failed to solve: mcr.microsoft.com/dotnet/aspnet:8.0: failed to resolve source metadata for mcr.microsoft.com/dotnet/aspnet:8.0: failed to do request: Head "https://mcr.microsoft.com/v2/dotnet/aspnet/manifests/8.0": EOF

This is a network issue and trying the build again for a few times should eventually work. I tested on two different computers, one had to try the build for at least 10 time before it would succeed. On the other computer the build would succeed every time.

## Using the API

Send a POST request to `/ProductionPlan` with a JSON payload containing the load, fuels, and powerplants information. The API will return a JSON response with the calculated production plan.

Example using curl (when using Docker):
```
curl -X POST -H "Content-Type: application/json" -d @example_payloads/payload1.json https://localhost:8888/ProductionPlan
```

Replace `example_payloads/payload1.json` with the path to your JSON payload file.

## Running Unit Tests

### Using Visual Studio
1. Open the solution in Visual Studio
2. Open Test Explorer (Test > Test Explorer or Ctrl+E, T)
3. Click "Run All" to run all tests, or right-click specific tests to run them individually
4. Test results will appear in the Test Explorer window

### Using Command Line
Navigate to the test project directory and run:
```bash
# Run all tests
dotnet test

# Run tests with detailed output
dotnet test --logger "console;verbosity=detailed"

# Run specific test(s)
dotnet test --filter "FullyQualifiedName~ProductionPlanControllerTests.CalculateProductionPlan_ValidRequest_ReturnsOkResult"

# Run tests and generate coverage report (requires coverlet.collector package)
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=lcov /p:CoverletOutput=./lcov.info
```

## API Documentation

When running the application locally and in Docker, you can access the Swagger UI at `/swagger` to view and test the API endpoints.
# Powerplant Production Plan API

## Overview

This API calculates the power production plan for a set of power plants to meet a specified load. It determines how much power each power plant needs to produce while taking into account the cost of the energy sources (such as gas and kerosine) and the minimum (Pmin) and maximum (Pmax) production capacity of each power plant. The goal is to meet the power demand efficiently by minimizing costs while adhering to the constraints of each power plant.

## Prerequisites

Before running the project, make sure you have the following installed:

- **[.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/sdk-8.0.403-windows-x64-installer)**
- **[Visual Studio 2022](https://visualstudio.microsoft.com/)**
- **[Docker](https://www.docker.com/products/docker-desktop/)**

## Getting Started

### 1. **Start project through command line:**

Open folder where the project is located and run the following command in the command line:

```bash
dotnet restore
```

```bash
dotnet run --project Powerplants.Api
```

Swagger of the project will be avaible on https://localhost:8888/swagger/index.html

### 2. **Start project through visual studio:**

After installing Visual Studio, make sure to install the **ASP.NET and web development** workload.

Once installed, Open folder where the project is located and ckick on the **Powerplant.sln** file. In the Solution Explorer, right-click on the **Powerplant** project and select **Set as Startup Project**. Then, press **F5** to start the project.

In either cases swagger of the project will be avaible on https://localhost:8888/swagger/index.html

### 3. **Start project through Docker:**

To run the API in a Docker container, follow these steps:

1. **Build the Docker image:**

   ```bash
   docker build -t powerplants-api .
   ```

2. **Run the Docker container:**

   ```bash
   docker run -p "8888:8888" powerplants-api
   ```

   The API will be accessible at `http://localhost:8888`.

## API Usage

### Endpoint

- **Method**: `POST`
- **URL**: `/productionplan`

The API accepts a request that contains 3 types of data:

- load: The energy (MWh) that needs to be generated in one hour.
- fuels: Determines the merit order based on fuel costs, guiding which power plants to activate and how much power they deliver

  - gas (euro/MWh): The price of gas per MWh. For example, at €6/MWh with 50% efficiency, generating 1 MWh costs €12.
  - kerosine(euro/Mwh): the price of kerosine per MWh.
  - co2(euro/ton): Price of emission allowances.
    -wind(%): Wind percentage; e.g., a 25% wind availability means a wind turbine with a Pmax of 4 MW generates 1 MWh.
  - name : Identifier of the power plant.
  - type: gasfired, turbojet or windturbine.
  - efficiency: the efficiency at which they convert a MWh of fuel into a MWh of electrical energy. Wind-turbines do not consume 'fuel' and thus are considered to generate power at zero price.
  - pmax: Maximum power output.
  - pmin: Minimum power output when activated..

### Sample payload

```json
{
  "load": 910,
  "fuels": {
    "gas(euro/MWh)": 13.4,
    "kerosine(euro/MWh)": 50.8,
    "co2(euro/ton)": 20,
    "wind(%)": 60
  },
  "powerplants": [
    {
      "name": "gasfiredbig1",
      "type": "gasfired",
      "efficiency": 0.53,
      "pmin": 100,
      "pmax": 460
    },
    {
      "name": "gasfiredbig2",
      "type": "gasfired",
      "efficiency": 0.53,
      "pmin": 100,
      "pmax": 460
    },
    {
      "name": "gasfiredsomewhatsmaller",
      "type": "gasfired",
      "efficiency": 0.37,
      "pmin": 40,
      "pmax": 210
    },
    {
      "name": "tj1",
      "type": "turbojet",
      "efficiency": 0.3,
      "pmin": 0,
      "pmax": 16
    },
    {
      "name": "windpark1",
      "type": "windturbine",
      "efficiency": 1,
      "pmin": 0,
      "pmax": 150
    },
    {
      "name": "windpark2",
      "type": "windturbine",
      "efficiency": 1,
      "pmin": 0,
      "pmax": 36
    }
  ]
}
```

### Sample Response

```json
[
  {
    "name": "windpark1",
    "p": 90
  },
  {
    "name": "windpark2",
    "p": 21.6
  },
  {
    "name": "gasfiredbig1",
    "p": 460
  },
  {
    "name": "gasfiredbig2",
    "p": 338.4
  },
  {
    "name": "gasfiredsomewhatsmaller",
    "p": 0
  },
  {
    "name": "tj1",
    "p": 0
  }
]
```

### Testing with Swagger

The API uses **Swagger** for interactive documentation. Once the API is running, you can access the Swagger UI at:

```
http(s)://localhost:8888/swagger/index.html
```

This interface allows you to explore and test the available endpoint.

## Testing

The project includes unit tests using **xUnit**. To run the tests, use:

```bash
dotnet test
```

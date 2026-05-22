# Powerplant Coding Challenge

REST API to calculate an optimal power production plan based on the merit-order.

## Architecture

The project uses a Clean Architecture with 3 layers:

- **PowerplantCodingChallenge.API** — Controllers, middleware, Swagger config
- **PowerplantCodingChallenge.Application** — Services, DTOs, interfaces
- **PowerplantCodingChallenge.Domain** — Entities, enums

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

## Running the Application

### Command Line

```bash
git clone git@github.com:gokhanguneyDav/powerplant-coding-challenge.git
cd PowerplantCodingChallenge
dotnet build
dotnet run --project PowerplantCodingChallenge.API
```

### Visual Studio

1. Open `PowerplantCodingChallenge.sln`
2. Set `PowerplantCodingChallenge.API` as the startup project
3. Press F5

## API

The API is documented with Swagger/OpenAPI. Once running, access it at http://localhost:8888/swagger.

### Endpoint

```
POST http://localhost:8888/productionplan
Content-Type: application/json
```

### Request Example

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
    { "name": "gasfiredbig1", "type": "gasfired", "efficiency": 0.53, "pmin": 100, "pmax": 460 },
    { "name": "windpark1", "type": "windturbine", "efficiency": 1, "pmin": 0, "pmax": 150 }
  ]
}
```

### Response Example

```json
[
  { "name": "windpark1", "p": 90.0 },
  { "name": "gasfiredbig1", "p": 460.0 }
]
```

## Algorithm

The algorithm uses the merit-order to decide which powerplants to activate:

1. Compute cost per MWh for each plant (wind = 0, gas = `gas_price / efficiency`, turbojet = `kerosine_price / efficiency`)
2. Sort by ascending cost (merit-order)
3. Allocate power greedily while respecting Pmin/Pmax constraints and matching the total load exactly (rounded to 0.1 MW)

When the remaining load falls below a plant's Pmin, the algorithm tries to reduce output from previously activated plants to make room.

## Docker

Requires [Docker](https://www.docker.com/get-started) with Compose.

```bash
docker-compose up -d
docker-compose logs -f
```

The API will be available at http://localhost:8888/productionplan.

## Testing

```bash
curl -X POST http://localhost:8888/productionplan \
  -H "Content-Type: application/json" \
  -d @example_payloads/payload3.json
```

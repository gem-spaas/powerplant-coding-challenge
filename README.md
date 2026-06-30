# Power Plant Coding Challenge

A REST API that computes a production plan: given a load and a set of power
plants, it decides how much power each plant should produce so that the total
exactly matches the load at the lowest possible cost.

## Requirements

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

## Build and run

From the repository root:

```bash
dotnet run --project PowerPlant.Api
```

The API listens on **http://localhost:8888**.

When running in Development, an interactive Swagger UI is available at
http://localhost:8888/swagger to try the endpoint from the browser.

## Usage

Send a `POST` request to `/productionplan` with a payload describing the load,
the fuel prices and the available power plants:

```bash
curl -X POST http://localhost:8888/productionplan \
     -H "Content-Type: application/json" \
     -d @example_payloads/payload3.json
```

The response lists, for every plant, the power it should deliver. Each value is
a multiple of 0.1 MW, and the sum of all values equals the requested load:

```json
[
  { "name": "windpark1", "p": 90.0 },
  { "name": "windpark2", "p": 21.6 },
  { "name": "gasfiredbig1", "p": 460.0 },
  { "name": "gasfiredbig2", "p": 338.4 },
  { "name": "gasfiredsomewhatsmaller", "p": 0.0 },
  { "name": "tj1", "p": 0.0 }
]
```

## Running the tests

```bash
dotnet test
```

## How the calculation works

The problem is solved in two separate steps:

1. **Which plants run?** Every plant is either on or off, so the on/off
   combinations are enumerated and each one is evaluated. The cheapest feasible
   combination wins. This brute-force search is intentional: it is provably
   correct (no combination is ever missed) and the challenge explicitly asks not
   to rely on an external solver.
2. **How much does each running plant produce?** For a fixed set of running
   plants, every plant starts at its minimum, and the remaining load is filled
   starting with the cheapest plant first. A final rounding step aligns each
   output to 0.1 MW while keeping the total exactly equal to the load.

Plant behaviour (cost, minimum and maximum output) is modelled through
polymorphism, so the algorithm never inspects a plant's concrete type. Wind
turbines, whose output is fixed at `pmax * wind%`, are handled the same way as
any other plant.

The CO2 cost of gas-fired plants is taken into account (0.3 ton per MWh
produced).

## Project structure

| Project | Responsibility |
| --- | --- |
| `PowerPlant.Domain` | Plant types and the planning algorithm. No dependencies. |
| `PowerPlant.Application` | Orchestration: maps the request, builds the plants, runs the planner. |
| `PowerPlant.Api` | REST endpoint, JSON contract, error handling. |
| `PowerPlant.Domain.UnitTests` | Unit tests for the algorithm. |

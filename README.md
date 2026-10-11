# ProductionPlan

## Overview
REST API for calculating an electricity production plan based on demand, power plant constraints, and energy prices.

## Features
- Merit-order dispatch based on production costs.
- Wind power prioritization.
- Power plant capacity constraints (`Pmin`, `Pmax`).
- Fuel prices and plant efficiency.
- Insufficient production capacity detection.
- Structured error handling with the Result Pattern.

## Architecture
- **ProductionPlanHost**: REST API and HTTP endpoints.
- **Application**: Use cases, MediatR handlers, validation and DTO mappings.
- **Domain**: Business rules, domain entities and production calculations.
- **SharedKernel**: Shared abstractions and Result Pattern.
- **DomainTests**: Domain logic and production calculation tests.
- **ApplicationTests**: Application handlers and use-case tests.

## Tech Stack
- .NET 10 / ASP.NET Core
- C#
- MediatR
- FluentValidation
- xUnit
- Dependency Injection
- REST / JSON
- CARTER
- Minimal APIs
- NetArchTest
- Central Package Management (CPM)

## Getting Started

### Prerequisites
- .NET 10 SDK

### Build
```bash
dotnet restore
dotnet build
```

### Run
```bash
dotnet run --project ProductionPlanHost
```

### Run Tests
```bash
dotnet test
```

## Production Logic
1. Calculate available wind production.
2. Sort conventional power plants by production cost.
3. Allocate production within plant capacity constraints.
4. Verify whether the requested load can be satisfied.
5. Return the production plan or an error.


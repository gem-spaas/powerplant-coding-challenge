# Powerplant Coding Challenge

This project contains an API written in .NET8 with the Clean Architecture principles alongside DDD, MediaR for mapping objects betweens layers.
The project also contain unit tests and integration tests using xUnit for the integration tests and nUnit for unit tests.

## Dependencies

The project uses the following dependencies:

- **MediaR**: A lightweight library that provides a mediator pattern implementation for .NET.
- **FluentValidation & FluentAssertion**: A validation library that provides a fluent API for validating objects.
- **Moq**: A library that help to mock external dependencies.
- **.NET8 SDK**
- A working IDE like Rider or Visual Studio

## Getting Started

1. Clone this repository
2. Open the solution file `PowerPlantCodingChallenge.sln` in your preferred IDE.
3. Build the project in order to get the Nuget packages (ctrl+shift+b for the shortcut).
4. Select the configuration `PowerPlant.Api: http` or `PowerPlant.Api: https` and run the API project.
5. A swagger UI will be trigerred in your browser on the following url `http://localhost:8888/swagger/index.html`.
6. Use the `POST /ProductionPlan` to send the payload that you want to the following url exposed `http://localhost:8888/ProductionPlan`

## Payload

The payload contains 3 types of data:
 - load: The load is the amount of energy (MWh) that need to be generated during one hour.
 - fuels: based on the cost of the fuels of each powerplant, the merit-order can be determined which is the starting point for deciding which powerplants should be switched on and how much power they will deliver.  Wind-turbine are either switched-on, and in that case generate a certain amount of energy depending on the % of wind, or can be switched off. 
   - gas(euro/MWh): the price of gas per MWh. Thus if gas is at 6 euro/MWh and if the efficiency of the powerplant is 50% (i.e. 2 units of gas will generate one unit of electricity), the cost of generating 1 MWh is 12 euro.
   - kerosine(euro/Mwh): the price of kerosine per MWh.
   - co2(euro/ton): the price of emission allowances (optionally to be taken into account).
   - wind(%): percentage of wind. Example: if there is on average 25% wind during an hour, a wind-turbine with a Pmax of 4 MW will generate 1MWh of energy.
 - powerplants: describes the powerplants at disposal to generate the demanded load. For each powerplant is specified:
   - name:
   - type: gasfired, turbojet or windturbine.
   - efficiency: the efficiency at which they convert a MWh of fuel into a MWh of electrical energy. Wind-turbines do not consume 'fuel' and thus are considered to generate power at zero price.
   - pmax: the maximum amount of power the powerplant can generate.
   - pmin: the minimum amount of power the powerplant generates when switched on. 

   Exemple value of the payload :

```json
{
  "load": 20,
  "fuels": {
    "gas(euro/MWh)": 1.0,
    "kerosine(euro/MWh)": 1.0,
    "co2(euro/ton)": 1.0,
    "wind(%)": 60
  },
  "powerplants": [
    {
      "name": "windpark",
      "type": "windturbine",
      "efficiency": 1,
      "pmin": 0,
      "pmax": 100
    }
  ]
}
```

## Response

Each powerplant will specify how much power they should deliver. 
The power produced by each powerplant is a multiple of 0.1 Mw and the sum of the power produced by all the powerplants together is equal the load.

Exemple value of the Response : 

```json
[
    {
        "name": "windpark",
        "p": 20.0
    }
]
```
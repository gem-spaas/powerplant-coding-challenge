# PowerPlant
This project is an implementation of the [challenge](https://github.com/gem-spaas/powerplant-coding-challenge) made by Engie [GEM](https://gems.engie.com/) team.

# Pre requirements
The project is using .NET 8 so, if you use Visual Studio or Visual Studio Code, make sure that : 
- .NET 8 SDK is installed
- Visual Studio is up-to-date if you use it

# Start the project

To start the project, you can either :
- Use Docker
- Use dotnet CLI command
- Use Visual Studio

## Use Docker
In the root of the solution, build the docker image by running the following command : 
```
docker build -t power-plant-api -f Dockerfile .
```

And then run the container with the following command : 
```
docker run -d -p 8888:8888 --name power-plant-api power-plant-api
```

## Use CLI command

Head in the api folder of the project and run it with the following commands :
```
cd .\PowerPlantCC.Api\
dotnet run
```

## Use Visual studio
Open ```PowerPlantCC.sln``` solution file and run the solution.

# Project feature

The project exposes the following endpoint :
```
POST http://localhost:8888/productionplan
```

The purpose of this endpoint is to calculate the usage of given powerplants to reach a specific given power load in the most cost effective way by taking into account different parameters such as fuels costs, CO2 production cost and wind.

It expects a body with the following structure : 

```json
{
  "load": 0.0,
  "fuels":{
    "gas(euro/MWh)": 0.0,
    "kerosine(euro/MWh)": 0.0,
    "co2(euro/ton)": 0.0,
    "wind(%)": 0.0
  },
  "powerplants": [
    {
      "name": "string",
      "type": "string",
      "efficiency": 0.0,
      "pmin": 0,
      "pmax": 0
    }
  ]
}
```

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


And returns the power produced by each given powerplants with the following structure :

```json
[
    {
        "name": "string",
        "p": 0.0
    }
]
```
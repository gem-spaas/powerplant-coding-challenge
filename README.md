# powerplant-coding-challenge


## Welcome !

This is an implementation of [SPaaS - Global Energy Management Solutions Challenge](https://github.com/gem-spaas/powerplant-coding-challenge).
If you want to run this code in the challenge please make sure you have all the files in ProductionPlanApi on your local machine.
Follow the instructions below 

## 1. Prerequisites
Please make sure you have dotnet core installed on the machine you are running

	1.1. In case you need to to download please use https://dotnet.microsoft.com/en-us/download
	1.2. to test the dotnet environment please run this command dotnet --version you should see
		> dotnet --version
		> 6.0.201
	Version higher than 6.0 is proper for the project


## 2. Running the project

2.1 First option 

	Run Command :	dotnet run --project "<local path to the project>"

				    ***local path example : D:\Challenge\ProductionPlanApi
	application Runs on  https://localhost:8888/productionplan  

2.2 Second Option 

	step 1: select the project directory
	step 2: Run command : dotnet run

	If everything is installed correctly you should see in the terminal the following:
		
	Building...
	info: Microsoft.Hosting.Lifetime[14]
      	Now listening on: https://localhost:8888       
	info: Microsoft.Hosting.Lifetime[0]
      	Application started. Press Ctrl+C to shut down.
	info: Microsoft.Hosting.Lifetime[0]
      	Hosting environment: Production
	info: Microsoft.Hosting.Lifetime[0]
      	Content root path: D:\Challenge\ProductionPlanApi
	
## 3. Testing the project
Within ProductionPlanApi/Postman you can find a simple Postman test collection to consume the services exposed by the ProductionPlanAPi.
Import the ProductionPlan.postman_collection.json into postman to easily test

## The challenge

### In short
Calculate how much power each of a multitude of different [powerplants](https://en.wikipedia.org/wiki/Power_station) need 
to produce (a.k.a. the production-plan) when the [load](https://en.wikipedia.org/wiki/Load_profile) is given
and taking into account the cost of the underlying energy sources (gas,  kerosine) and the Pmin and Pmax of each powerplant.

### More in detail

The load is the continuous demand of power. The total load at each moment in time is forecasted. For instance
for Belgium you can see the load forecasted by the grid operator [here](https://www.elia.be/en/grid-data/load-and-load-forecasts).

At any moment in time, all available powerplants need to generate the power to exactly match the load.
The cost of generating power can be different for every powerplant and is dependent on external factors:
The cost of producing power using a [turbojet](https://en.wikipedia.org/wiki/Gas_turbine#Industrial_gas_turbines_for_power_generation), 
that runs on kerosine, is higher compared to the cost of generating power 
using a gas-fired powerplant because of gas being cheaper compared to kerosine and because of the 
[thermal efficiency](https://en.wikipedia.org/wiki/Thermal_efficiency) of a gas-fired powerplant being around
50% (2 units of gas will generate 1 unit of electricity) while that of a turbojet is only around 30%.
The cost of generating power using windmills however is zero. Thus deciding which powerplants to
activate is dependent on the [merit-order](https://en.wikipedia.org/wiki/Merit_order).

When deciding which powerplants in the merit-order to activate 
(a.k.a. [unit-commitment problem](https://en.wikipedia.org/wiki/Unit_commitment_problem_in_electrical_power_production)) 
the maximum amount of power each powerplant can produce (Pmax) obviously needs to be taken into account. 
Additionally gas-fired powerplants generate a certain minimum amount of power when switched on, called the Pmin. 


### Performing the challenge

Build a REST API exposing an endpoint `/productionplan` that accepts a POST with a payload as you can 
find in the `example_payloads` directory and that returns a json with the same structure as 
in `example_response.json` and that manages and logs run-time errors.

For calculating the unit-commitment, we prefer you not to rely on an existing (linear-programming) solver but
instead write an algorithm yourself.

Implementations can be submitted in either C# (on .Net 5 or higher) or Python (3.8 or higher) as these are (currently) 
the main languages we use in SPaaS.
Along with the implementation should be a README that describes how to compile (if applicable) and launch the application.

- C# implementations should contain a solutions file to compile the application. 
- Python implementations should contain
a `requirements.txt` or a `pyproject.toml` (for use with poetry) to install all needed dependencies.

#### Payload

The payload contains 3 types of data:
 - load: The load is the amount of energy (MWh) that need to be generated during one hour.
 - fuels: based on the cost of the fuels of each powerplant, the merit-order can be determined which is the starting
 point for deciding which powerplants should be switched on and how much power they will deliver.
 Wind-turbine are either switched-on, and in that case generate a certain amount of energy 
 depending on the % of wind, or can be switched off. 
   - gas(euro/MWh): the price of gas per MWh. Thus if gas is at 6 euro/MWh and if the efficiency of the powerplant is 50%
   (i.e. 2 units of gas will generate one unit of electricity), the cost of generating 1 MWh is 12 euro.
   - kerosine(euro/Mwh): the price of kerosine per MWh.
   - co2(euro/ton): the price of emission allowances (optionally to be taken into account).
   - wind(%): percentage of wind. Example: if there is on average 25% wind during an hour, a wind-turbine 
   with a Pmax of 4 MW will generate 1MWh of energy.
 - powerplants: describes the powerplants at disposal to generate the demanded load. For each powerplant.
 is specified:
   - name:
   - type: gasfired, turbojet or windturbine.
   - efficiency: the efficiency at which they convert a MWh of fuel into a MWh of electrical energy.
   Wind-turbines do not consume 'fuel' and thus are considered to generate power at zero price.
   - pmax: the maximum amount of power the powerplant can generate.
   - pmin: the minimum amount of power the powerplant generates when switched on. 

#### response

The response should be a json as in `example_response.json`, specifying for each powerplant how much 
power each powerplant should deliver. The power produced by each powerplant has to be a multiple
of 0.1 Mw and the sum of the power produced by all the powerplants together should
equal the load. 

### Want more challenge?

Having fun with this challenge and want to make it more realistic. Optionally, do one of the extra's below:

#### Docker

Provide a Dockerfile along with the implementation to allow deploying your solution quickly.

#### CO2

Taken into account that a gas-fired powerplant also emits CO2, the cost of running the powerplant should
also take into account the cost of the [emission allowances](https://en.wikipedia.org/wiki/Carbon_emission_trading).
For this challenge, you may take into account that each MWh generated creates 0.3 ton of CO2. 

#### Websocket

Provide a websocket server connection that will emit after every post the input of the POST together with
the response to every client connected on the websocket.

## Acceptance criteria

For a submission to be reviewed as part of an application for a position in the team, the project
needs to:
  - contain a README.md explaining how to build and launch the API
  - expose the API on port `8888`
  - return a result where the sum of the power generated by each of the different powerplants is
  exactly equal to the load specified in the payload for at least the example payloads provided.

Failing to comply with any of these criteria will automatically disqualify the submission.

## More info

For more info on energy management, check out:

 - [Global Energy Management Solutions](https://www.youtube.com/watch?v=SAop0RSGdHM)
 - [COO hydroelectric power station](https://www.youtube.com/watch?v=edamsBppnlg)
 - [Management of supply](https://www.youtube.com/watch?v=eh6IIQeeX3c) - video made during winter 2018-2019

## FAQ

##### Can an existing solver be used to calculate the unit-commitment
Implementations should not rely on an external solver and thus contain an algorithm written
from scratch (clarified in the text as of version v1.1.0)


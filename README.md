# Powerplant Coding Challenge Solution by Burak Cetin

## Run With Docker

Download the source code and go to the root of the repository.<br>
```
docker build -t production_plan_api_server .
docker run -d -p 8888:8888 production_plan_api_server
```
It will start a container in the background.

## Run Locally

Download the source code and go to the root of the repository.<br><br>
Create the virtual environment and install the requirements:
```
python3 -m venv env
source env/bin/activate
pip3 install -r requirements.txt
```
Run the app (by default it will run on port 8888):
```
python3 src/api_server.py
```

## Run Tests

Run all the tests from the repository root:
```
python3 -m unittest tests/tests.py
```

## Understand the Problem
Main problem is that **we can not store the electricity in a cheap way**. Because of that we need to predict the **demand** about electricity consumption. To meet this electricity demand, there are different types of power plants, like ones using **gas, kerosine, or even windmills**. Each power plant has its own **cost** to produce electricity and the cost generally is like below for all of them.
- Windmills (0) < gas (\$) < kerosine (\$$)

<br>There are 3 types of **powerplant** for 3 types of energy

1. Gasfired (gas)
2. Turbojet (kerosine)
3. Windturbine (wind)

<p>
An energy production company may utilize various types of power plants to fulfill electricity demand. In addition, gasfired powerplants might incur different efficiency, but gas price is same.
</p>

<p>
So, at any given moment, we need to decide which power plants to turn on to meet the electricity demand in the most cost-effective way. This is where the "unit-commitment problem" comes in. We have to figure out the best combination of power plants to use, considering their costs and how much electricity they can produce.
</p>

<p>
Here's another thing to consider: Some power plants have a minimum amount of electricity they have to produce when they're turned on. We call this the "Pmin." So, when deciding which power plants to activate, we need to think about both their maximum capacity (Pmax) and this minimum amount they have to generate (Pmin).
</p>

<p>
The goal is to supply the needed electricity at the lowest cost by choosing the right combination of power plants, considering their different costs, capacities, and minimum production requirements.
</p>

## Solution
<p>The primary challenge lies in establishing the merit order, a crucial factor in determining the activation sequence of power plants and the corresponding power output. This sequence is determined based on the cost of fuels associated with each power plant, forming the foundation for decisions on which power plants to activate and the quantity of power they will generate.<p>

<p>Certainly! We aim to determine the minimum cost of meeting electricity demand by considering the constraints of various power plants. The selection process relies on the cost of energy production from these power plants. This involves employing a **uniform cost search algorithm**. Unlike having a predefined map, path, or graph illustrating the sequence of power plants in operation, we will generate multiple path options by initiating each power plant. Subsequently, we will choose the path with the lowest cost for our electricity production.</p>
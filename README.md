# powerplant-coding-challenge

<hr>

### Deploy and run using Poetry:
1. **Get project** 
```
git clone https://github.com/shizde/powerplant-coding-challenge.git
cd powerplant-coding-challenge
```

2. **Install Poetry:**
If you don’t have Poetry installed, you can install it with:

```
curl -sSL https://install.python-poetry.org | python3 -
```

3. **Install Dependencies:** Run the following command in the root of your project (where the file pyproject.toml is located):

```
poetry install
```

4. **Run the Project:** you can run your API with the following command:

```
poetry run python src/app.py
```

5. **Testing API Call:** On a terminal, run the following command

```
curl --location '127.0.0.1:8888/productionplan' \
--header 'Content-Type: application/json' \
--data '{
  "load": 910,
  "fuels":
  {
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
'
```